using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Beyova.ServicePortal
{
    /// <summary>
    /// Class HighChartsSeriesDataModel.
    /// </summary>
    public class HighChartsSeriesDataModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        [JsonProperty(PropertyName = "data")]
        public List<decimal> Data { get; set; }

        /// <summary>
        /// Gets or sets the name of the stack.
        /// </summary>
        /// <value>The name of the stack.</value>
        [JsonProperty(PropertyName = "stack")]
        public string StackName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HighChartsSeriesDataModel"/> class.
        /// </summary>
        public HighChartsSeriesDataModel()
        {
            this.Data = new List<decimal>();
        }
    }
}
