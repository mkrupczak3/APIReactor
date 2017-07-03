//-----------------------------------------------------------------------
// <copyright file="RelayMessage.cs" company="https://dudley.codes">
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
    using System.Diagnostics;

    /// <summary>Represents a message that can be sent through <see cref="DudleyCodes.RelayMessaging"/>. Is composed by a collection of <see cref="RelayMessage.MessageEntry"/>.</summary>
    public class RelayMessage
    {
        /// <summary>Gets or sets the Collection of line entries that make up this RelayMessage.</summary>
        public List<MessageEntry> LineEntries { get; set; } = new List<MessageEntry>();

        /// <summary>Gets the emoji icon associated with the message.</summary>
        public string IconEmoji { get; private set; } = null;

        /// <summary>Sends the RelayMessage to the provided destination relays.</summary>
        /// <param name="relays">Array of destinations relays to send the RelayMessage to.</param>
        public void Send(params IRelay[] relays)
        {
            if (relays == null)
            {
                throw new Exception("At least one relay must be provided.");
            }

            foreach (IRelay relay in relays)
            {
                if (relay == null)
                {
                    throw new NullReferenceException("Cannot send to a null relay.");
                }

                relay.Send(this);
            }
        }

        /// <summary>Adds a line-break to the RelayMessage.</summary>
        /// <returns>The RelayMessage being built.</returns>
        public RelayMessage AddLineBreak()
        {
            this.AddText("\n");

            return this;
        }

        /// <summary>Adds text to the RelayMessage.</summary>
        /// <param name="text">Text to add to the RelayMessage.</param>
        /// <returns>The RelayMessage being built.</returns>
        public RelayMessage AddText(string text)
        {
            this.LineEntries.Add(new MessageEntry() { Text = text });

            return this;
        }

        /// <summary>Adds a line of text to the RelayMessage.</summary>
        /// <param name="lines">Line(s) of text to add to the RelayMessage.</param>
        /// <returns>The RelayMessage being built.</returns>
        public RelayMessage AddLine(params string[] lines)
        {
            foreach (var line in lines)
            {
                if (line != null)
                {
                    this.LineEntries.Add(new MessageEntry() { Text = line.TrimEnd() + "\n" });
                }
            }

            return this;
        }

        /// <summary>Adds a URL to the RelayMessage.</summary>
        /// <param name="url">URL to add to the RelayMessage.</param>
        /// <param name="text">The text (if any) to associate with the URL.</param>
        /// <returns>The RelayMessage being built.</returns>
        public RelayMessage AddUrl(string url, string text = null)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                text = url;
            }

            this.LineEntries.Add(new MessageEntry() { Text = text, Url = url.Trim() });

            return this;
        }

        /// <summary>Sets the emoji icon to be used with the RelayMessage.</summary>
        /// <param name="iconEmoji">The text of the emoji icon to associate with the RelayMessage.</param>
        /// <returns>The RelayMessage being built.</returns>
        /// /// <remarks>May not be supported by all relays.</remarks>
        public RelayMessage SetIconEmoji(string iconEmoji)
        {
            if (this.IconEmoji != null)
            {
                Debug.WriteLine($"Notice: previously set icon emoji \"{this.IconEmoji}\" is being replaced with \"{iconEmoji}\".");
            }

            this.IconEmoji = iconEmoji;

            return this;
        }

        /// <summary>Represents a single line entry that makes up a RelayMessage.</summary>
        public class MessageEntry
        {
            /// <summary>Gets or sets the text of the message entry.</summary>
            public string Text { get; set; }

            /// <summary>Gets or sets the URL (if any) of the message entry.</summary>
            public string Url { get; set; }

            /// <summary>Gets a value indicating whether the message entry is a URL.</summary>
            public bool IsUrl
            {
                get
                {
                    return !string.IsNullOrWhiteSpace(this.Url);
                }
            }
        }
    }
}
