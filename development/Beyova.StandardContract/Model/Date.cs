using System;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public struct Date
    {
        private const string dateFormat2 = "yyyy/MM/dd";
        private const string dateFormat1 = "yyyy-MM-dd";

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        /// <value>
        /// The month.
        /// </value>
        public int Month { get; set; }

        /// <summary>
        /// Gets or sets the day.
        /// </summary>
        /// <value>
        /// The day.
        /// </value>
        public int Day { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Date"/> struct.
        /// </summary>
        /// <param name="date">The date.</param>
        public Date(DateTime date)
        {
            this.Year = date.Year;
            this.Month = date.Month;
            this.Day = date.Day;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Date"/> struct.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        public Date(int year, int month, int day) : this(new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Unspecified))
        {
        }

        /// <summary>
        /// Froms the date time.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static Date FromDateTime(DateTime date)
        {
            return new Date(date);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="dateA">The anniversary a.</param>
        /// <param name="dateB">The anniversary b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Date dateA, Date dateB)
        {
            return dateA.Year == dateB.Year && dateA.Month == dateB.Month && dateA.Day == dateB.Day;
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="dateA">The date a.</param>
        /// <param name="dateB">The date b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static int operator -(Date dateA, Date dateB)
        {
            return dateA == dateB ? 0 : (int)((ToDateTime(dateA, DateTimeKind.Utc) - ToDateTime(dateB, DateTimeKind.Utc)).TotalSeconds / (24 * 60 * 60));
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="dateA">The date a.</param>
        /// <param name="dateB">The date b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Date dateA, Date dateB)
        {
            return dateA.Year != dateB.Year || dateA.Month != dateB.Month || dateA.Day != dateB.Day;
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="addDays">The add days.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Date operator +(Date date, int addDays)
        {
            return addDays == 0 ? date : new Date(ToDateTime(date, DateTimeKind.Unspecified).AddDays(addDays));
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DateTime"/> to <see cref="Date"/>.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator Date(DateTime dateTime)
        {
            return new Date(dateTime);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.String"/> to <see cref="System.Nullable{Date}"/>.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator Date? (string date)
        {
            var dateTime = date.ToDateTime(dateFormat1, System.Globalization.DateTimeStyles.AssumeLocal)
                ?? date.ToDateTime(dateFormat2, System.Globalization.DateTimeStyles.AssumeLocal);

            return dateTime.HasValue ? new Date(dateTime.Value) as Date? : null;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Date"/> to <see cref="DateTime"/>.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator DateTime(Date date)
        {
            return ToDateTime(date, DateTimeKind.Unspecified);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Date"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(Date date)
        {
            return date.ToString();
        }

        /// <summary>
        /// To the date time.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="kind">The kind.</param>
        /// <returns></returns>
        private static DateTime ToDateTime(Date date, DateTimeKind kind)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, kind);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj != null && this == (Date)obj;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.Year.GetHashCode() + this.Month.GetHashCode() + this.Day.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}-{1}-{2}", this.Year.ToString("0000"), this.Month.ToString("00"), this.Day.ToString("00"));
        }
    }
}