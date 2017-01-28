//-----------------------------------------------------------------------
// <copyright file="APIRegistry.cs" company="n/a">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace SteamReaction
{
    using System;
    using System.Collections.Generic;

    /// <summary>Registry to statically store information on remote APIs.</summary>
    public static class APIRegistry
    {
        private static Dictionary<string, APIRegistryEntry> registryStore = new Dictionary<string, APIRegistryEntry>();

        /// <summary>Validates an API named to be stored in the registry.</summary>
        /// <param name="apiName">Name of the API to validate.</param>
        /// <returns>Name of th sanitized API (if valid); otherwise throws exception.</returns>
        /// <throws></throws>
        private static string SanatizeAndValidateAPIName(string apiName)
        {
            if (string.IsNullOrWhiteSpace(apiName))
            {
                throw new ArgumentException("Value cannot be null, empty, or whitespace", "apiName");
            }

            return apiName.Trim().ToLower();
        }

        /// <summary></summary>
        /// <param name="apiName"></param>
        /// <returns></returns>
        /// <throws></throws>
        private static bool ValidateRegisteredAPI(string apiName)
        {
            if (!registryStore.ContainsKey(apiName))
            {
                throw new Exception("API Not Found!");
            }

            return true;
        }

        public static void Add(string apiName, TimeSpan queryTimeDelay)
        {
            apiName = APIRegistry.SanatizeAndValidateAPIName(apiName);

            if (!APIRegistry.ValidateRegisteredAPI(apiName))
            {
                if (queryTimeDelay == default(TimeSpan))
                {
                    queryTimeDelay = new TimeSpan(0, 0, 0, 1, 250);
                }

                registryStore.Add(apiName, new APIRegistryEntry() { FloodDelay = queryTimeDelay, LastQueried = default(DateTime) });
            }
        }

        public static void AddQueryRecord(string apiName)
        {
            if (APIRegistry.ValidateRegisteredAPI(apiName))
            {
                registryStore[apiName].LastQueried = DateTime.Now;
            }
        }

        public static DateTime GetNextQueryTime(string apiName)
        {
            APIRegistry.ValidateRegisteredAPI(apiName);

            return registryStore[apiName].GetNextQueryTime();
        }
    }
}
