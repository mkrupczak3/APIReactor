//-----------------------------------------------------------------------
// <copyright file="Slack.cs" company="https://dudley.codes">
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

namespace DudleyCodes.RelayMessaging
{
    using System;
    using System.Collections.Generic;

    /// <summary>Collection of generic methods for working with SLACK.</summary>
    public static class Slack
    {
        /// <summary>Sanitizes an emoji for use in SLACK.</summary>
        /// <param name="emojii">The emoji to sanitize.</param>
        /// <returns>Slack-compliant emoji.</returns>
        public static string SanitizeEmoji(string emojii)
        {
            if (string.IsNullOrWhiteSpace(emojii))
            {
                return string.Empty;
            }

            emojii = emojii.Trim();

            if (!emojii.StartsWith(":"))
            {
                emojii = $":{emojii}";
            }

            if (!emojii.EndsWith(":"))
            {
                emojii = $"{emojii}:";
            }

            return emojii;
        }

        /// <summary>Sanitizes text for use in SLACK.</summary>
        /// <param name="text">The text to sanitize.</param>
        /// <returns>The text sanitized for SLACK.</returns>
        public static string SanitizeText(string text)
        {
            var replaceStrings = new TupleList<string, string>
            {
                { "&", "&amp" },
                { "<", "&lt;" },
                { ">", "&gt;" },
                { "\t", " " },
                { "  ", " " }
            };

            foreach (Tuple<string, string> item in replaceStrings)
            {
                while (text.Contains(item.Item1))
                {
                    text.Replace(item.Item1, item.Item2);
                }
            }

            return text.Trim();
        }

        /// <summary>Represents a list of tuples.</summary>
        /// <typeparam name="T1">Type of the first item in the tuple.</typeparam>
        /// <typeparam name="T2">Type of the second item in the tuple.</typeparam>
        private class TupleList<T1, T2> : List<Tuple<T1, T2>>
        {
            /// <summary>Add a tuple to the list.</summary>
            /// <param name="item">First item of the tuple to add.</param>
            /// <param name="item2">Second item of the tuple to add.</param>
            public void Add(T1 item, T2 item2)
            {
                this.Add(new Tuple<T1, T2>(item, item2));
            }
        }
    }
}
