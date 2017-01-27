//-----------------------------------------------------------------------
// <copyright file="ExtensionMethods.cs" company="n/a">
//     Snippets taken from https://github.com/dudleycodes/CSharp-Extension-Methods
// </copyright>
//-----------------------------------------------------------------------

namespace SteamReaction
{
    /// <summary>The goal of this collection of extension methods is to make c# code throughout the project more readable.</summary>
    internal static class ExtensionMethods
    {
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
