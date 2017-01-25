//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Laclede's LAN">
//     Applicable rights reserved
// </copyright>
//-----------------------------------------------------------------------

namespace SteamReaction
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using Newtonsoft.Json;
    using SteamKit2;
    using SteamReaction.Triggers;
    using Console = Colorful.Console;

    /// <summary>Main entry point of application.</summary>
    public class Program
    {
        /// <summary>Full path to "SteamReaction" executable.</summary>
        private static readonly string AppPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        /// <summary></summary>
        private static readonly string DataFile = AppPath + Path.DirectorySeparatorChar + "SteamReaction.json";

        /// <summary>Contains the text padding of the console-display columns.</summary>
        private static readonly byte[] DisplayCols = { 48, 12, 18, 18, 22 };

        /// <summary>Full path to log file.</summary>
        private static readonly string LogFile = AppPath + Path.DirectorySeparatorChar + "SteamReaction.log";

        /// <summary>Number of seconds in the main loop.</summary>
        private static readonly int SettingMainLoopDelay = 4;

        private static List<SteamHook> triggerList;

        /// <summary>The handle to the log file.</summary>
        private static StreamWriter logHandle;

        /// <summary>Main entry point of application</summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>Windows Exit Code</returns>
        protected static int Main(string[] args)
        {
            Debug.WriteLine("Main() called.");

            Console.SetWindowSize(120, 35);

            triggerList = BuildData.Builder();

            return MonitorSteamAPI();

            return 0;
        }

        /// <summary>Monitors steam via it's web API.</summary>
        /// <returns>Windows Exit Code</returns>
        protected static int MonitorSteamAPI()
        {
            if (triggerList == null || triggerList.Count < 1)
            {
                System.Console.WriteLine("No triggers defined - nothing to do.");
                return 0;
            }

            using (logHandle = File.AppendText(LogFile))
            {
                Log(2, "Starting");

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine(DateTime.Now.ToString(), Color.DarkGray);
                    Console.WriteAscii(" Steam Reaction");
                    Console.Write("[Name]".PadRight(DisplayCols[0]));
                    Console.Write("[AppId]".PadRight(DisplayCols[1]));
                    Console.Write("[Steam Version]".PadRight(DisplayCols[2]));
                    Console.Write("[Current Version]".PadRight(DisplayCols[3]));
                    Console.WriteLine("[Last Triggered]".PadRight(DisplayCols[4]));

                    // UPDATE LOOP
                    {
                        string jsonAfter, jsonBefore = string.Empty;

                        // Don't use foreach as we want to pass by reference (and not a read-only copy)
                        for (int i = 0; i < triggerList.Count; i++)
                        {
                            Array.Sort(triggerList[i].SteamAppIds);

                            jsonBefore = JsonConvert.SerializeObject(triggerList[i]);

                            CheckHook(triggerList[i]);

                            jsonAfter = JsonConvert.SerializeObject(triggerList[i]);

                            if (jsonAfter != jsonBefore)
                            {
                                // write updated file
                                // run trigger
                            }

                            if (i + 1 < triggerList.Count)
                            {
                                ConsoleCountdown(("Querying for `" + triggerList[i + 1].Name).Truncate(60, "..") + "` in", 9, 21, true);
                            }
                        }
                    }

                    Console.WriteLine();
                    ConsoleCountdown("Re-running all checks in", SettingMainLoopDelay);
                }
            }
        }

        /// <summary>Logs a message to the log file.</summary>
        /// <param name="msg">Message to log.</param>
        protected static void Log(string msg = null)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                logHandle.Write(Environment.NewLine);
            }
            else
            {
                logHandle.WriteLine(DateTime.Now.ToString() + " -- " + msg);
            }
        }

        /// <summary>Logs the specified number of newlines followed by a message to the log file.</summary>
        /// <param name="numberOfBlankLines">Number of newlines to add to log file.</param>
        /// <param name="msg">Message to log.</param>
        protected static void Log(int numberOfBlankLines, string msg = null)
        {
            for (int i = 0; i < numberOfBlankLines; i++)
            {
                logHandle.Write(Environment.NewLine);
            }

            Log(msg);
        }

        /// <summary>Waits a specified number of seconds on the command line while displaying a personalized countdown message.</summary>
        /// <param name="msg">Message to display during the countdown (appended to the current number of seconds left).</param>
        /// <param name="numberOfSeconds">Number of seconds to wait.</param>
        /// <param name="darkenColor">If true, countdown message text color will be darkened.</param>
        protected static void ConsoleCountdown(string msg, int numberOfSeconds, bool darkenColor = false)
        {
            ConsoleCountdown(msg, numberOfSeconds, numberOfSeconds, darkenColor);
        }

        /// <summary>Waits a random number of seconds (from a specified range) on the command line while displaying a personalized countdown message.</summary>
        /// <param name="msg">Message to display during the countdown (appended to the current number of seconds left).</param>
        /// <param name="smallest">Minimum number of seconds to wait.</param>
        /// <param name="highest">Maximum number of seconds to wait.</param>
        /// <param name="darkenColor">If true, countdown message text color will be darkened.</param>
        protected static void ConsoleCountdown(string msg, int smallest, int highest, bool darkenColor = false)
        {
            string displayMsg = string.Empty;
            Random r = new Random();

            int numberOfSeconds = r.Next(smallest, highest);

            for (int t = numberOfSeconds; t > 0; t--)
            {
                displayMsg = msg + " " + t.ToString() + " seconds.";

                if (darkenColor == true)
                {
                    Console.Write(displayMsg, Color.SlateGray);
                }
                else
                {
                    Console.Write(displayMsg);
                }

                Thread.Sleep(1000);

                ConsoleClearLine();
            }

            // Fuzz the Milliseconds
            Thread.Sleep(r.Next(1, 350));
        }

        protected static void CheckHook(SteamHook hook)
        {
            if (hook.LastKnownVersions == null)
            {
                hook.LastKnownVersions = new Dictionary<int, string>();
            }

            bool executeTriggers = false;

            for (int i = 0; i < hook.SteamAppIds.Length; i++)
            {
                char paddingChar;

                if (i == 0)
                {
                    paddingChar = '·';

                    Console.Write(hook.Name.Truncate(DisplayCols[0] - 2, "..").PadRight(DisplayCols[0], paddingChar));
                }
                else
                {
                    paddingChar = ' ';
                    ConsoleCountdown(string.Empty.PadRight(DisplayCols[0]) + "Querying in", 2, 4, true);
                    Console.Write(string.Empty.PadRight(DisplayCols[0], paddingChar));
                }

                int steamAppId = hook.SteamAppIds[i];
                Console.Write(steamAppId.ToString().PadRight(DisplayCols[1], paddingChar));

                if (!hook.LastKnownVersions.ContainsKey(steamAppId))
                {
                    hook.LastKnownVersions.Add(steamAppId, "0");
                }

                string steamVersion = GetSteamVersion(steamAppId);
                string lastKnownVersion = hook.LastKnownVersions[steamAppId];

                Console.Write(steamVersion.PadRight(DisplayCols[2], paddingChar));

                if (lastKnownVersion == steamVersion)
                {
                    Console.Write(lastKnownVersion.PadRight(DisplayCols[3], paddingChar));
                }
                else
                {
                    Console.Write(lastKnownVersion.PadRight(DisplayCols[3], paddingChar), Color.Red);
                    lastKnownVersion = hook.LastKnownVersions[steamAppId] = steamVersion;
                    executeTriggers = true;
                }

                if (executeTriggers)
                {
                    Console.WriteLine("NOW".PadRight(DisplayCols[4], paddingChar), Color.Green);
                }
                else
                {
                    Console.WriteLine(hook.LastTriggered.ToString().PadRight(DisplayCols[4], paddingChar));
                }
            }

            if (executeTriggers)
            {
                hook.LastTriggered = DateTime.Now;
                foreach (ITrigger trigger in hook.Triggers)
                {
                    trigger.Execute();
                }
            }
        }

        /// <summary>Clears the current line of the console and returns the cursor to the home position.</summary>
        protected static void ConsoleClearLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        /// <summary>Using Steam's API gets the current version of a steam package</summary>
        /// <param name="steamAppId">The AppID of the steam package.</param>
        /// <returns>Current version of the steam package.</returns>
        /// <throws>Exception</throws>
        protected static string GetSteamVersion(int steamAppId)
        {
            using (dynamic conn = WebAPI.GetInterface("ISteamApps"))
            {
                KeyValue response = conn.UpToDateCheck1(appid: steamAppId, version: 0);

                if (response["success"].AsBoolean())
                {
                    return response["required_version"].AsString().ToLower().Trim();
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
        }
    }
}
