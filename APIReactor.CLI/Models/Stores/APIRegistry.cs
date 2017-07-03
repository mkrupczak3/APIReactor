//-----------------------------------------------------------------------
// <copyright file="APIRegistry.cs" company="https://dudley.codes">
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

namespace DudleyCodes.APIReactor
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>Registry to statically store information on remote APIs.</summary>
    public static class APIRegistry
    {
        /// <summary></summary>
        private static Dictionary<string, APIRegistryEntry> apiInfoStore = new Dictionary<string, APIRegistryEntry>();

        /// <summary>Initializes static members of the <see cref="APIRegistry" /> class.</summary>
        static APIRegistry()
        {
            apiInfoStore.Add("system:clock", new APIRegistryEntry() { FloodDelay = TimeSpan.Zero });
            apiInfoStore.Add("valve:steam", new APIRegistryEntry() { FloodDelay = new TimeSpan(0, 0, 4) });
        }

        /// <summary></summary>
        /// <param name="apiName"></param>
        public static void AddQueryRecord(string apiName)
        {
            apiName = apiName.Trim().ToLower();

            apiInfoStore[apiName].LastQueried = DateTime.Now;
        }

        /// <summary>Gets the next DateTime the API can be safely-queried.</summary>
        /// <param name="apiName">The name of the API to query.</param>
        /// <returns>When the API can be safely-queried.</returns>
        public static DateTime GetNextQueryTime(string apiName)
        {
            apiName = apiName.Trim().ToLower();

            if (!APIRegistry.IsRegisteredAPIName(apiName))
            {
                throw new Exception("API '" + apiName + "' not defined in API Registry!");
            }

            return apiInfoStore[apiName].GetNextQueryTime();
        }

        /// <summary>Indicates if an API name is in the registry.</summary>
        /// <param name="apiName">The name of the API to check for in the registry.</param>
        /// <returns>True if it exist in the registry; otherwise false.</returns>
        private static bool IsRegisteredAPIName(string apiName)
        {
            return apiInfoStore.ContainsKey(apiName);
        }
    }
}
