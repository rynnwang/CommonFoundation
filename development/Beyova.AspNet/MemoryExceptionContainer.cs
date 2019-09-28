using Beyova.Cache;
using Beyova.Diagnostic;
using System;

namespace Beyova.Web
{
    /// <summary>
    ///
    /// </summary>
    public class MemoryExceptionContainer : MemoryCacheContainer<Guid, ExceptionInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryExceptionContainer"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public MemoryExceptionContainer(int? capacity) : base(new MemoryCacheContainerOptions<Guid>
        {
            Capacity = (capacity.HasValue && capacity.Value > 0) ? capacity.Value : 500
        })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryExceptionContainer"/> class.
        /// </summary>
        public MemoryExceptionContainer() : this(null)
        {
        }

        /// <summary>
        /// Saves the exception information.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public Guid? SaveExceptionInfo(ExceptionInfo exception)
        {
            if (exception != null)
            {
                if (!exception.Key.HasValue)
                {
                    exception.Key = Guid.NewGuid();
                }

                Update(exception.Key.Value, exception);
                return exception.Key;
            }

            return null;
        }
    }
}