//-----------------------------------------------------------------------
// <copyright file="Trigger.cs" company="n/a">
//     Applicable rights reserved
// </copyright>
//-----------------------------------------------------------------------

namespace APIReactor.Triggers
{
    using System;
    using Newtonsoft.Json;

    /// <summary>Base class to reactor triggers.</summary>
    public abstract class Trigger
    {
        /// <summary>Gets or sets the name of the API this trigger uses.</summary>
        public string APIName { get; protected set; } = string.Empty;

        /// <summary>Gets or sets a human-readable summary of the result of most recent check.</summary>
        public string CurrentCheckInfo { get; protected set; } = string.Empty;

        /// <summary>Gets or sets a human-readable description of what this trigger is about.</summary>
        public string Description { get; protected set; } = string.Empty;

        /// <summary>The Date and Time the trigger's parent was activated.</summary>
        public virtual DateTime ParentLastActivated { get; set; }

        /// <summary>The value from the check</summary>
        /// <remarks>Use value to determine if things have changed since the last trigger.</remarks>
        public string PreviousCheckResult = string.Empty;

        /// <summary>Checks the conditions of the trigger to determine if it should be activated.</summary>
        /// <returns>True if trigger activated; otherwise false.</returns>
        public abstract bool Check();

        /// <summary>Validates that the trigger has properly built by the end user.</summary>
        /// <returns>True if the trigger is valid; otherwise false.</returns>
        public abstract bool Validate();
    }
}
