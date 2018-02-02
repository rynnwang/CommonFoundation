using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova.Gravity.DataAccessController
{
    /// <summary>
    /// Class GravityAccessController.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GravityAccessController<T> : BaseDataAccessController<T>
    {
        /// <summary>
        /// The column_ host name
        /// </summary>
        protected const string column_HostName = "HostName";

        /// <summary>
        /// The column_ server name
        /// </summary>
        protected const string column_ServerName = "ServerName";

        /// <summary>
        /// The column_ request key
        /// </summary>
        protected const string column_RequestKey = "RequestKey";

        /// <summary>
        /// The column_ action
        /// </summary>
        protected const string column_Action = "Action";

        /// <summary>
        /// The column_ parameters
        /// </summary>
        protected const string column_Parameters = "Parameters";

        /// <summary>
        /// The column_ client key
        /// </summary>
        protected const string column_ClientKey = "ClientKey";

        /// <summary>
        /// The column_ configuration
        /// </summary>
        protected const string column_Configuration = "Configuration";

        /// <summary>
        /// The column_ configuration name
        /// </summary>
        protected const string column_ConfigurationName = "ConfigurationName";

        /// <summary>
        /// The column_ last heartbeat stamp
        /// </summary>
        protected const string column_LastHeartbeatStamp = "LastHeartbeatStamp";

        /// <summary>
        /// The column_ clientkey
        /// </summary>
        protected const string column_Clientkey = "column_Clientkey";

        /// <summary>
        /// Initializes a new instance of the <see cref="GravityAccessController{T}" /> class.
        /// </summary>
        public GravityAccessController()
            : base()
        {
        }
    }
}
