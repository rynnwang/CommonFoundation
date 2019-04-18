using System;
using System.Collections.Generic;
using Beyova.Diagnostic;

namespace Beyova.Scheduling
{
    /// <summary>
    /// Class TimeBlockSize
    /// </summary>
    public class TimeBlockSize
    {
        /// <summary>
        /// The day minutes
        /// </summary>
        private const int dayMinutes = 24 * 60;

        /// <summary>
        /// Gets or sets the block minutes.
        /// </summary>
        /// <value>
        /// The block minutes.
        /// </value>
        public int BlockMinutes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeBlockSize"/> class.
        /// </summary>
        public TimeBlockSize() : this(5)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeBlockSize" /> class.
        /// </summary>
        /// <param name="blockMinutes">The minute.</param>
        /// <exception cref="FriendlyHint"></exception>
        public TimeBlockSize(int blockMinutes = 5)
        {
            if (blockMinutes < 1 || blockMinutes > dayMinutes || dayMinutes % blockMinutes != 0)
            {
                throw ExceptionFactory.CreateInvalidObjectException(nameof(blockMinutes), blockMinutes, "OutOfRange", new FriendlyHint { Message = "Value of minute should be between [1, 1440] and integer dividable by 1440." });
            }

            BlockMinutes = blockMinutes;
        }

        /// <summary>
        /// Detects the block hit.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="sample">The sample.</param>
        /// <returns></returns>
        public List<IDayTimeBlockAssignable> DetectBlockHit(TimeRange range, IDayTimeBlockAssignable sample)
        {
            List<IDayTimeBlockAssignable> result = new List<IDayTimeBlockAssignable>();

            if (sample != null && range != null && range.From.HasValue && range.To.HasValue)
            {
                Date startDate = new Date(range.From.Value);
                Date endDate = new Date(range.To.Value);
                var maxBlockIndex = dayMinutes / BlockMinutes;

                var startIndexOfDay = GetBlockIndexOfDay(range.From.Value);
                var endIndexOfDay = GetBlockIndexOfDay(range.To.Value.AddMilliseconds(-1));

                if (startDate == endDate)
                {
                    for (var i = startIndexOfDay; i <= endIndexOfDay; i++)
                    {
                        result.Add(CreateBlockItem(sample, startDate, i));
                    }
                }
                else
                {
                    //Start day blocks
                    for (var i = startIndexOfDay; i <= maxBlockIndex; i++)
                    {
                        result.Add(CreateBlockItem(sample, startDate, i));
                    }

                    // Day block between start date and end date.
                    for (var i = 1; i < (endDate - startDate); i++)
                    {
                        var date = startDate + i;
                        for (var j = 1; j <= maxBlockIndex; j++)
                        {
                            result.Add(CreateBlockItem(sample, date, j));
                        }
                    }

                    //End day blocks
                    for (var i = 1; i <= endIndexOfDay; i++)
                    {
                        result.Add(CreateBlockItem(sample, endDate, i));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Creates the block item.
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="date">The date.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private static IDayTimeBlockAssignable CreateBlockItem(IDayTimeBlockAssignable sample, Date date, int index)
        {
            var item = sample.Clone() as IDayTimeBlockAssignable;
            item.UtcDate = date;
            item.DayTimeBlockIndex = index;
            return item;
        }

        /// <summary>
        /// Gets the block index of day.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public int GetBlockIndexOfDay(DateTime dateTime)
        {
            var x = (dateTime.Hour * 60d + dateTime.Minute) / BlockMinutes;

            // Index is started from 1
            // Regarding it is calculated based on minute, when time is hit like 2:00, 3:00, ... need to add index 1.
            return (int)Math.Ceiling((x == (int)x) ? x + 1 : x);
        }
    }
}