//-----------------------------------------------------------------------
// <copyright file="ExtensionMethods.cs" company="n/a">
//     Snippets taken from https://github.com/dudleycodes/CSharp-Extension-Methods
// </copyright>
//-----------------------------------------------------------------------

namespace APIReactor
{
    using System;

    /// <summary>The goal of this collection of extension methods is to make c# code throughout the project more readable.</summary>
    internal static class ExtensionMethods
    {
        /// <summary>Gets a human-friendly description of a TimeSpan.</summary>
        /// <param name="ts">TimeSpan to display contents of.</param>
        /// <param name="skipSeconds">If true won't display the number of seconds.</param>
        /// <returns> human-friendly description of the TimeSpan value.</returns>
        public static string PrettyString(this TimeSpan ts, bool skipSeconds = false)
        {
            ts = ts.Duration();

            string days = (ts.Days > 0) ? ts.Days + " days " : string.Empty;
            string hours = (ts.Hours > 0) ? ts.Hours + " hours " : string.Empty;
            string minutes = (ts.Minutes > 0) ? ts.Minutes + " minutes " : string.Empty;
            string seconds = (ts.Seconds > 0 && !skipSeconds) ? ts.Seconds + "s " : string.Empty;

            return $"{days}{hours}{minutes}{seconds}".Trim();
        }

        /// <summary>Truncate a string to a maximum length.</summary>
        /// <param name="str">The string to truncate.</param>
        /// <param name="maxLength">The maximum length of the string after truncation.</param>
        /// <param name="trailingAnnotationText">Trailing text annotation (if any) to indicate that the string has been truncated (e.g. "...").</param>
        /// <returns>The truncated string.</returns>
        public static string Truncate(this string str, int maxLength, string trailingAnnotationText = "...")
        {
            if (str.Length > maxLength)
            {
                if (string.IsNullOrEmpty(trailingAnnotationText))
                {
                    str = str.Substring(0, maxLength);
                }
                else if (str.Length <= trailingAnnotationText.Length)
                {
                    str = trailingAnnotationText.Substring(0, maxLength);
                }
                else
                {
                    str = str.Substring(0, maxLength - trailingAnnotationText.Length) + trailingAnnotationText;
                }
            }

            return str;
        }
    }
}
