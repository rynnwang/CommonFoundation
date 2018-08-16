using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Beyova.ExceptionSystem;
using Newtonsoft.Json.Linq;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityShell.
    /// </summary>
    internal class GravityShell
    {
        /// <summary>
        /// The _client key
        /// </summary>
        private Guid? _clientKey = null;

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        public GravityClient Client { get; protected set; }

        /// <summary>
        /// Gets or sets the entry.
        /// </summary>
        /// <value>The entry.</value>
        public GravityEntryObject Entry { get; protected set; }

        /// <summary>
        /// Gets or sets the instruction invokers.
        /// </summary>
        /// <value>
        /// The instruction invokers.
        /// </value>
        public Dictionary<string, IGravityInstructionInvoker> InstructionInvokers { get; protected set; }

        /// <summary>
        /// Gets or sets the configuration reader.
        /// </summary>
        /// <value>The configuration reader.</value>
        public GravityConfigurationReader ConfigurationReader { get; protected set; }

        /// <summary>
        /// Gets or sets the component attribute.
        /// </summary>
        /// <value>The component attribute.</value>
        public BeyovaComponentAttribute ComponentAttribute { get; protected set; }

        /// <summary>
        /// Gets or sets the watcher thread.
        /// </summary>
        /// <value>The watcher thread.</value>
        public Thread WatcherThread { get; internal protected set; }

        /// <summary>
        /// Gets the information.
        /// </summary>
        /// <value>The information.</value>
        internal dynamic Info
        {
            get
            {
                return new
                {
                    Uri = this.Client.Entry.GravityServiceUri,
                    ConfigurationName = this.Client.Entry.ConfigurationName,
                    Actions = this.InstructionInvokers.Keys
                };
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GravityShell" /> class.
        /// </summary>
        /// <param name="componentAttribute">The component attribute.</param>
        /// <param name="sourceAssembly">The source assembly.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="commandInvokers">The command invokers.</param>
        protected GravityShell(BeyovaComponentAttribute componentAttribute, string sourceAssembly, GravityEntryObject entry, IEnumerable<IGravityInstructionInvoker> commandInvokers)
        {
            this.Entry = entry;
            this.Client = new GravityClient(entry);
            this.InstructionInvokers = commandInvokers.AsDictionary((x) => x?.Type, StringComparer.OrdinalIgnoreCase);

            this.ComponentAttribute = componentAttribute;
            this.ConfigurationReader = new GravityConfigurationReader(this.Client, sourceAssembly, componentAttribute?.UnderlyingObject?.Version, entry.ConfigurationName);
            this.WatcherThread = new Thread(new ThreadStart(Watch))
            {
                IsBackground = true
            };

            this.WatcherThread.Start();
        }

        /// <summary>
        /// Invokes the instruction.
        /// </summary>
        /// <param name="invoker">The invoker.</param>
        /// <param name="request">The instruction.</param>
        protected void InvokeInstruction(IGravityInstructionInvoker invoker, GravityInstruction request)
        {
            JToken result = null;
            BaseException exception = null;

            if (invoker != null && request != null && request.Key.HasValue)
            {
                try
                {
                    result = invoker.Invoke(request.Action, request.Parameters);
                }
                catch (Exception ex)
                {
                    exception = ex.Handle(new { request });
                }

                Client.SubmitInstructionResult(new GravityInstructionResult
                {
                    Key = Guid.NewGuid(),
                    Detail = result,
                    ClientKey = _clientKey,
                    InstructionKey = request.Key,
                    Exception = exception?.ToExceptionInfo()
                });
            }
        }

        /// <summary>
        /// Processes the instructions.
        /// </summary>
        /// <param name="instructionRequest">The instruction request.</param>
        protected void ProcessInstructions(List<GravityInstruction> instructionRequest)
        {
            if (instructionRequest.HasItem())
            {
                foreach (var item in instructionRequest)
                {
                    if (item != null && !string.IsNullOrWhiteSpace(item.Action))
                    {
                        IGravityInstructionInvoker invoker;
                        if (InstructionInvokers.TryGetValue(item.Type, out invoker))
                        {
                            InvokeInstruction(invoker, item);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Watches this instance.
        /// </summary>
        protected void Watch()
        {
            while (true)
            {
                try
                {
                    var machineHealth = new Heartbeat
                    {
                        ConfigurationName = this.ConfigurationReader?.ConfigurationName
                    };
                    machineHealth.FillHealthData();
                    var echo = Client.Heartbeat(machineHealth);

                    _clientKey = echo.ClientKey;
                    ProcessInstructions(echo.Instructions);
                }
                catch { }

                //1 min
                Thread.Sleep(60000);
            }
        }

        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        internal static GravityShell Current { get; } = InitializeGravityHost();

        /// <summary>
        /// Initializes the gravity host.
        /// </summary>
        /// <returns>Beyova.Gravity.GravityHost.</returns>
        private static GravityShell InitializeGravityHost()
        {
            HashSet<IGravityInstructionInvoker> invokers = new HashSet<IGravityInstructionInvoker>();
            invokers.Add(new UpdateConfigurationCommandInvoker());
            invokers.Add(new GravityMethodInvoker());
            invokers.Add(new GravityDynamicMethodInvoker());

            GravityEntryObject entryObject = null;
            BeyovaComponentAttribute componentAttribute = null;
            string assemblyName = null;

            bool toFindMoreEntry = true;
            foreach (var assembly in EnvironmentCore.AscendingAssemblyDependencyChain)
            {
                var commandActionAttribute = assembly.GetCustomAttribute<GravityCommandActionAttribute>();
                if (commandActionAttribute != null)
                {
                    foreach (var one in commandActionAttribute.Invokers)
                    {
                        invokers.Add(one);
                    }
                }

                if (toFindMoreEntry)
                {
                    var protocolAttribute = assembly.GetCustomAttribute<GravityProtocolAttribute>();
                    if (protocolAttribute != null)
                    {
                        entryObject = protocolAttribute.Entry;
                        assemblyName = assembly.FullName;
                        componentAttribute = assembly.GetCustomAttribute<BeyovaComponentAttribute>();

                        if (protocolAttribute.IsSealed)
                        {
                            toFindMoreEntry = false;
                        }
                    }
                }
            }

            return entryObject == null ? null : new GravityShell(componentAttribute, assemblyName, entryObject, invokers);
        }

        /// <summary>
        /// Reloads the configuration.
        /// </summary>
        /// <returns></returns>
        internal static JToken ReloadConfiguration()
        {
            try
            {
                var configurationReader = Current.ConfigurationReader;

                configurationReader.CheckNullObject(nameof(configurationReader));
                configurationReader.Reload();

                return configurationReader.Hash.ToJson(false);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { localTime = DateTime.Now });
            }
        }
    }
}