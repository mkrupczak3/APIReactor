//-----------------------------------------------------------------------
// <copyright file="APIScheduler.cs" company="https://dudley.codes">
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

namespace DudleyCodes.APIScheduler
{ 
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>Schedule calls to an API specifying flood limits and other restrictions</summary>
    /// <example>
    /// Creates an instance, specifies API limitations, and attaches callback that gets executed when the remote API can be queried
    /// <code>
    ///     APIScheduler remoteApiScheduler = new APIScheduler(10000, this.processQueue);
    /// </code>
    /// </example>
    /// <remarks>Has a lot of boiler plate for future expansion - ability to have both a daily limit as well as a rate limit</remarks>
    public class APIScheduler
    {
        private Action callback;

        /// <summary>The rate limit for the API request.</summary>
        private RateLimit rateLimit;

        private TimeZoneInfo apiTimeZone = TimeZoneInfo.Utc;

        /// <summary>Initializes a new instance of the <see cref="APIScheduler"/> class</summary>
        /// <param name="dailyLimit">The maximum number of API calls that can be made in a 24-hour time period.</param>
        /// <param name="callback">The callback to execute when an API call can be made</param>
        public APIScheduler(ushort dailyLimit, Action callback)
        {
            this.SetDailyLimit(dailyLimit);

            this.callback = callback ?? throw new ArgumentNullException("callback");
        }

        /// <summary>Sets the maximum number of queries that can be executed in a 24 hour period.</summary>
        /// <param name="dailyLimit">The maximum number of requests to make in a 24 hour period.</param>
        private void SetDailyLimit(ushort dailyLimit)
        {
            if (dailyLimit < 1)
            {
                throw new ArgumentOutOfRangeException("dailyLimit", "value must be positive");
            }

            int limitPerMinute = dailyLimit / (24 * 60);

            try
            {
                byte rateLimit = Convert.ToByte(limitPerMinute);
                this.rateLimit = new RateLimit(rateLimit, TimeSpan.FromMinutes(1));
            }
            catch (OverflowException ex)
            {
                if (limitPerMinute > byte.MaxValue)
                {
                    limitPerMinute = byte.MaxValue;
                }
                else
                {
                    throw ex;
                }
            }
        }

        private void ExecuteCallback()
        {
            this.callback();
        }
    }
}
