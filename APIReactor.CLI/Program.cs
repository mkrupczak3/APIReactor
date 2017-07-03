//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="https://dudley.codes">
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <author>James Dudley</author>
//-----------------------------------------------------------------------

namespace DudleyCodes.APIReactor
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using DudleyCodes.APIReactor.Triggers;
    using DudleyCodes.APIReactor.WebHooks;
    using Newtonsoft.Json;

    /// <summary>Main entry point of application.</summary>
    public class Program
    {
        /// <summary>Developer toggle. If true disables logging.</summary>
        internal static readonly bool DevDisableLogging = false && Debugger.IsAttached;

        /// <summary>Developer toggle. If true disables execution of web-hooks.</summary>
        internal static readonly bool DevDisableWebHooks = false && Debugger.IsAttached;

        /// <summary>Statistic containing the date and time the program started.</summary>
        internal static readonly DateTime StatRuntime = DateTime.Now;

        /// <summary>Full path to "SteamReaction" executable.</summary>
        private static readonly string AppPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        /// <summary>Path to file where reactors are stored on disk.</summary>
        private static readonly string DataFile = AppPath + Path.DirectorySeparatorChar + "SteamReaction.json";

        /// <summary>Full path to log file.</summary>
        private static readonly string LogFile = AppPath + Path.DirectorySeparatorChar + "SteamReaction.log";

        /// <summary>Number of seconds in the main loop.</summary>
        private static readonly TimeSpan SettingMainLoopDelay = new TimeSpan(0, 8, 0);

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
                        Terminal.WriteTitle();
                        StartReactors(reactors);
                        Console.WriteLine();
                        Terminal.Countdown("Re-running all reactors in", SettingMainLoopDelay);
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
