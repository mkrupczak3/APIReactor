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
    using Console = Colorful.Console;

    /// <summary>Main entry point of application.</summary>
    public class Program
    {
        /// <summary>Full path to "SteamReaction" executable.</summary>
        private static readonly string AppPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        /// <summary></summary>
        private static readonly string DataFile = AppPath + Path.DirectorySeparatorChar + "SteamReaction.json";

        /// <summary>Full path to log file.</summary>
        private static readonly string LogFile = AppPath + Path.DirectorySeparatorChar + "SteamReaction.log";

        /// <summary>Number of seconds in the main loop.</summary>
        private static readonly int SettingMainLoopDelay = 600;

        private static List<SteamHook> triggerList;

        /// <summary>The handle to the log file.</summary>
        private static StreamWriter logHandle;

        /// <summary>Main entry point of application</summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>Exit code</returns>
        protected static int Main(string[] args)
        {
            using (logHandle = File.AppendText(LogFile))
            {
                Log(2, "Starting");

                // Open Data File
                {
                    if (!File.Exists(DataFile))
                    {
                        Console.Write("Data file '" + DataFile + "' not found!\nExiting.");
                        Log("Couldn't find data file!");
                        return 1;
                    }

                    try
                    {
                        triggerList = JsonConvert.DeserializeObject<List<SteamHook>>(File.ReadAllText(DataFile));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Datafile appears corrupt!");
                        System.Console.WriteLine("Details: " + ex.Message);

                        return 1;
                    }
                }

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine(DateTime.Now.ToString(), Color.DarkGray);
                    Console.WriteAscii(" Steam Reaction");
                    Console.Write("[Name]".PadRight(44));
                    Console.Write("[Current Version]".PadRight(20));
                    Console.Write("[Steam Version]".PadRight(20));
                    Console.WriteLine("[Last Triggered]");

                    // UPDATE LOOP
                    {
                        string jsonAfter, jsonBefore = string.Empty;

                        // Don't use foreach as we want to pass by reference (and not a read-only copy)
                        for (int i = 0; i < triggerList.Count; i++)
                        {
                            jsonBefore = JsonConvert.SerializeObject(triggerList[i]);

                            CheckHook(triggerList[i]);

                            jsonAfter = JsonConvert.SerializeObject(triggerList[i]);

                            if (jsonAfter != jsonBefore)
                            {
                                // write updated file
                                // run trigger
                            }

                            ConsoleCountdown("\tNext query in", 11, 23, true);
                        }
                    }

                    Console.WriteLine();
                    ConsoleCountdown("Restarting in", SettingMainLoopDelay);
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
            bool processTrigger = false;

            Console.Write(hook.Name.PadRight(44));

            int steamVersion = GetSteamVersion(hook);
            if (hook.LastKnownVersion == steamVersion)
            {
                Console.Write(hook.LastKnownVersion.ToString().PadRight(20), Color.Green);
            }
            else
            {
                Console.Write(hook.LastKnownVersion.ToString().PadRight(20), Color.Red);
                processTrigger = true;
                hook.LastKnownVersion = steamVersion;
            }

            Console.Write(steamVersion.ToString().PadRight(20));

            if (processTrigger)
            {
                hook.LastTriggered = DateTime.Now;
                Console.WriteLine("JUST NOW".PadRight(20), Color.Green);

                string json = JsonConvert.SerializeObject(triggerList);
                File.WriteAllText(DataFile, json);
            }
            else
            {
                Console.WriteLine(hook.LastTriggered.ToString().PadRight(20));
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
        /// <param name="hook"></param>
        /// <returns>Current version of the steam package.</returns>
        /// <throws>Exception</throws>
        protected static int GetSteamVersion(SteamHook hook)
        {
            return GetSteamVersion(hook.SteamAppId);
        }

        /// <summary>Using Steam's API gets the current version of a steam package</summary>
        /// <param name="steamAppId">The AppID of the steam package.</param>
        /// <returns>Current version of the steam package.</returns>
        /// <throws>Exception</throws>
        protected static int GetSteamVersion(int steamAppId)
        {
            using (dynamic conn = WebAPI.GetInterface("ISteamApps"))
            {
                KeyValue response = conn.UpToDateCheck1(appid: steamAppId, version: 0);

                if (response["success"].AsBoolean())
                {
                    return response["required_version"].AsInteger();
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
