using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    [JsonConverter(typeof(DateConverter))]
    public struct Date : IComparable, IStringConvertable
    {
        private const string dateFormat2 = "yyyy/MM/dd";
        private const string dateFormat1 = "yyyy-MM-dd";
        private const DateTimeKind kind = DateTimeKind.Unspecified;
        private static Regex formatRegex = new Regex(@"(?<Year>(\d{4}))[\-\.\/\\](?<Month>(\d{1,2}))([\-\.\/\\](?<Day>(\d{1,2})))?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

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
            Year = date.Year;
            Month = date.Month;
            Day = date.Day;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Date"/> struct.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        public Date(int year, int month, int day) : this(new DateTime(year, month, day, 0, 0, 0, kind))
        {
        }

        /// <summary>
        /// Gets the day of week.
        /// </summary>
        /// <returns></returns>
        public DayOfWeek GetDayOfWeek()
        {
            return ToDateTime().DayOfWeek;
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
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="dateA">The date a.</param>
        /// <param name="dateB">The date b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >(Date dateA, Date dateB)
        {
            if (dateA.Year > dateB.Year)
            {
                return true;
            }
            else if (dateA.Year == dateB.Year)
            {
                if (dateA.Month > dateB.Month)
                {
                    return true;
                }
                else if (dateA.Month == dateB.Month)
                {
                    return dateA.Day > dateB.Day;
                }
            }
            return false;
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="dateTime">The date time.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >(Date date, DateTime dateTime)
        {
            if (date.Year > dateTime.Year)
            {
                return true;
            }
            else if (date.Year == dateTime.Year)
            {
                if (date.Month > dateTime.Month)
                {
                    return true;
                }
                else if (date.Month == dateTime.Month)
                {
                    return date.Day > dateTime.Day;
                }
            }
            return false;
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="dateA">The date a.</param>
        /// <param name="dateB">The date b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >=(Date dateA, Date dateB)
        {
            return !(dateA < dateB);
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="dateTime">The date time.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >=(Date date, DateTime dateTime)
        {
            return !(date < dateTime);
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="dateA">The date a.</param>
        /// <param name="dateB">The date b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <(Date dateA, Date dateB)
        {
            if (dateA.Year < dateB.Year)
            {
                return true;
            }
            else if (dateA.Year == dateB.Year)
            {
                if (dateA.Month < dateB.Month)
                {
                    return true;
                }
                else if (dateA.Month == dateB.Month)
                {
                    return dateA.Day < dateB.Day;
                }
            }

            return false;
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="dateTime">The date time.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <(Date date, DateTime dateTime)
        {
            if (date.Year < dateTime.Year)
            {
                return true;
            }
            else if (date.Year == dateTime.Year)
            {
                if (date.Month < dateTime.Month)
                {
                    return true;
                }
                else if (date.Month == dateTime.Month)
                {
                    return date.Day < dateTime.Day;
                }
            }

            return false;
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="dateA">The date a.</param>
        /// <param name="dateB">The date b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <=(Date dateA, Date dateB)
        {
            return !(dateA > dateB);
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="dateTime">The date time.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <=(Date date, DateTime dateTime)
        {
            return !(date > dateTime);
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
            return dateA == dateB ? 0 : (int)((ToDateTime(dateA, kind) - ToDateTime(dateB, kind)).TotalSeconds / (24 * 60 * 60));
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
            return addDays == 0 ? date : new Date(ToDateTime(date, kind).AddDays(addDays));
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="minusDays">The minus days.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Date operator -(Date date, int minusDays)
        {
            return date + (-1 * minusDays);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DateTime"/> to <see cref="Date"/>.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Date(DateTime dateTime)
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
        public static implicit operator Date? (string date)
        {
            return FromDateString(date);
        }

        /// <summary>
        /// Froms the date string.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static Date? FromDateString(string date)
        {
            Match match = string.IsNullOrWhiteSpace(date) ? null : formatRegex.Match(date);

            return (match != null && match.Success) ?
                new Date(match.Result("${Year}").ToInt32(), match.Result("${Month}").ToInt32(), match.Result("${Day}").ToInt32(1))
                : null as Date?;
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
            return ToDateTime(date, kind);
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
        private static DateTime ToDateTime(Date date, DateTimeKind kind = kind)
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
            return Year.GetHashCode() + Month.GetHashCode() + Day.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}-{1}-{2}", Year.ToString("0000"), Month.ToString("00"), Day.ToString("00"));
        }

        /// <summary>
        /// To the date time.
        /// </summary>
        /// <returns></returns>
        public DateTime ToDateTime(DateTimeKind kind = DateTimeKind.Unspecified)
        {
            return ToDateTime(this, kind);
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            return ToDateTime().CompareTo(((Date)obj).ToDateTime());
        }

        /// <summary>
        /// Todays this instance.
        /// </summary>
        /// <returns></returns>
        public static Date Today
        {
            get
            {
                return new Date(DateTime.Now);
            }
        }
    }
}