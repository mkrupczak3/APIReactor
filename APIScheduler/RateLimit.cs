//-----------------------------------------------------------------------
// <copyright file="RateLimit.cs" company="https://dudley.codes">
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <author>James Dudley</author>
//-----------------------------------------------------------------------

#if DEBUG  
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("APIScheduler.Tests")]
#endif

namespace DudleyCodes.APIScheduler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>Defines a rate-limit relationship between <see cref="QuantityLimit"/> during a <see cref="TimePeriod"/>.</summary>
    internal class RateLimit
    {
        /// <summary>Initializes a new instance of the <see cref="RateLimit"/> class.</summary>
        /// <param name="quantityLimit">The quantity during the <see cref="TimePeriod"/>.</param>
        /// <param name="timePeriod">The time period that <see cref="QuantityLimit"/> applies to.</param>
        public RateLimit(byte quantityLimit, TimeSpan timePeriod)
        {
            if (timePeriod <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("timePeriod", "TimeSpan must have a positive value.");
            }

            this.QuantityLimit = quantityLimit;
            this.TimePeriod = timePeriod;
        }

        /// <summary>Gets the amount of queries allowed in a <see cref="TimePeriod"/>.</summary>
        public byte QuantityLimit { get; private set; }

        /// <summary>Gets the amount of time in which <see cref="QuantityLimit"/> are allowed.</summary>
        public TimeSpan TimePeriod { get; private set; }

        /// <summary>Determines how far in the future a DateTime is</summary>
        /// <param name="futureDateTime">The (future) DateTime</param>
        /// <param name="currentTime">Optional current DateTime for comparing at time of function invocation rather than execution</param>
        /// <returns>The amount of time <paramref name="futureDateTime"/> is in the future; otherwise if <paramref name="futureDateTime"/> has already occurred <see cref="TimeSpan.Zero"/></returns>
        public static TimeSpan TimeUntil(DateTime futureDateTime, DateTime? currentTime = null)
        {
            DateTime currentDateTime = currentTime ?? DateTime.Now;

            if (futureDateTime.CompareTo(currentTime) <= 0)
            {
                return TimeSpan.Zero;
            }

            return futureDateTime.Subtract(currentDateTime);
        }

        /// <summary>Determines the next <see cref="DateTime"/> a query can be executed.</summary>
        /// <param name="history">query history</param>
        /// <returns>When the next query can be executed.</returns>
        public DateTime WhenCanNextRequestBeRun(List<DateTime> history)
        {
            DateTime invokedDateTime = DateTime.Now;

            if (history != null && history.Count > this.QuantityLimit)
            {
                List<DateTime> queryHistory = new List<DateTime>(history);

                // Disregard entries outside of current time period
                DateTime timePeriodStart = invokedDateTime.Subtract(this.TimePeriod);

                queryHistory.RemoveAll(x => x < timePeriodStart);

                if (queryHistory.Count >= this.QuantityLimit)
                {
                    return invokedDateTime.Add(RateLimit.TimeUntil(queryHistory.Min(x => x)));
                }
            }

            return invokedDateTime;
        }
    }
}
