//-----------------------------------------------------------------------
// <copyright file="APIRegistry.cs" company="n/a">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace APIReactor
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
