﻿//-----------------------------------------------------------------------
// <copyright file="ITrigger.cs" company="n/a">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace SteamReaction.Triggers
{
    using System;
    using Newtonsoft.Json;

    /// <summary>Specifies a condition that will trigger Web Hooks when a condition is met.</summary>
    public interface ITrigger
    {
        /// <summary>Gets the minimum amount of time between requests (if any).</summary>
        TimeSpan FloodDelay { get; }


        /// <summary>Gets a human-readable summary of the result of most recent check.</summary>
        string CurrentCheckInfo { get; }

        /// <summary>Gets a human-readable description of what this trigger is about.</summary>
        string Description { get; }

        /// <summary>Gets the value from the check</summary>
        /// <remarks>Use value to determine if things have changed since the last trigger.</remarks>
        string PreviousCheckResult { get; }

        /// <summary>Get a value indicating whether the trigger was properly built by the end user.</summary>
        bool Validate { get; }

        /// <summary>Checks the conditions of the trigger to determine if it should be activated.</summary>
        /// <returns>True if trigger activated; otherwise false.</returns>
        bool Check();
    }
}
