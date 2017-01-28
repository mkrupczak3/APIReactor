//-----------------------------------------------------------------------
// <copyright file="DockerHub.cs" company="n/a">
//     Applicable rights reserved
// </copyright>
//-----------------------------------------------------------------------
namespace SteamReaction.WebHooks
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
