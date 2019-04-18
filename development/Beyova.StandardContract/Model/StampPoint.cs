using System;

namespace Beyova
{
    /// <summary>
    /// Temp or view model for helping sorting stamp based objects
    /// </summary>
    public class StampPoint<T> : StampPoint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StampPoint{T}"/> class.
        /// </summary>
        /// <param name="stamp">The stamp.</param>
        /// <param name="obj">The object.</param>
        public StampPoint(DateTime stamp, T obj)
        {
            if (obj != null)
            {
                Stamp = stamp;
                ObjectType = typeof(T);
                Object = obj;
            }
        }
    }

    /// <summary>
    /// Temp or view model for helping sorting stamp based objects
    /// </summary>
    public class StampPoint : IComparable
    {
        /// <summary>
        /// Gets or sets the stamp.
        /// </summary>
        /// <value>
        /// The stamp.
        /// </value>
        public DateTime Stamp { get; set; }

        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        /// <value>
        /// The object.
        /// </value>
        public Object Object { get; set; }

        /// <summary>
        /// Gets or sets the type of the object.
        /// </summary>
        /// <value>
        /// The type of the object.
        /// </value>
        public Type ObjectType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StampPoint"/> class.
        /// </summary>
        /// <param name="stamp">The stamp.</param>
        /// <param name="obj">The object.</param>
        public StampPoint(DateTime stamp, Object obj)
        {
            if (obj != null)
            {
                Stamp = stamp;
                Object = obj;
                ObjectType = obj.GetType();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StampPoint"/> class.
        /// </summary>
        protected StampPoint() { }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj" /> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj" />. Greater than zero This instance follows <paramref name="obj" /> in the sort order.
        /// </returns>
        public int CompareTo(object obj)
        {
            return obj == null ? 1 : Stamp.CompareTo(((StampPoint)obj).Stamp);
        }
    }
}
