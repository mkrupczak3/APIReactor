

namespace SteamReaction.Reactor
{
    using System.Collections.Generic;
    using SteamReaction.WebHooks;

    interface IReactor
    {
        /// <summary>Gets or sets the name of the reactor.</summary>
        string Name { get; set; }

        /// <summary>Gets or sets the list of WebHooks to execute when triggered.</summary>
        List<IWebHook> WebHooks { get; set; }
    }
}
