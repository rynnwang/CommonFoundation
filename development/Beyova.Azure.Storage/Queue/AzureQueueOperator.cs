using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <seealso cref="Beyova.IQueueMessageOperator{T}" />
    public class AzureQueueOperator<T> : IQueueMessageOperator<T>
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
            queueClient = storageAccount.CreateCloudQueueClient();

            Queue = queueClient.GetQueueReference(queueName.SafeToLower());
            Queue.EncodeMessage = encodeMessage;
            Queue?.CreateIfNotExists();
        }

        #endregion Constructor

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return Queue?.ApproximateMessageCount ?? 0;
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            Queue.Clear();
        }

        /// <summary>
        /// Enqueues the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Enqueue(T item)
        {
            item.CheckNullObject(nameof(item));
            Queue.AddMessage(new CloudQueueMessage(item?.ToJson(false)));
        }

        /// <summary>
        /// Peeks this instance.
        /// </summary>
        /// <returns></returns>
        public QueueMessageItem<T> Peek()
        {
            return ConvertObject(Queue.PeekMessage());
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <param name="invisibilityTimeout">The invisibility timeout.</param>
        /// <returns></returns>
        public QueueMessageItem<T> GetMessage(int? invisibilityTimeout)
        {
            return ConvertObject(Queue.GetMessage(invisibilityTimeout.HasValue ? new System.TimeSpan(0, 0, invisibilityTimeout.Value) : null as TimeSpan?));
        }

        /// <summary>
        /// Gets the messages.
        /// </summary>
        /// <param name="messageCount">The message count.</param>
        /// <param name="invisibilityTimeout">The invisibility timeout.</param>
        /// <returns></returns>
        public List<QueueMessageItem<T>> GetMessages(int messageCount, int? invisibilityTimeout)
        {
            return Queue.GetMessages(messageCount, invisibilityTimeout.HasValue ? new System.TimeSpan(0, 0, invisibilityTimeout.Value) : null as TimeSpan?)
                .Select(ConvertObject)
                .ToList();
        }

        /// <summary>
        /// Deletes the message.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="receipt">The receipt.</param>
        public void DeleteMessage(string id, string receipt)
        {
            id.CheckEmptyString(nameof(id));
            Queue.DeleteMessage(id, receipt);
        }

        /// <summary>
        /// Peeks the messages.
        /// </summary>
        /// <param name="messageCount">The message count.</param>
        /// <returns></returns>
        public List<QueueMessageItem<T>> PeekMessages(int messageCount)
        {
            return Queue.PeekMessages(messageCount).Select(ConvertObject).ToList();
        }

        /// <summary>
        /// Converts the object.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        private static QueueMessageItem<T> ConvertObject(CloudQueueMessage message)
        {
            return (message != null) ? new QueueMessageItem<T>
            {
                CreatedStamp = message.InsertionTime.ToDateTime(),
                Id = message.Id,
                PopReceipt = message.PopReceipt,
                Message = message.AsString.TryConvertJsonToObject<T>()
            } : default(QueueMessageItem<T>);
        }
    }
}