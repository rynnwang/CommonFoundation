using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Beyova;
using Beyova.Http;

namespace Beyova.Elastic
{
    partial class ElasticClient
    {
        /// <summary>
        /// The default time out
        /// </summary>
        const int defaultTimeOut = 10000;

        /// <summary>
        /// The default HTTP client
        /// </summary>
        static HttpClient defaultHttpClient = new HttpClient { Timeout = new TimeSpan(defaultTimeOut * TimeSpan.TicksPerMillisecond) };

        /// <summary>
        /// Indexes the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="F"></typeparam>
        /// <param name="indexObject">The index object.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>
        /// Task&lt;System.String&gt;.
        /// </returns>
        public async Task<string> IndexAsync<T, F>(IElasticWorkObject<T, F> indexObject, int? timeout = null)
        {
            try
            {
                indexObject.CheckNullObject(nameof(indexObject));

                if (!string.IsNullOrWhiteSpace(indexObject.IndexName)
                  && !string.IsNullOrWhiteSpace(indexObject.Type)
                  && indexObject.RawData != null)
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, GetHttpRequestUri(indexObject.IndexName, indexObject.Type));
                    request.FillJsonObject(indexObject.RawData);
                    return await request.ReadResponseAsText((timeout.HasValue && timeout.Value > 0 && timeout.Value != defaultTimeOut) ? new HttpClient() { Timeout = new TimeSpan(timeout.Value * TimeSpan.TicksPerMillisecond) } : defaultHttpClient).ContinueWith(x => x.Result.Body);
                }
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Indexes the specified index name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="F"></typeparam>
        /// <param name="indexObject">The index object.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>System.String.</returns>
        public string Index<T, F>(IElasticWorkObject<T, F> indexObject, int? timeout = null)
        {
            try
            {
                indexObject.CheckNullObject(nameof(indexObject));

                if (!string.IsNullOrWhiteSpace(indexObject.IndexName)
                    && !string.IsNullOrWhiteSpace(indexObject.Type)
                    && indexObject.RawData != null)
                {
                    var httpRequest = GetHttpRequestUri(indexObject.IndexName, indexObject.Type).CreateHttpWebRequest(HttpConstants.HttpMethod.Post);
                    httpRequest.Timeout = timeout ?? 10000;//10sec for timeout as default.
                    httpRequest.FillData(indexObject.RawData.ToJson(false), Encoding.UTF8, HttpConstants.ContentType.Json);

                    return httpRequest.ReadResponseAsText(Encoding.UTF8).Body;
                }
            }
            catch { }

            return null;
        }
    }
}
