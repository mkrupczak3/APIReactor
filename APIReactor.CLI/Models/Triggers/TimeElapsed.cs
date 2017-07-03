//-----------------------------------------------------------------------
// <copyright file="TimeElapsed.cs" company="https://dudley.codes">
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
// Snippets taken from https://github.com/dudleycodes/CSharp-Extension-Methods
//-----------------------------------------------------------------------

namespace DudleyCodes.APIReactor.Triggers
{
    using System;
    using System.Collections.Generic;

    /// <summary>Activates when a minimum amount of time has passed since last triggered.</summary>
    /// <remarks>Does not necessarily active precisely when said amount of time has passed.</remarks>
    internal class TimeElapsed : Trigger
    {
        /// <summary>The amount of time to elapse before the trigger activates.</summary>
        private TimeSpan timePeriod = default(TimeSpan);

        /// <summary>Initializes a new instance of the <see cref="TimeElapsed" /> class.></summary>
        public TimeElapsed()
        {
            this.APIName = "system:clock";
        }

        /// <summary>The Date and Time the trigger's parent was activated.</summary>
        public override DateTime ParentLastActivated
        {
            get
            {
                return base.ParentLastActivated;
            }

            set
            {
                base.ParentLastActivated = value;
                this.PreviousCheckResult = value.ToString();
            }
        }

        /// <summary>Gets or sets the amount of time to elapse before the trigger activates.</summary>
        public TimeSpan TimePeriod
        {
            get
            {
                return this.timePeriod;
            }

            set
            {
                this.timePeriod = value = value.Duration();

                if (value <= TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException("Must be positive value", "timePeriod");
                }

                this.Description = $"Every " + value.PrettyString();
            }
        }

        /// <summary>Checks the conditions of the trigger to determine if it should be activated.</summary>
        /// <returns>True if trigger activated; otherwise false.</returns>
        public override bool Check()
        {
            TimeSpan timeLeft = DateTime.Now - this.ParentLastActivated.Add(this.timePeriod);

            if (this.ParentLastActivated.Add(this.timePeriod) <= DateTime.Now)
            {
                this.PreviousCheckResult = DateTime.Now.ToString();
                this.CurrentCheckInfo = "Time has elapsed";
                return true;
            }

            this.CurrentCheckInfo = timeLeft.PrettyString(skipSeconds: true) + " to go.";

            return false;
        }

        /// <summary>Validates that the trigger has properly built by the end user.</summary>
        /// <returns>True if the trigger is valid; otherwise false.</returns>
        public override bool Validate()
        {
            return false;
        }
    }
}
