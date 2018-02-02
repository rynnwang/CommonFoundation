using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova.ExceptionSystem;

namespace Beyova.Gravity.TestClient
{
    public class TestClientEventHook : GravityEventHook
    {
        /// <summary>
        /// Called when [processing command].
        /// </summary>
        /// <param name="invoker">The invoker.</param>
        /// <param name="command">The command.</param>
        public override void OnProcessingCommand(IGravityCommandInvoker invoker, GravityCommandRequest command)
        {
            base.OnProcessingCommand(invoker, command);

            Console.WriteLine("{0}: Processing command {1} with parameter: {2}", DateTime.Now.ToFullDateTimeString(), invoker?.Action, command.ToJson());
            Console.WriteLine();
        }

        /// <summary>
        /// Called when [secured message processed completed].
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="exception">The exception.</param>
        public override void OnSecuredMessageProcessedCompleted(string feature, object parameter, BaseException exception)
        {
            base.OnSecuredMessageProcessedCompleted(feature, parameter, exception);

            Console.WriteLine("{0}: Processing secured message for feature {1}.", DateTime.Now.ToFullDateTimeString(), feature);
            Console.WriteLine("Exception: {0}", exception?.ToExceptionInfo()?.ToJson().SafeToString("N/A"));
            Console.WriteLine();
        }
    }
}
