//-----------------------------------------------------------------------
// <copyright file="Slack.cs" company="n/a">
//     Applicable rights reserved
// </copyright>
//-----------------------------------------------------------------------

namespace APIReactor.WebHooks
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
