//-----------------------------------------------------------------------
// <copyright file="SteamHook.cs" company="Laclede's LAN">
//     Applicable rights reserved
// </copyright>
//-----------------------------------------------------------------------

namespace SteamReaction
{
    using System;

    /// <summary>Structure to link steam applications and docker hub triggers</summary>
    public class SteamHook
    {
        /// <summary>Gets or sets the name of the Docker Image that will be triggered</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Gets or sets the Steam Application Id</summary>
        public int SteamAppId { get; set; } = 0;

        /// <summary>URL of the trigger to execute</summary>
        public string TriggerUrl { get; set; } = string.Empty;

        /// <summary>Gets or sets the Last known version of the steam application</summary>
        public int LastKnownVersion { get; set; } = 0;

        /// <summary>Date and time of last trigger execution</summary>
        public DateTime LastTriggered { get; set; } = default(DateTime);
    }
}
