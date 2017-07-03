//-----------------------------------------------------------------------
// <copyright file="SlackChannel.cs" company="https://dudley.codes">
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
    using System.Text;

    /// <summary>A relay that connects to a SLACK channel.</summary>
    public class SlackChannel : IRelay
    {
        /// <summary>The SLACK team this channel belongs to.</summary>
        private SlackTeam slackTeam;

        /// <summary>The default emoji icon to use in the SLACK channel.</summary>
        private string defaultIconEmoji;

        /// <summary>Initializes a new instance of the <see cref="SlackChannel"/> class.</summary>
        /// <param name="slackTeam">The <see cref="SlackTeam"/> that the SLACK channel belongs to.</param>
        /// <param name="channel">The slack channel.</param>
        /// <param name="username">The slack username to post messages as.</param>
        /// <param name="defaultEmoji">The default emoji to use when posting messages.</param>
        public SlackChannel(SlackTeam slackTeam, string channel, string username = null, string defaultEmoji = null)
        {
            this.slackTeam = slackTeam;
            this.Channel = channel;
            this.DefaultUsername = username;
            this.defaultIconEmoji = Slack.SanitizeEmoji(defaultEmoji);
        }

        /// <summary>Gets the name of the SLACK channel.</summary>
        public string Channel { get; private set; }

        /// <summary>Gets the default username used when transmitting to this SLACK channel.</summary>
        public string DefaultUsername { get; private set; }

        /// <summary>Sends a <see cref="RelayMessage"/> to the SLACK channel.</summary>
        /// <param name="msg">The <see cref="RelayMessage"/> to send.</param>
        public void Send(RelayMessage msg)
        {
            SlackPayload payload = new SlackPayload()
            {
                Channel = this.Channel,
                Icon_emoji = Slack.SanitizeEmoji(msg.IconEmoji ?? this.defaultIconEmoji),
                Text = string.Empty,
                Username = this.DefaultUsername
            };

            foreach (RelayMessage.MessageEntry line in msg.LineEntries)
            {
                if (line.IsUrl)
                {
                    if (string.IsNullOrWhiteSpace(line.Text))
                    {
                        payload.Text += $"<{line.Url}>";
                    }
                    else
                    {
                        payload.Text += $"<{line.Url}|{Slack.SanitizeText(line.Text)}>";
                    }
                }
                else
                {
                    payload.Text += Slack.SanitizeText(line.Text);
                }
            }

            this.slackTeam.SendPayload(payload);
        }
    }
}
