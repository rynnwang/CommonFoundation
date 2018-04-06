using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;

namespace Beyova.Azure
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiTrackingLake
    {
        /// <summary>
        /// The client
        /// </summary>
        protected DocumentClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTrackingLake"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="token">The token.</param>
        public ApiTrackingLake(Uri endpoint, string token)
        {
            if (endpoint != null && !string.IsNullOrWhiteSpace(token))
            {
                _client = new DocumentClient(endpoint, token);
                
            }
        }
    }
}
