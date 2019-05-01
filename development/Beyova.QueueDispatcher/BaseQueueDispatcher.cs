using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Beyova.Diagnostic;

namespace Beyova.QueueDispatch
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseQueueDispatcher<T> : IQueueDispatcher
    {
        /// <summary>
        /// Gets or sets the queue message operator.
        /// </summary>
        /// <value>
        /// The queue message operator.
        /// </value>
        public IQueueMessageOperator<T> QueueMessageOperator { get; private set; }

        /// <summary>
        /// Gets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public QueueDispatchOptions Options { get; private set; }

        /// <summary>
        /// The watch thread
        /// </summary>
        protected Thread watchThread = null;

        /// <summary>
        /// The thread locker
        /// </summary>
        protected object threadLocker = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseQueueDispatcher{T}" /> class.
        /// </summary>
        /// <param name="queueMessageOperator">The queue message operator.</param>
        /// <param name="options">The options.</param>
        protected BaseQueueDispatcher(IQueueMessageOperator<T> queueMessageOperator, QueueDispatchOptions options)
        {
            queueMessageOperator.CheckNullObject(nameof(queueMessageOperator));
            QueueMessageOperator = queueMessageOperator;
            Options = options ?? new QueueDispatchOptions
            {
                IdleInterval = 5
            };

            Initialize();
            DispatcherHub.Register(this);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            if (watchThread == null)
            {
                lock (threadLocker)
                {
                    if (watchThread == null)
                    {
                        watchThread = new Thread(new ThreadStart(Watch))
                        {
                            IsBackground = true
                        };

                        watchThread.Start();
                    }
                }
            }
        }

        /// <summary>
        /// Watches this instance.
        /// </summary>
        protected virtual void Watch()
        {
            var idleIntervalInMillisecond = Options.IdleInterval * 1000;
            if (idleIntervalInMillisecond < 1000)
            {
                idleIntervalInMillisecond = 5000;
            }
            var batchSize = (!Options.BatchSize.HasValue || Options.BatchSize.Value < 1 || Options.BatchSize.Value > 20) ? 1 : Options.BatchSize.Value;
            bool needIdle = false;

            while (true)
            {
                try
                {
                    var items = QueueMessageOperator.GetMessages(batchSize, Options.InvisibilityTimeout);

                    if (items.HasItem())
                    {
                        needIdle = false;

                        if (items.Count == 1)
                        {
                            Task.WaitAll(Task.Factory.StartNew(Dispatch, items.FirstOrDefault()));
                        }
                        else
                        {
                            Task.WaitAll(items.Select(x => Task.Factory.StartNew(Dispatch, x)).ToArray());
                        }
                    }
                    else
                    {
                        needIdle = true;
                    }

                    needIdle = !items.HasItem();
                }
                catch (Exception ex)
                {
                    needIdle = true;
                    WriteException(ex.Handle());
                }

                if (needIdle)
                {
                    Thread.Sleep(idleIntervalInMillisecond);
                }
            }
        }

        /// <summary>
        /// Dispatches the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        protected void Dispatch(object obj)
        {
            QueueMessageItem<T> message = obj as QueueMessageItem<T>;

            try
            {
                message.CheckNullObject(nameof(message));
                var result = Process(message);
                result.CheckNullObject(nameof(result));

                if (result.IsSucceed)
                {
                    QueueMessageOperator.DeleteMessage(message.Id, result.Receipt);
                }
                else if (result.Exception != null)
                {
                    throw result.Exception;
                }
            }
            catch (Exception ex)
            {
                WriteException(ex.Handle(new { message }));
            }
        }

        /// <summary>
        /// Processes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        protected abstract QueueDispatcherResult Process(QueueMessageItem<T> message);

        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        protected abstract string WriteLog(QueueMessageItem<T> message);

        /// <summary>
        /// Writes the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        protected abstract void WriteException(BaseException exception);
    }
}