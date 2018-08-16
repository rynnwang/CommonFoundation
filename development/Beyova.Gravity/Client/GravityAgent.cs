using System;
using Beyova.ExceptionSystem;
using Beyova.VirtualSecuredTransferProtocol;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityAgent. It is abstract and very low level, to control basic communication actions.
    /// </summary>
    internal abstract class GravityAgent
    {
        /// <summary>
        /// Gets or sets the entry.
        /// </summary>
        /// <value>The entry.</value>
        public GravityEntryObject Entry { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GravityAgent" /> class.
        /// </summary>
        /// <param name="entry">The entry.</param>
        protected GravityAgent(GravityEntryObject entry)
        {
            entry.CheckNullObject(nameof(entry));
            this.Entry = entry;
        }

        /// <summary>
        /// Invokes the specified feature.
        /// </summary>
        /// <typeparam name="TIn">The type of the t in.</typeparam>
        /// <typeparam name="TOut">The type of the t out.</typeparam>
        /// <param name="module">The module.</param>
        /// <param name="action">The action.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        /// TOut.
        /// </returns>
        protected TOut Invoke<TIn, TOut>(string module, string action, TIn parameter)
        {
            BaseException exception = null;

            try
            {
                return VirtualSecuredTransferProtocolHelper.Invoke<TIn, TOut>(
                GetInvokeUri(module, action),
                new ClassicVirtualSecuredRequestMessagePackage<TIn>
                {
                    Object = parameter,
                    Token = this.Entry.SecrectKey
                },
                new RsaKeys { PublicKey = this.Entry.PublicKey }.CreateRsaPublicProvider());
            }
            catch (Exception ex)
            {
                exception = ex.Handle(new { action, parameter });
                throw exception;
            }
        }

        /// <summary>
        /// Gets the invoke URI.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="action">The feature.</param>
        /// <returns>
        /// Uri.
        /// </returns>
        protected Uri GetInvokeUri(string module, string action)
        {
            return new Uri(string.Format("{0}{1}/{2}/", this.Entry.GravityServiceUri.ToString().EnsureEndWith('/'), module, action));
        }
    }
}