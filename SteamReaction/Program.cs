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
    using SteamReaction.WebHooks;
    using Console = Colorful.Console;

    /// <summary>Main entry point of application.</summary>
    public class Program
    {
        /// <summary>Full path to "SteamReaction" executable.</summary>
        private static readonly string AppPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        private static readonly string DataFile = AppPath + Path.DirectorySeparatorChar + "SteamReaction.json";

        /// <summary>Fold where static-data will be stored.</summary>
        private static readonly string DataFolder = AppPath + Path.DirectorySeparatorChar + "DataStore";

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

            System.Console.WriteLine("START");
            Console.ReadKey();

            try
            {
                Debug.WriteLine("Main() called.");

                Directory.CreateDirectory(DataFolder);

                Console.SetWindowSize(120, 35);

                reactors = BuildData.BuildReactor();

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine(DateTime.Now.ToString(), Color.DarkGray);
                    Console.WriteAscii(" Steam Reaction");
                    MonitorReactors(reactors);
                    Console.WriteLine();
                    ConsoleCountdown("Re-running all queries in", SettingMainLoopDelay, true);
                }
            }
            catch (Exception)
            {
                Reactor.ExecuteWebHooks(crashOutWebHooks);

                return 1;
            }

            System.Console.WriteLine("END");
            Console.ReadKey();

            return 0;
        }

        protected static void MonitorReactors(List<Reactor> reactors)
        {
            if (reactors != null)
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

        /// <summary>Waits a specified number of seconds on the command line while displaying a personalized countdown message.</summary>
        /// <param name="msg">Message to display during the countdown (appended to the current number of seconds left).</param>
        /// <param name="numberOfSeconds">Number of seconds to wait.</param>
        /// <param name="darkenColor">If true, countdown message text color will be darkened.</param>
        public static void ConsoleCountdown(string msg, int numberOfSeconds, bool darkenColor = false)
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

        /// <summary>Clears the current line of the console and returns the cursor to the home position.</summary>
        protected static void ConsoleClearLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        internal static void ConsolePrintLine(string reactorName, string triggerName, bool triggerActivated, string triggerCurrentCheckInfo, char paddingChar = ' ')
        {
            byte[] colPos = { 40, 18, 13, 48 };

            string actionTaken = (triggerActivated) ? "[ACTIVATED]": string.Empty;

            Console.Write(reactorName.Truncate(colPos[0]).PadRight(colPos[0], paddingChar));
            Console.Write(triggerName.Truncate(colPos[1]).PadRight(colPos[1], paddingChar));

            if (triggerActivated)
            {
                Console.Write(actionTaken.Truncate(colPos[2]).PadRight(colPos[2], paddingChar), Color.Red);
                Console.WriteLine(triggerCurrentCheckInfo.Truncate(colPos[3]).PadRight(colPos[3], paddingChar), Color.Green);
            }
            else
            {
                Console.Write(actionTaken.Truncate(colPos[2]).PadRight(colPos[2], paddingChar));
                Console.WriteLine(triggerCurrentCheckInfo.Truncate(colPos[3]).PadRight(colPos[3], paddingChar));
            }
        }
    }
}
