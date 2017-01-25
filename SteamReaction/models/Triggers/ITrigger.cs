//-----------------------------------------------------------------------
// <copyright file="ITrigger.cs" company="Laclede's LAN">
//     Applicable rights reserved
// </copyright>
//-----------------------------------------------------------------------

namespace SteamReaction.Triggers
{
    /// <summary>Interface representing a trigger.</summary>
    public interface ITrigger
    {
        /// <summary>Gets the HTTP request header Content-Type.</summary>
        string ContentType { get; }

        /// <summary>Gets or sets the data to post.</summary>
        string Data { get; set; }

        /// <summary>Gets the HTTP RequestMethod of the trigger.</summary>
        string RequestMethod { get; }

        /// <summary>Gets or sets the URL of the trigger.</summary>
        string URL { get; set; }

        /// <summary>Executes the trigger.</summary>
        void Execute();
    }
}
