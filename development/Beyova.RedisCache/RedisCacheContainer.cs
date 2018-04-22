using System;
using System.Collections.Generic;
using Beyova.ProgrammingIntelligence;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Configuration;

namespace Beyova.Cache
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class RedisCacheContainer<TKey, TEntity> : CacheContainerBase<TKey, TEntity>, IRedisCacheContainer<TKey, TEntity>
    {
        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        /// <value>
        /// The name of the entity.
        /// </value>
        public string EntityName { get; protected set; }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        public IDatabase Database { get; protected set; }

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        public ICacheClient Client { get; protected set; }

        /// <summary>
        /// Gets or sets the key generator.
        /// </summary>
        /// <value>
        /// The key generator.
        /// </value>
        public IRedisKeyGenerator<TKey> KeyGenerator { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCacheContainer{TKey, TEntity}" /> class.
        /// </summary>
        /// <param name="containerOptions">The container options.</param>
        /// <param name="keyGenerator">The key generator.</param>
        /// <param name="retrievalOptions">The retrieval options.</param>
        public RedisCacheContainer(RedisCacheOptions containerOptions = null, IRedisKeyGenerator<TKey> keyGenerator = null, CacheAutoRetrievalOptions<TKey, TEntity> retrievalOptions = null) : base(containerOptions, retrievalOptions)
        {
            if (containerOptions == null)
            {
                containerOptions = new RedisCacheOptions();
            }

            this.EntityName = containerOptions.UseEntityFullName ? typeof(TEntity).ToCodeLook() : typeof(TEntity).Name;
            this.KeyGenerator = keyGenerator ?? new DefaultRedisKeyGenerator<TKey>(this.EntityName);
            this.Client = GetMultiplexer(containerOptions.Endpoints, (containerOptions.DatabaseIndex));
        }

        /// <summary>
        /// Internals the try get valid entity from cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        protected override bool InternalTryGetValidEntityFromCache(TKey key, out TEntity entity)
        {
            if (this.Client != null)
            {
                var slotKey = this.KeyGenerator.GetKey(key);

                if (this.Client.Exists(slotKey))
                {
                    entity = this.Client.Get<TEntity>(slotKey);
                    return true;
                }
            }

            entity = default(TEntity);
            return false;
        }

        /// <summary>
        /// Internals the update.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="ifNotExistsThenInsert">if set to <c>true</c> [if not exists then insert].</param>
        /// <param name="getExpiredStamp">The get expired stamp.</param>
        /// <returns></returns>
        protected override DateTime? InternalUpdate(TKey key, TEntity entity, bool ifNotExistsThenInsert, Func<DateTime?> getExpiredStamp)
        {
            DateTime? expiredStamp = null;
            if (this.Client != null)
            {
                try
                {
                    var slotKey = this.KeyGenerator.GetKey(key);

                    if (this.Client.Exists(slotKey))
                    {
                        expiredStamp = getExpiredStamp();
                        this.Client.Replace(slotKey, entity, expiredStamp);
                    }
                    else if (ifNotExistsThenInsert)
                    {
                        expiredStamp = getExpiredStamp();
                        this.Client.Add(slotKey, entity, expiredStamp);
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { key, entity });
                }
            }

            return expiredStamp;
        }

        /// <summary>
        /// Internals the clear.
        /// </summary>
        protected override void InternalClear()
        {
            this.Client?.FlushDb();
        }

        #region Static methods

        /// <summary>
        /// Gets the multiplexer.
        /// </summary>
        /// <param name="endpoints">The endpoints.</param>
        /// <param name="databaseIndex">Index of the database.</param>
        /// <returns></returns>
        internal static ICacheClient GetMultiplexer(List<UriEndpoint> endpoints, int databaseIndex)
        {
            try
            {
                endpoints.CheckNullOrEmptyCollection(nameof(endpoints));

                var options = new ConfigurationOptions
                {
                    AllowAdmin = true,
                    DefaultDatabase = databaseIndex
                };

                foreach (var redisHost in endpoints)
                {
                    options.EndPoints.Add(redisHost.Host, redisHost.Port ?? RedisDefaultSettings.Port);
                }

                return new StackExchangeRedisCacheClient(
                    ConnectionMultiplexer.Connect(options),
                    new StackExchangeRedisSerializer(),
                    new ServerEnumerationStrategy
                    {
                        TargetRole = ServerEnumerationStrategy.TargetRoleOptions.PreferSlave,
                        Mode = ServerEnumerationStrategy.ModeOptions.All
                    },
                    string.Empty);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { endpoints, databaseIndex });
            }
        }

        #endregion Static methods
    }
}