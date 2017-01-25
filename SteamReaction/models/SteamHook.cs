//-----------------------------------------------------------------------
// <copyright file="SteamHook.cs" company="Laclede's LAN">
//     Applicable rights reserved
// </copyright>
//-----------------------------------------------------------------------

namespace SteamReaction
{
    using System;
    using System.Collections.Generic;
    using SteamReaction.Triggers;

    /// <summary>Structure to link steam applications and Docker Hub triggers</summary>
    public class SteamHook
    {
        /// <summary>Gets or sets the name of the Docker Image that will be triggered</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Gets or sets the Steam Application Id</summary>
        public int[] SteamAppIds { get; set; } = null;

        /// <summary>Gets or sets the trigger.</summary>
        public List<ITrigger> Triggers { get; set; } = null;

        /// <summary></summary>
        public Dictionary<int, string> LastKnownVersions { get; set; } = new Dictionary<int, string>();

        /// <summary>Gets or sets the date and time of last trigger execution.</summary>
        public DateTime LastTriggered { get; set; } = default(DateTime);
    }
}
