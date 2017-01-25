//-----------------------------------------------------------------------
// <copyright file="Slack.cs" company="Laclede's LAN">
//     Applicable rights reserved
// </copyright>
//-----------------------------------------------------------------------

namespace SteamReaction.WebHooks
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Http;
    using Newtonsoft.Json;

    /// <summary>Trigger for SLACK (incoming webhooks)</summary>
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

        static public string BuildData(string channel, string message, string username = "SteamReaction", string iconEmoji = ":robot_face:")
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

        static public string EncodeURL(string url, string text = "")
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                text = url;
            }

            return "<" + url + "|" + text + ">";
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
