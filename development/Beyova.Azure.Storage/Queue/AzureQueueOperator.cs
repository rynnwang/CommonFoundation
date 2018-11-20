using Beyova.Api;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Beyova.Azure
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Beyova.IQueueOperator{T}" />
    public class AzureQueueOperator<T> : IQueueOperator<T>
    {
        #region Fields

        /// <summary>
        /// The storage account
        /// </summary>
        internal CloudStorageAccount storageAccount;

        /// <summary>
        /// The queue client
        /// </summary>
        internal CloudQueueClient queueClient;

        /// <summary>
        /// Gets or sets the queue.
        /// </summary>
        /// <value>
        /// The queue.
        /// </value>
        public CloudQueue Queue { get; protected set; }

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureQueueOperator{T}" /> class.
        /// </summary>
        /// <param name="storageConnectionString">The storage connection string. Example: Region=China;DefaultEndpointsProtocol=https;AccountName=YOURACCOUNTNAME;AccountKey=YOURACCOUNTKEY</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="encodeMessage">if set to <c>true</c> [encode message].</param>
        public AzureQueueOperator(string storageConnectionString, string queueName, bool encodeMessage)
            : this(storageConnectionString.ConnectionStringToCredential(), queueName, encodeMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureQueueOperator{T}" /> class.
        /// </summary>
        /// <param name="serviceEndpoint">The service endpoint.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="encodeMessage">if set to <c>true</c> [encode message].</param>
        public AzureQueueOperator(RegionalServiceEndpoint serviceEndpoint, string queueName, bool encodeMessage)
            : this(AzureStorageExtension.ToCloudStorageAccount(serviceEndpoint, serviceEndpoint?.Region.ParseToEnum<AzureServiceProviderRegion>(AzureServiceProviderRegion.China) ?? AzureServiceProviderRegion.China), queueName, encodeMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureQueueOperator{T}" /> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="region">The region.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="encodeMessage">if set to <c>true</c> [encode message].</param>
        protected AzureQueueOperator(ApiEndpoint endpoint, AzureServiceProviderRegion region, string queueName, bool encodeMessage)
            : this(AzureStorageExtension.ToCloudStorageAccount(endpoint, region), queueName, encodeMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureQueueOperator{T}" /> class.
        /// Host=CustomizeHost;Region=Region;Account=AccountName,Token=AccountKey;Protocol=https/http
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="encodeMessage">if set to <c>true</c> [encode message].</param>
        public AzureQueueOperator(AzureBlobEndpoint endpoint, string queueName, bool encodeMessage)
            : this(endpoint.ToCloudStorageAccount(), queueName, encodeMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureQueueOperator{T}" /> class.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="encodeMessage">if set to <c>true</c> [encode message].</param>
        /// <param name="region">The region.</param>
        /// <param name="useHttps">The use HTTPS.</param>
        public AzureQueueOperator(StorageCredentials credential, string queueName, bool encodeMessage, AzureServiceProviderRegion region = AzureServiceProviderRegion.Global, bool useHttps = true)
            : this(credential.ToCloudStorageAccount(region, useHttps), queueName, encodeMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureQueueOperator{T}" /> class.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="encodeMessage">if set to <c>true</c> [encode message].</param>
        public AzureQueueOperator(CloudStorageAccount account, string queueName, bool encodeMessage)
        {
            storageAccount = account;
            this.queueClient = storageAccount.CreateCloudQueueClient();

            this.Queue = queueClient.GetQueueReference(queueName.SafeToLower());
            this.Queue.EncodeMessage = encodeMessage;
            this.Queue?.CreateIfNotExists();
        }

        #endregion Constructor

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return this.Queue?.ApproximateMessageCount ?? 0;
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.Queue.Clear();
        }

        /// <summary>
        /// Enqueues the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Enqueue(T item)
        {
            item.CheckNullObject(nameof(item));
            this.Queue.AddMessage(new CloudQueueMessage(item?.ToJson(false)));
        }

        /// <summary>
        /// Dequeues this instance.
        /// </summary>
        /// <returns></returns>
        public QueueItem<T> Dequeue()
        {
            var message = this.Queue.GetMessage();
            return (message != null) ? new QueueItem<T>
            {
                CreatedStamp = message.InsertionTime.ToDateTime(),
                Id = message.Id,
                Message = message.AsString.TryConvertJsonToObject<T>()
            } : default(QueueItem<T>);
        }

        /// <summary>
        /// Peeks this instance.
        /// </summary>
        /// <returns></returns>
        public QueueItem<T> Peek()
        {
            var message = this.Queue.PeekMessage();
            return (message != null) ? new QueueItem<T>
            {
                CreatedStamp = message.InsertionTime.ToDateTime(),
                Id = message.Id,
                Message = message.AsString.TryConvertJsonToObject<T>()
            } : default(QueueItem<T>);
        }
    }
}