//using System;
//using System.Collections.Generic;
//using Beyova.Api.RestApi;

//namespace Beyova.FunctionService.Generic
//{
//    /// <summary>
//    /// Interface ITransactionService
//    /// </summary>
//    /// <seealso cref="CommonServiceInterface.ITransactionService{TransactionRequest, TransactionAudit, TransactionCriteria}" />
//    public interface ITradeService : ITradeService<TransactionRequest, TransactionAudit, TransactionCriteria>
//    {
//    }

//    /// <summary>
//    /// Interface ITransactionService
//    /// </summary>
//    /// <typeparam name="TTransactionRequest">The type of the t transaction request.</typeparam>
//    /// <typeparam name="TTransactionAudit">The type of the t transaction audit.</typeparam>
//    /// <typeparam name="TTransactionCriteria">The type of the t transaction criteria.</typeparam>
//    [ApiContract("v1", "TradeService")]
//    public interface ITradeService<TTransactionRequest, TTransactionAudit, TTransactionCriteria>
//           where TTransactionRequest : ITransactionInfo
//           where TTransactionAudit : ITransactionAudit
//    {
//        /// <summary>
//        /// Creates the transaction.
//        /// </summary>
//        /// <param name="transactionRequest">The transaction request.</param>
//        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
//        [ApiOperation(GenericFunctionalServiceConstants.ResourceName.Transaction, HttpConstants.HttpMethod.Put)]
//        Guid? CreateTransaction(TTransactionRequest transactionRequest);

//        /// <summary>
//        /// Commits the transaction.
//        /// </summary>
//        /// <param name="transactionKey">The transaction key.</param>
//        /// <returns>TTransactionAudit.</returns>
//        [ApiOperation(GenericFunctionalServiceConstants.ResourceName.Transaction, HttpConstants.HttpMethod.Post, GenericFunctionalServiceConstants.ActionName.Commit)]
//        TTransactionAudit CommitTransaction(Guid? transactionKey);

//        /// <summary>
//        /// Queries the transaction audit.
//        /// </summary>
//        /// <param name="criteria">The criteria.</param>
//        /// <returns>List&lt;TTransactionAudit&gt;.</returns>
//        [ApiOperation(GenericFunctionalServiceConstants.ResourceName.Transaction, HttpConstants.HttpMethod.Post, GenericFunctionalServiceConstants.ActionName.Query)]
//        List<TTransactionAudit> QueryTransactionAudit(TTransactionCriteria criteria);

//        /// <summary>
//        /// Cancels the transaction.
//        /// </summary>
//        /// <param name="transactionKey">The transaction key.</param>
//        /// <returns>TTransactionAudit.</returns>
//        [ApiOperation(GenericFunctionalServiceConstants.ResourceName.Transaction, HttpConstants.HttpMethod.Delete)]
//        TTransactionAudit CancelTransaction(Guid? transactionKey);
//    }
//}