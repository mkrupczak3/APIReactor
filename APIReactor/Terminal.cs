//-----------------------------------------------------------------------
// <copyright file="Terminal.cs" company="n/a">
//     Applicable rights reserved
// </copyright>
//-----------------------------------------------------------------------

namespace APIReactor
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Threading;
    using Console = Colorful.Console;

    /// <summary>Collection of functionality related to screen output.</summary>
    internal static class Terminal
    {
        /// <summary>Gets the column positions for output on the terminal screen.</summary>
        private static int[] ColPos
        {
            get
            {
                int[] t = { 40, 18, 13, 0 };
                t[3] = Console.WindowWidth - t[0] - t[1] - t[2] - 1;
                return t;
            }
        }

        /// <summary>Clears the current line of the console and returns the cursor to the home position.</summary>
        internal static void ClearLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        /// <summary>Blocks synchronous processing and display a countdown until a given <see cref="DateTime"/> has passed.</summary>
        /// <param name="msg">Text message to display in front of the countdown.</param>
        /// <param name="deadline">When the blocking/countdown expires.</param>
        internal static void CountdownTo(string msg, DateTime deadline)
        {
            if (deadline != default(DateTime))
            {
                TimeSpan tmp = deadline - DateTime.Now;

                if (tmp > TimeSpan.Zero)
                {
                    Terminal.Countdown(msg, tmp);
                }
            }
        }

        /// <summary>Blocks synchronous processing and display a countdown until a given <see cref="TimeSpan"/> has expired.</summary>
        /// <param name="msg">Text message to display in front of the countdown.</param>
        /// <param name="period">Duration of blocking/countdown.</param>
        internal static void Countdown(string msg, TimeSpan period)
        {
            if (period <= TimeSpan.Zero)
            {
                return;
            }

            string displayMsg = string.Empty;

            // Add some milliseconds to "fuzz" queries
            Random r = new Random();
            TimeSpan fuzz = new TimeSpan(0, 0, 0, 0, r.Next(50, 400));
            period = period.Duration().Add(fuzz);

            while (period.TotalMilliseconds > 0)
            {
                if (period.TotalMilliseconds >= 1000)
                {
                    if (period.TotalMinutes > 3)
                    {
                        displayMsg = msg + " " + Math.Floor(period.TotalMinutes).ToString() + "m " + period.Seconds.ToString()  + "s";
                    }
                    else
                    {
                        displayMsg = msg + " " + Math.Floor(period.TotalSeconds).ToString() + " seconds.";
                    }

                    Console.Write(displayMsg, Color.SlateGray);
                    Thread.Sleep(1000);
                    period = period.Subtract(new TimeSpan(0, 0, 1));
                }
                else
                {
                    displayMsg = msg + " " + period.TotalMilliseconds.ToString() + " ms.";
                    Console.Write(displayMsg, Color.SlateGray);
                    Thread.Sleep(period);
                    period = period.Subtract(period);
                }

                Terminal.ClearLine();
            }
        }

        /// <summary>Writes the title (in ASCII art) to the console.</summary>
        internal static void WriteTitle()
        {
            TimeSpan uptime = DateTime.Now - Program.StatRuntime;
            if (uptime.Minutes > 1)
            {
                Console.WriteLine($"Uptime: {uptime.PrettyString()}", Color.DarkGray);
            }

            Console.WriteAscii("API Reactor");
            Console.WriteLine();
        }

        /// <summary>Writes a line of reactor output to the console.</summary>
        /// <param name="reactorName">Name of the reactor writing output.</param>
        /// <param name="triggerName">Name of trigger currently being inputted.</param>
        /// <param name="triggerActivated">True if trigger was activated; otherwise false.</param>
        /// <param name="triggerCurrentCheckInfo">Human-readable information from the trigger.</param>
        /// <param name="paddingChar">The padding character (if any) to display.</param>
        internal static void PrintLine(string reactorName, string triggerName, bool triggerActivated, string triggerCurrentCheckInfo, char paddingChar = ' ')
        {
            Console.Write(reactorName.Truncate(ColPos[0]).PadRight(ColPos[0], paddingChar));
            Console.Write(triggerName.Truncate(ColPos[1]).PadRight(ColPos[1], paddingChar));

            if (triggerActivated)
            {
                Console.Write("[ACTIVATED]".Truncate(ColPos[2]).PadRight(ColPos[2], paddingChar), Color.Red);
                Console.WriteLine(triggerCurrentCheckInfo.Truncate(ColPos[3]).PadRight(ColPos[3], paddingChar), Color.Green);
            }
            else
            {
                Console.Write(string.Empty.PadRight(ColPos[2], paddingChar));
                Console.WriteLine(triggerCurrentCheckInfo.Truncate(ColPos[3]).PadRight(ColPos[3], paddingChar));
            }
        }
    }
}
