//-----------------------------------------------------------------------
// <copyright file="SteamAppUpdate.cs" company="n/a">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace APIReactor.Triggers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using APIReactor;
    using Newtonsoft.Json;
    using SteamKit2;

    /// <summary>Activates whenever a specified application on Steam gets updated.</summary>
    internal class SteamAppUpdate : Trigger
    {
        /// <summary>The Steam Application ID to check for updates.</summary>
        private int steamAppId = 0;

        /// <summary>Initializes a new instance of the <see cref="SteamAppUpdate"/> class.</summary>
        internal SteamAppUpdate()
        {
            this.APIName = "valve:steam";
        }

        /// <summary>Gets or sets the Steam Application ID to check for updates.</summary>
        public int SteamAppId
        {
            get
            {
                return this.steamAppId;
            }

            set
            {
                this.Description = "App ID: " + value.ToString();
                this.steamAppId = value;
            }
        }

        /// <summary>Checks the conditions of the trigger to determine if it should be activated.</summary>
        /// <returns>True if trigger activated; otherwise false.</returns>
        public override bool Check()
        {
            using (dynamic conn = WebAPI.GetInterface("ISteamApps"))
            {
                KeyValue response = conn.UpToDateCheck1(appid: this.SteamAppId, version: 0);

                if (response["success"].AsBoolean())
                {
                    string currentVersion = response["required_version"].AsString().ToLower().Trim();

                    if (currentVersion == this.PreviousCheckResult)
                    {
                        this.CurrentCheckInfo = "Version is still: " + this.PreviousCheckResult;
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(this.PreviousCheckResult))
                        {
                            this.CurrentCheckInfo = "Steam App `" + this.SteamAppId + "` updated to `" + currentVersion + "`";
                        }
                        else
                        {
                            this.CurrentCheckInfo = "Steam App `" + this.SteamAppId + "` updated: `" + this.PreviousCheckResult + "` -=> `" + currentVersion + "`";
                        }

                        this.PreviousCheckResult = currentVersion;
                        return true;
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(response["error"].AsString()))
                    {
                        throw new Exception("SteamAPI failed to return version information.");
                    }

                    throw new Exception(response["error"].AsString());
                }
            }

            return false;
        }

        /// <summary>Validates that the trigger has properly built by the end user.</summary>
        /// <returns>True if the trigger is valid; otherwise false.</returns>
        public override bool Validate()
        {
            // Validate SteamAppId
            if (this.steamAppId < 1)
            {
                return false;
            }

            return true;
        }

        /// <summary>Gets a complete list of all applications (not-DLC) from Steam.</summary>
        /// <returns>Collection of steam titles: id, name</returns>
        protected static Dictionary<int, string> GetSteamAppList()
        {
            Dictionary<int, string> t = new Dictionary<int, string>();

            using (dynamic conn = WebAPI.GetInterface("ISteamApps"))
            {
                KeyValue response = conn.GetAppList2();
                List<KeyValue> z = response["apps"].Children;

                foreach (var item in z)
                {
                    t.Add(item["appid"].AsInteger(), item["name"].AsString());
                }
            }

            return t;
        }
    }
}
