//-----------------------------------------------------------------------
// <copyright file="Terminal.cs" company="n/a">
//     Applicable rights reserved
// </copyright>
//-----------------------------------------------------------------------

namespace SteamReaction
{
    using System;
    using System.Drawing;
    using System.Threading;
    using Console = Colorful.Console;

    /// <summary>Collection of functionality related to screen output.</summary>
    internal static class Terminal
    {
        /// <summary>Clears the current line of the console and returns the cursor to the home position.</summary>
        internal static void ClearLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        /// <summary>Waits a specified number of seconds on the command line while displaying a personalized countdown message.</summary>
        /// <param name="msg">Message to display during the countdown (appended to the current number of seconds left).</param>
        /// <param name="numberOfSeconds">Number of seconds to wait.</param>
        /// <param name="darkenColor">If true, countdown message text color will be darkened.</param>
        internal static void Countdown(string msg, int numberOfSeconds, bool darkenColor = false)
        {
            Terminal.Countdown(msg, numberOfSeconds, numberOfSeconds, darkenColor);
        }

        /// <summary>Waits a random number of seconds (from a specified range) on the command line while displaying a personalized countdown message.</summary>
        /// <param name="msg">Message to display during the countdown (appended to the current number of seconds left).</param>
        /// <param name="smallest">Minimum number of seconds to wait.</param>
        /// <param name="highest">Maximum number of seconds to wait.</param>
        /// <param name="darkenColor">If true, countdown message text color will be darkened.</param>
        internal static void Countdown(string msg, int smallest, int highest, bool darkenColor = false)
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

                Terminal.ClearLine();
            }

            // Fuzz the Milliseconds
            Thread.Sleep(r.Next(1, 350));
        }

        /// <summary>Writes a title (in ASCII art) to the console.</summary>
        /// <param name="title">Title text to display.</param>
        internal static void WriteTitle(string title)
        {
            Console.WriteLine(DateTime.Now.ToString(), Color.DarkGray);
            Console.WriteAscii(title);
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
            byte[] colPos = { 40, 18, 13, 48 };

            Console.Write(reactorName.Truncate(colPos[0]).PadRight(colPos[0], paddingChar));
            Console.Write(triggerName.Truncate(colPos[1]).PadRight(colPos[1], paddingChar));

            if (triggerActivated)
            {
                Console.Write("[ACTIVATED]".Truncate(colPos[2]).PadRight(colPos[2], paddingChar), Color.Red);
                Console.WriteLine(triggerCurrentCheckInfo.Truncate(colPos[3]).PadRight(colPos[3], paddingChar), Color.Green);
            }
            else
            {
                Console.Write(string.Empty.PadRight(colPos[2], paddingChar));
                Console.WriteLine(triggerCurrentCheckInfo.Truncate(colPos[3]).PadRight(colPos[3], paddingChar));
            }
        }
    }
}
