//-----------------------------------------------------------------------
// <copyright file="SteamReactor.cs" company="Laclede's LAN">
//     Applicable rights reserved
// </copyright>
//-----------------------------------------------------------------------

namespace SteamReaction.Reactor
{
    using System;
    using System.Collections.Generic;
    using SteamReaction.WebHooks;

    /// <summary>Structure to link steam applications and Web-Hooks triggers</summary>
    public class SteamReactor : IReactor
    {
        /// <summary>Gets or sets the name of the Docker Image that will be triggered</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Gets or sets the list of Steam Application Ids to react to.</summary>
        public int[] SteamAppIds { get; set; } = null;

        /// <summary>Gets or sets the list of WebHooks to execute when triggered.</summary>
        public List<IWebHook> WebHooks { get; set; } = null;

        /// <summary></summary>
        public Dictionary<int, string> LastKnownVersions { get; set; } = new Dictionary<int, string>();

        /// <summary>Gets or sets the date and time of last trigger execution.</summary>
        public DateTime LastTriggered { get; set; } = default(DateTime);
    }
}
