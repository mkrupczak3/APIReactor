﻿//-----------------------------------------------------------------------
// <copyright file="SteamAppUpdate.cs" company="n/a">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace SteamReaction.Triggers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Newtonsoft.Json;
    using SteamKit2;
    using SteamReaction;

    /// <summary>Activates whenever </summary>
    internal class SteamAppUpdate : ITrigger
    {
        /// <summary>Gets a human-readable description of what this trigger is about.</summary>
        public string Description
        {
            get
            {
                return "App ID: " + this.SteamAppId.ToString();
            }
        }

        /// <summary>Gets or sets the minimum amount of time between requests (if any).</summary>
        public TimeSpan FloodDelay { get; set; } = new TimeSpan(0, 0, 3, 250);

        /// <summary>Gets or sets the Steam Application Id to react to.</summary>
        public int SteamAppId { get; set; } = 0;

        /// <summary>Gets a human-readable summary of the result of most recent check.</summary>
        [JsonIgnore]
        public string CurrentCheckInfo { get; private set; }

        /// <summary>Gets or sets the value from the check</summary>
        /// <remarks>Use value to determine if things have changed since the last trigger.</remarks>
        public string PreviousCheckResult { get; set; }

        /// <summary>Get a value indicating whether the trigger was properly built by the end user.</summary>
        public bool Validate { get; }

        /// <summary>Checks the conditions of the trigger to determine if it should be activated.</summary>
        /// <returns>True if trigger activated; otherwise false.</returns>
        public bool Check()
        {
            Debug.WriteLine(this.GetType().Name);

            using (dynamic conn = WebAPI.GetInterface("ISteamApps"))
            {
                Terminal.Countdown("Querying In", 4, true);

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