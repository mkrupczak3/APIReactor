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
    using System.IO;
    using Newtonsoft.Json;
    using SteamReaction.WebHooks;

    /// <summary>Main entry point of application.</summary>
    public class Program
    {
        /// <summary>Developer toggle. If true disables logging.</summary>
        internal static readonly bool DevDisableLogging = false && Debugger.IsAttached;

        /// <summary>Developer toggle. If true disables execution of web-hooks.</summary>
        internal static readonly bool DevDisableWebHooks = false && Debugger.IsAttached;

        /// <summary>Full path to "SteamReaction" executable.</summary>
        private static readonly string AppPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        /// <summary>Path to file where reactors are stored on disk.</summary>
        private static readonly string DataFile = AppPath + Path.DirectorySeparatorChar + "SteamReaction.json";

        /// <summary>Full path to log file.</summary>
        private static readonly string LogFile = AppPath + Path.DirectorySeparatorChar + "SteamReaction.log";

        /// <summary>Number of seconds in the main loop.</summary>
        private static readonly int SettingMainLoopDelay = 240;

        /// <summary>Collection of reactors to be process.</summary>
        private static List<Reactor> reactors;

        /// <summary>Collection of WebHooks to trigger in the event of an uncaught Exception.</summary>
        private static List<IWebHook> crashOutWebHooks;

        /// <summary>The handle to the log file.</summary>
        private static StreamWriter logHandle;

        /// <summary>Main entry point of application</summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>Windows Exit Code</returns>
        protected static int Main(string[] args)
        {
            crashOutWebHooks = BuildData.BuildCrashOutWebHooks();

            try
            {
                Console.SetWindowSize(120, 35);

                reactors = BuildData.BuildReactor();

                using (logHandle = File.AppendText(LogFile))
                {
                    while (true)
                    {
                        Console.Clear();
                        Terminal.WriteTitle("Steam Reaction");
                        StartReactors(reactors);
                        Console.WriteLine();
                        Terminal.Countdown("Re-running all queries in", SettingMainLoopDelay, true);
                    }
                }
            }
            catch (Exception)
            {
                Reactor.ExecuteWebHooks(crashOutWebHooks);

                return 1;
            }
        }

        /// <summary>Run provided reactors checking all triggers for activations and executing related web-hooks.</summary>
        /// <param name="reactors">Reactors to start.</param>
        protected static void StartReactors(List<Reactor> reactors)
        {
            if (reactors != null && reactors.Count > 0)
            {
                for (int i = 0; i < reactors.Count; i++)
                {
                    reactors[i].Start();
                }
            }
            else
            {
                Console.WriteLine("No reactors to monitor.");
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
    }
}
