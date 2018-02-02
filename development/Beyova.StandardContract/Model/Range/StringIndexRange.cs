namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public class StringIndexRange : Range<int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringIndexRange"/> class.
        /// </summary>
        public StringIndexRange() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringIndexRange" /> class.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="length">The length.</param>
        public StringIndexRange(int? start = null, int? length = null) : base(start, (start.HasValue && length.HasValue) ? (start.Value + length.Value) as int? : null) { }
    }
}
