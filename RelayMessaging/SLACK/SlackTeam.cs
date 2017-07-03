//-----------------------------------------------------------------------
// <copyright file="SlackTeam.cs" company="https://dudley.codes">
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
    using System.Net.Http;
    using Newtonsoft.Json;

    /// <summary>Represents a SLACK team.</summary>
    public class SlackTeam
    {
        /// <summary>The URL of the incoming web-hook for the SLACK team.</summary>
        private Uri webhookUrl;

        /// <summary>Initializes a new instance of the <see cref="SlackTeam"/> class.</summary>
        /// <param name="webhookUrl">The URL for the SLACK team's incoming web-hook.</param>
        public SlackTeam(string webhookUrl)
        {
            webhookUrl = webhookUrl.Trim();

            if (webhookUrl.ToLower().StartsWith("services/"))
            {
                this.webhookUrl = new Uri("https://hooks.slack.com/" + webhookUrl);
            }
            else if (!webhookUrl.ToLower().StartsWith("https://"))
            {
                this.webhookUrl = new Uri(webhookUrl);
            }
            else
            {
                throw new Exception("Invalid webhookUrl");
            }
        }

        /// <summary>Sends a payload to an incoming SLACK web-hook.</summary>
        /// <param name="payload">The payload to send to the incoming web-hook.</param>
        internal void SendPayload(SlackPayload payload)
        {
            using (HttpClient client = new HttpClient())
            {
                var data = new FormUrlEncodedContent(new Dictionary<string, string>() { { "payload", JsonConvert.SerializeObject(payload) } });
                var response = client.PostAsync(this.webhookUrl, data).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                }
            }
        }
    }
}
