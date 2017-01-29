//-----------------------------------------------------------------------
// <copyright file="APIRegistryEntry.cs" company="n/a">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace APIReactor
{
    using System;

    /// <summary>Represents a query-able API.</summary>
    public class APIRegistryEntry
    {
        /// <summary>Gets or sets the minimum amount of time between requests.</summary>
        public TimeSpan FloodDelay { get; set; }

        /// <summary>Gets or sets the last time the API was last queried.</summary>
        public DateTime LastQueried { get; set; } = default(DateTime);

        /// <summary>Gets the next DateTime that a query can be preformed.</summary>
        /// <returns>DateTime containing a value that indicates when a query can be preformed.</returns>
        public DateTime GetNextQueryTime()
        {
            return (this.LastQueried == default(DateTime)) ? DateTime.Now : this.LastQueried.Add(this.FloodDelay);
        }
    }
}
