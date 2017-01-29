//-----------------------------------------------------------------------
// <copyright file="TimeElapsed.cs" company="n/a">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace APIReactor.Triggers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>Activates when a minimum amount of time has passed since last triggered.</summary>
    /// <remarks>Does not necessarily active precisely when said amount of time has passed.</remarks>
    public class TimeElapsed : ITrigger
    {
        /// <summary>The name of the API this trigger uses.</summary>
        public string APIName { get; } = "system:clock";

        /// <summary>Gets a human-readable summary of the result of most recent check.</summary>
        public string CurrentCheckInfo { get; }

        /// <summary>Gets a human-readable description of what this trigger is about.</summary>
        public string Description { get; }

        /// <summary>Gets the value from the check</summary>
        /// <remarks>Use value to determine if things have changed since the last trigger.</remarks>
        public string PreviousCheckResult { get; }

        /// <summary>Validates that the trigger has properly built by the end user.</summary>
        /// <returns>True if the trigger is valid; otherwise false.</returns>
        public bool Validate()
        {
            return false;
        }

        /// <summary>Checks the conditions of the trigger to determine if it should be activated.</summary>
        /// <returns>True if trigger activated; otherwise false.</returns>
        public bool Check()
        {
            return false;
        }
    }
}
