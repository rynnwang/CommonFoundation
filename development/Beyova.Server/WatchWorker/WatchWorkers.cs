using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public static class WatchWorkers
    {
        /// <summary>
        /// The watcher thread
        /// </summary>
        private static Dictionary<string, Thread> watcherThreads = new Dictionary<string, Thread>();

        /// <summary>
        /// Gets the thread information.
        /// </summary>
        /// <value>
        /// The thread information.
        /// </value>
        public static Dictionary<string, int> ThreadInfo
        {
            get
            {
                return watcherThreads.Select(x => new KeyValuePair<string, int>(x.Key, x.Value?.ManagedThreadId ?? 0)).ToDictionary();
            }
        }

        /// <summary>
        /// Adds the watch work.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TContextCredential">The type of the context credential.</typeparam>
        /// <param name="workName">Name of the work.</param>
        /// <param name="watchPackage">The watch package.</param>
        public static bool AddWatchWork<TEntity, TContextCredential>(string workName, WatchPackage<TEntity, TContextCredential> watchPackage)
            where TContextCredential : class, ICredential
        {
            if (watchPackage != null && watchPackage.GetItemsToProcess != null && watchPackage.ItemProcesser != null)
            {
                if (string.IsNullOrWhiteSpace(workName))
                {
                    workName = typeof(TEntity).Name;
                }

                if (!watcherThreads.ContainsKey(workName))
                {
                    Thread thread = new Thread(new ParameterizedThreadStart(Watch<TEntity, TContextCredential>))
                    {
                        IsBackground = true
                    };
                    thread.Start(watchPackage);
                    watcherThreads.Add(workName, thread);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Watches the specified get items to process.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TContextCredential">The type of the context credential.</typeparam>
        /// <param name="p">The p.</param>
        private static void Watch<TEntity, TContextCredential>(object p)
               where TContextCredential : class, ICredential
        {
            var package = p as WatchPackage<TEntity, TContextCredential>;

            if (package != null && package.GetItemsToProcess != null && package.ItemProcesser != null)
            {
                if (!package.IdleSeconds.HasValue || package.IdleSeconds.Value < 5)
                {
                    package.IdleSeconds = 5;
                }

                while (true)
                {
                    bool needIdle = false;

                    try
                    {
                        var itemsTodo = package.GetItemsToProcess();

                        if (itemsTodo.HasItem())
                        {
                            List<Task> tasks = new List<Task>();

                            foreach (var item in itemsTodo)
                            {
                                TContextCredential credential = null;

                                if (package.GetContextCredential != null)
                                {
                                    credential = package.GetContextCredential(item);
                                }

                                tasks.AddIfNotNull(Task.Factory.StartNew(Work<TEntity, TContextCredential>,
                                new WorkerJobPackage<TEntity, TContextCredential>
                                {
                                    Target = item,
                                    Processor = package.ItemProcesser,
                                    ContextCredential = credential
                                } as object));
                            }

                            Task.WaitAll(tasks.ToArray());
                        }
                        else
                        {
                            needIdle = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Framework.ApiTracking?.LogException(ex.Handle().ToExceptionInfo());
                    }

                    if (needIdle)
                    {
                        Thread.Sleep(new TimeSpan(0, 0, package.IdleSeconds.Value));
                    }
                }
            }
        }

        /// <summary>
        /// Works the specified p.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TContextCredential">The type of the context credential.</typeparam>
        /// <param name="p">The p.</param>
        private static void Work<TEntity, TContextCredential>(object p)
             where TContextCredential : class, ICredential
        {
            var package = p as WorkerJobPackage<TEntity, TContextCredential>;
            if (package != null && package.Processor != null)
            {
                if (package.ContextCredential != null)
                {
                    ContextHelper.ApiContext.CurrentCredential = package.ContextCredential;
                }

                package.Processor(package.Target);
            }
        }
    }
}