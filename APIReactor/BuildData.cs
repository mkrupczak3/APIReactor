//-----------------------------------------------------------------------
// <copyright file="BuildData.cs" company="n/a">
//     Applicable rights reserved
// </copyright>
//-----------------------------------------------------------------------
namespace APIReactor
{
    using System.Collections.Generic;
    using APIReactor.Triggers;
    using APIReactor.WebHooks;

    /// <summary>Temporary Scaffold for Data Creation</summary>
    public class BuildData
    {
        /// <summary>Generate the data</summary>
        /// <returns>data set</returns>
        public static List<Reactor> BuildReactor()
        {
            List<Reactor> returnData = new List<Reactor>();

            returnData.Add(
                new Reactor()
                {
                    Name = "Blackmesa",
                    Triggers = new List<ITrigger>()
                    {
                        new SteamAppUpdate() { SteamAppId = 221140, PreviousCheckResult = "1000" }
                    },
                    WebHooks = new List<IWebHook>()
                    {
                        new Slack()
                        {
                            URL = "https://hooks.slack.com/services/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",
                            Data = Slack.BuildData(
                                channel: "#gamesvr-logs",
                                message: ":steam: has updated *Black Mesa*. Triggering build of " + Slack.EncodeURL("https://hub.docker.com/r/llgameserverbot/blackmesa-watcher/", "llgameserverbot/blackmesa-watcher") + " on docker hub.")
                        },
                        new DockerHub()
                        {
                            URL = "https://registry.hub.docker.com/u/llgameserverbot/blackmesa-watcher/trigger/7a9a249c-4ca2-4c6a-8950-d368eacecaf4/"
                        }
                    }
                });

            returnData.Add(
                new Reactor()
                {
                    Name = "Counter-Strike: Global Offensive",
                    Triggers = new List<ITrigger>()
                    {
                        new SteamAppUpdate() { SteamAppId = 730, PreviousCheckResult = "13565"  }
                    },
                    WebHooks = new List<IWebHook>()
                    {
                        new Slack()
                        {
                            URL = "https://hooks.slack.com/services/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",
                            Data = Slack.BuildData(
                                channel: "#gamesvr-logs",
                                message: ":steam: has updated *Counter-Strike: Global Offensive*. Triggering build of " + Slack.EncodeURL("https://hub.docker.com/r/llgameserverbot/csgo-watcher/", "llgameserverbot/csgo-watcher") + " on docker hub.")
                        },
                        new DockerHub()
                        {
                            URL = "https://registry.hub.docker.com/u/llgameserverbot/csgo-watcher/trigger/6a7f4a90-e02a-41b1-8e2a-accf16daef0b/"
                        }
                    }
                });

            returnData.Add(
                new Reactor()
                    {
                        Name = "Day of Defeat: Source",
                        Triggers = new List<ITrigger>()
                        {
                            new SteamAppUpdate() { SteamAppId = 300, PreviousCheckResult = "3398447" }
                        },
                        WebHooks = new List<IWebHook>()
                        {
                            new Slack()
                            {
                                URL = "https://hooks.slack.com/services/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",
                                Data = Slack.BuildData(
                                    channel: "#gamesvr-logs",
                                    message: ":steam: has updated *Day of Defeat: Source*. Triggering build of " + Slack.EncodeURL("https://hub.docker.com/r/llgameserverbot/dods-watcher/", "llgameserverbot/dods-watcher") + " on docker hub.")
                            },
                            new DockerHub()
                            {
                                URL = "https://registry.hub.docker.com/u/llgameserverbot/dods-watcher/trigger/b4da44a4-2332-4117-ad94-0289add09da6/"
                            }
                        }
                    });

            returnData.Add(
                new Reactor()
                    {
                        Name = "Garry's Mod",
                        Triggers = new List<ITrigger>()
                        {
                            new SteamAppUpdate() { SteamAppId = 4000, PreviousCheckResult = "140419" }
                        },
                        WebHooks = new List<IWebHook>()
                        {
                            new Slack()
                            {
                                URL = "https://hooks.slack.com/services/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",
                                Data = Slack.BuildData(
                                    channel: "#gamesvr-logs",
                                    message: ":steam: has updated *Garry's Mod*. Triggering build of " + Slack.EncodeURL("https://hub.docker.com/r/llgameserverbot/garrysmod-watcher/", "llgameserverbot/garrysmod-watcher") + " on docker hub.")
                            },
                            new DockerHub()
                            {
                                URL = "https://registry.hub.docker.com/u/llgameserverbot/garrysmod-watcher/trigger/84ea21ed-51e0-44b8-a0a4-41245fc927c7/"
                            }
                        }
                    });

            returnData.Add(
                new Reactor()
                    {
                        Name = "GoldSrc",
                        Triggers = new List<ITrigger>()
                        {
                            new SteamAppUpdate() { SteamAppId = 20, PreviousCheckResult = "1122" },
                            new SteamAppUpdate() { SteamAppId = 30, PreviousCheckResult = "1126" },
                            new SteamAppUpdate() { SteamAppId = 60, PreviousCheckResult = "1121" },
                            new SteamAppUpdate() { SteamAppId = 70, PreviousCheckResult = "1122" },
                            new SteamAppUpdate() { SteamAppId = 90, PreviousCheckResult = "1121" }
                        },
                        WebHooks = new List<IWebHook>()
                        {
                            new Slack()
                            {
                                URL = "https://hooks.slack.com/services/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",
                                Data = Slack.BuildData(
                                    channel: "#gamesvr-logs",
                                    message: ":steam: has updated *GoldSrc*. Triggering build of " + Slack.EncodeURL("https://hub.docker.com/r/llgameserverbot/goldsource-watcher/", "llgameserverbot/goldsource-watcher") + " on docker hub.")
                            },
                            new DockerHub()
                            {
                                URL = "https://registry.hub.docker.com/u/llgameserverbot/goldsource-watcher/trigger/a43d3050-bbb5-4604-93a2-851e73dfb283/"
                            }
                        }
                    });

            returnData.Add(
                new Reactor()
                    {
                        Name = "Half-Life 2: Deathmatch",
                        Triggers = new List<ITrigger>()
                        {
                            new SteamAppUpdate() { SteamAppId = 320, PreviousCheckResult = "3398447" }
                        },
                        WebHooks = new List<IWebHook>()
                        {
                            new Slack()
                            {
                                URL = "https://hooks.slack.com/services/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",
                                Data = Slack.BuildData(
                                    channel: "#gamesvr-logs",
                                    message: ":steam: has updated *Half-Life 2: Deathmatch*. Triggering build of " + Slack.EncodeURL("https://hub.docker.com/r/llgameserverbot/hl2dm-watcher/", "llgameserverbot/hl2dm-watcher") + " on docker hub.")
                            },
                            new DockerHub()
                            {
                                URL = "https://registry.hub.docker.com/u/llgameserverbot/hl2dm-watcher/trigger/40195db5-df60-4846-96c3-8d31b1463ac1/"
                            }
                        }
                    });

            returnData.Add(
                new Reactor()
                    {
                        Name = "Sven Co-op",
                        Triggers = new List<ITrigger>()
                        {
                            new SteamAppUpdate() { SteamAppId = 276060, PreviousCheckResult = "5000" }
                        },
                        WebHooks = new List<IWebHook>()
                        {
                            new Slack()
                            {
                                URL = "https://hooks.slack.com/services/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",
                                Data = Slack.BuildData(
                                    channel: "#gamesvr-logs",
                                    message: ":steam: has updated *Sven Co-op*. Triggering build of " + Slack.EncodeURL("https://hub.docker.com/r/llgameserverbot/svencoop-watcher/", "llgameserverbot/svencoop-watcher") + " on docker hub.")
                            },
                            new DockerHub()
                            {
                                URL = "https://registry.hub.docker.com/u/llgameserverbot/svencoop-watcher/trigger/2555cbd6-bb6f-48bb-b5bf-9876959e1a9f/"
                            }
                        }
                    });

            returnData.Add(
                new Reactor()
                    {
                        Name = "Team Fortress 2",
                        Triggers = new List<ITrigger>()
                        {
                            new SteamAppUpdate() { SteamAppId = 440, PreviousCheckResult = "3780793" }
                        },
                        WebHooks = new List<IWebHook>()
                        {
                            new Slack()
                            {
                                URL = "https://hooks.slack.com/services/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",
                                Data = Slack.BuildData(
                                    channel: "#gamesvr-logs",
                                    message: ":steam: has updated *Team Fortress 2*. Triggering build of " + Slack.EncodeURL("https://hub.docker.com/r/llgameserverbot/tf2-watcher/", "llgameserverbot/tf2-watcher") + " on docker hub.")
                            },
                            new DockerHub()
                            {
                                URL = "https://registry.hub.docker.com/u/llgameserverbot/tf2-watcher/trigger/5e6ac482-2626-48be-90f8-767ba6aa3a60/"
                            }
                        }
                    });

            return returnData;
        }

        /// <summary>Build hooks for crash out</summary>
        /// <returns>hooks to be used in crash out</returns>
        public static List<IWebHook> BuildCrashOutWebHooks()
        {
            return new List<IWebHook>()
            {
                new Slack()
                {
                    URL = "https://hooks.slack.com/services/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",
                    Data = Slack.BuildData(
                        channel: "#gamesvr-logs",
                        message: ":fire: :fire: Hey @dudley - SteamReaction has crashed! :fire: :fire:",
                        iconEmoji: ":bug:",
                        username: "SteamReaction Bug")
                }
            };
        }
    }
}
