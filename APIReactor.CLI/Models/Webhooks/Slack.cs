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
// Snippets taken from https://github.com/dudleycodes/CSharp-Extension-Methods
//-----------------------------------------------------------------------

namespace DudleyCodes.APIReactor.WebHooks
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Http;
    using Newtonsoft.Json;

    /// <summary>Trigger for SLACK (incoming web-hooks)</summary>
    public class Slack : IWebHook
    {
        /// <summary>Gets the HTTP request header Content-Type.</summary>
        public string ContentType { get; }

        /// <summary>Gets or sets the data to post.</summary>
        public string Data { get; set; }

        /// <summary>Gets the HTTP RequestMethod of the trigger.</summary>
        public string RequestMethod { get; }

        /// <summary>Gets or sets the URL of the trigger.</summary>
        public string URL { get; set; }

        /// <summary></summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <param name="username"></param>
        /// <param name="iconEmoji"></param>
        /// <returns></returns>
        static public string BuildData(string channel, string message, string username = "SteamReaction", string iconEmoji = ":zbot:")
        {
            Dictionary<string, string> test = new Dictionary<string, string>()
            {
                { "channel", channel },
                { "icon_emoji", iconEmoji },
                { "text", message },
                { "username", username }
            };

            return JsonConvert.SerializeObject(test);
        }

        /// <summary>Encodes a URL into a SLACK-compatible format.</summary>
        /// <param name="url">URL to encode.</param>
        /// <param name="text">(optional) Text to describe the URL.</param>
        /// <returns>The encoded URL.</returns>
        static public string EncodeURL(string url, string text = "")
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                text = url;
            }

            return "<" + url + "|" + text + ">";
        }

        /// <summary></summary>
        /// <param name="ex"></param>
        /// <param name="url"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        static public Slack FromException(Exception ex, string url, string channel = "")
        {
            return new WebHooks.Slack()
            {
                URL = url,
                Data = Slack.BuildData(
                    channel: channel,
                    iconEmoji: ":zbot-fire:",
                    username: "SteamReaction Bug",
                    message: ":fire: :fire: Hey @dudley - SteamReaction has crashed! :fire: :fire: \n"
                        + ":fire: Message: " + ex.Message + "\n"
                        + ":fire: Source" + ex.Source + "\n"
                        + ":fire: Stack Trace" + ex.StackTrace
                )
            };
        }

        /// <summary>Executes the trigger.</summary>
        public void Execute()
        {
            using (var client = new HttpClient())
            {
                var data = new FormUrlEncodedContent(new Dictionary<string, string>() { { "payload", this.Data } });
                var response = client.PostAsync(this.URL, data).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                }
            }
        }
    }
}
