//-----------------------------------------------------------------------
// <copyright file="SteamAppUpdate.cs" company="n/a">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace SteamReaction.Triggers
{
    using System;
    using System.Diagnostics;
    using Newtonsoft.Json;
    using SteamKit2;
    using SteamReaction;

    /// <summary>Activates whenever </summary>
    internal class SteamAppUpdate : ITrigger
    {
        /// <summary>Gets a human-readable description of what this trigger is about.</summary>
        public string description {
            get {
                return "App ID: "+ this.SteamAppId.ToString();
            }
        }

        /// <summary>Gets or sets the minimum amount of time between requests (if any).</summary>
        public TimeSpan FloodDelay { get; set; } = new TimeSpan(0, 0, 3, 250);

        /// <summary>Gets or sets the Steam Application Id to react to.</summary>
        public int SteamAppId { get; set; } = 0;

        [JsonIgnore]
        /// <summary>Gets a human-readable summary of the result of most recent check.</summary>
        public string CurrentCheckInfo { get; private set; }

        /// <summary>Gets or sets the value from the check</summary>
        /// <remarks>Use value to determine if things have changed since the last trigger.</remarks>
        public string PreviousCheckResult { get; set; }

        /// <summary>Checks the conditions of the trigger to determine if it should be activated.</summary>
        /// <returns>True if trigger activated; otherwise false.</returns>
        public bool Check()
        {
            Debug.WriteLine(this.GetType().Name);

            using (dynamic conn = WebAPI.GetInterface("ISteamApps"))
            {
                Program.ConsoleCountdown("Querying In", 4);

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
    }
}
