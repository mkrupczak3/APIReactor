//-----------------------------------------------------------------------
// <copyright file="TimeElapsed.cs" company="n/a">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace APIReactor.Triggers
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
