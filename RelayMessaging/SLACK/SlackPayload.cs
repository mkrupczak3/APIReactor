//-----------------------------------------------------------------------
// <copyright file="SlackPayload.cs" company="https://dudley.codes">
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
    using Newtonsoft.Json;

    /// <summary>Represents a SLACK Incoming web-hook payload.</summary>
    /// <seealso cref="https://api.slack.com/incoming-webhooks"/>
    public class SlackPayload
    {
        /// <summary>Gets or sets the "text" of the SLACK payload.</summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>Gets or sets the "channel" of the SLACK payload.</summary>
        [JsonProperty("channel")]
        public string Channel { get; set; }

        /// <summary>Gets or sets the "link_names" of the SLACK payload.</summary>
        [JsonProperty("link_names")]
        public int Link_names { get; set; } = 1;

        /// <summary>Gets or sets the "username" of the SLACK payload.</summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>Gets or sets the "icon_emoji" of the SLACK payload.</summary>
        [JsonProperty("icon_emoji")]
        public string Icon_emoji { get; set; }
    }
}
