//-----------------------------------------------------------------------
// <copyright file="DockerHub.cs" company="https://dudley.codes">
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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Http;

    /// <summary>Trigger for Docker Hub.</summary>
    public class DockerHub : IWebHook
    {
        /// <summary>Gets the HTTP request header Content-Type.</summary>
        public string ContentType { get; } = "Content-Type: application/json";

        /// <summary>Gets or sets the data to post. Defaults value tells Docker Hub to rebuild all tags for the image.</summary>
        public string Data { get; set; } = "{\"build\": true}";

        /// <summary>Gets the HTTP RequestMethod of the trigger.</summary>
        public string RequestMethod { get; } = "POST";

        /// <summary>Gets or sets the URL of the trigger.</summary>
        public string URL { get; set; } = string.Empty;

        /// <summary>Executes the trigger.</summary>
        public void Execute()
        {
            using (var client = new HttpClient())
            {
                var data = new FormUrlEncodedContent(new Dictionary<string, string>() { { "data", this.Data } });
                var response = client.PostAsync(this.URL, data).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                }
            }
        }
    }
}
