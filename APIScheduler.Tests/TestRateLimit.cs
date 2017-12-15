//-----------------------------------------------------------------------
// <copyright file="TestRateLimit.cs" company="https://dudley.codes">
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

namespace DudleyCodes.APIScheduler.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using DudleyCodes.APIScheduler;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>Unit tests for the <see cref="RateLimit"/> class.</summary>
    [TestClass]
    [TestCategory("APIScheduler - RateLimit")]
    public class TestRateLimit
    {
        private RateLimit limitFivePerHour = new RateLimit(5, new TimeSpan(1, 0, 0));

        [TestInitialize]
        public void InitializeTestData()
        {

        }

        /// <summary>Verify that <see cref="RateLimit"/> doesn't accept TimeSpan with a negative or zero-valued TimeSpan.</summary>
        /// <param name="ticks">Ticks to create TimeSpan from</param>
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void Initialize_NonPositive_TimeStamp(int ticks)
        {
            RateLimit t;

            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => { t = new RateLimit(10, new TimeSpan(ticks)); }
            );
        }

        /// <summary>Verify that <see cref="RateLimit.TimeUntil()"/> returns <see cref="TimeSpan.Zero"/> on DateTimes that have already occurred.</summary>
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-42)]
        [DataRow(int.MinValue)]
        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1611:ElementParametersMustBeDocumented", Justification = "Unit Test; full documentation not needed.")]
        public void TimeUntil_PastTimes(int ticks)
        {
            TimeSpan expectedTimeUntil = TimeSpan.Zero;
            DateTime currentDateTime = DateTime.Now;
            DateTime pastDateTime = currentDateTime.Add(new TimeSpan(ticks));
            TimeSpan actualTimeUntil = RateLimit.TimeUntil(pastDateTime, currentDateTime);

            Assert.AreEqual(expectedTimeUntil.Ticks, actualTimeUntil.Ticks, $"DataRow ticks: {ticks}");
        }

        /// <summary>Verify that <see cref="RateLimit.TimeUntil()"/> returns positive valued TimeSpan when DateTimes are in the future.</summary>
        [DataTestMethod]
        [DataRow(1)]
        [DataRow(42)]
        [DataRow(int.MaxValue)]
        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1611:ElementParametersMustBeDocumented", Justification = "Unit Test; full documentation not needed.")]
        public void TimeUntil_FutureTimes(int ticks)
        {
            TimeSpan expectedTimeUntil = new TimeSpan(ticks);
            DateTime currentDateTime = DateTime.Now;

            DateTime futureDateTime = currentDateTime.Add(expectedTimeUntil);
            TimeSpan actualTimeUntil = RateLimit.TimeUntil(futureDateTime, currentDateTime);

            Assert.AreEqual(expectedTimeUntil.Ticks, actualTimeUntil.Ticks, $"DataRow ticks: {ticks}");
        }

        [TestMethod]
        public void WhenCanNextRequestBeRun_Empty()
        {
            Assert.IsTrue(this.limitFivePerHour.WhenCanNextRequestBeRun(new List<DateTime>()) <= DateTime.Now);
        }

        /// <summary>Verify a passed query history is not modified.</summary>
        [TestMethod]
        public void WhenCanNextRequestBeRun_HistoryIsImmutable()
        {
            List <DateTime> tmp = new List<DateTime>()
            {
                DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MaxValue
            };

            DateTime t = this.limitFivePerHour.WhenCanNextRequestBeRun(tmp);

            Assert.AreEqual(7, tmp.Count);
        }

        /// <summary>Verify <see cref="RateLimit.WhenCanNextRequestBeRun(List{DateTime})"/> </summary>
        [TestMethod]
        public void WhenCanNextRequestBeRun_Null()
        {
            Assert.IsTrue(this.limitFivePerHour.WhenCanNextRequestBeRun(null) <= DateTime.Now);
        }
    }
}
