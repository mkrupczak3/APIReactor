//-----------------------------------------------------------------------
// <copyright file="Reactor.cs" company="Laclede's LAN">
//     Applicable rights reserved
// </copyright>
//-----------------------------------------------------------------------

namespace SteamReaction
{
    using System;
    using System.Collections.Generic;
    using SteamReaction.Triggers;
    using SteamReaction.WebHooks;

    /// <summary>Structure to link steam applications and Web-Hooks triggers</summary>
    public class Reactor
    {
        /// <summary>Gets or sets the name of the Docker Image that will be triggered</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Gets or sets the collection of WebHooks to execute when an exception is thrown.</summary>
        public List<IWebHook> ExceptionWebHooks { get; set; } = null;

        /// <summary>Gets or sets the date and time of last trigger execution.</summary>
        public DateTime LastTriggered { get; set; } = default(DateTime);

        /// <summary>Gets or sets the collection of Triggers that trigger this reactor.</summary>
        public List<ITrigger> Triggers { get; set; } = null;

        /// <summary>Gets or sets the collection of WebHooks to execute when triggered.</summary>
        public List<IWebHook> WebHooks { get; set; } = null;

        public void Start()
        {
            try
            {
                if (this.CheckTriggers(this.Triggers))
                {
                    Reactor.ExecuteWebHooks(this.WebHooks);
                }
            }
            catch (Exception Ex)
            {
                if (this.ExceptionWebHooks != null && this.ExceptionWebHooks.Count > 0)
                {
                    Reactor.ExecuteWebHooks(this.ExceptionWebHooks);
                    this.LastTriggered = DateTime.Now;
                }
                else
                {
                    Console.WriteLine("Exception went uncaught in Reactor.Start()");
                    throw Ex;
                }
            }
        }

        internal bool CheckTriggers(List<ITrigger> triggers)
        {
            int triggersActivated = 0;
            bool returnResult = false;

            for (int i = 0; i < triggers.Count; i++)
            {
                bool result = triggers[i].Check();

                if (result)
                {
                    triggersActivated++;
                    returnResult = true;
                }

                if (i == 0)
                {
                    Program.ConsolePrintLine(this.Name, triggers[i].description, result, triggers[i].CurrentCheckInfo, '·');
                }
                else
                {
                    Program.ConsolePrintLine("", triggers[i].description, result, triggers[i].CurrentCheckInfo);
                }
            }

            return returnResult;
        }

        internal static void ExecuteWebHooks(List<IWebHook> webHooks)
        {
            if (webHooks != null && webHooks.Count > 0)
            {
                for (int i = 0; i < webHooks.Count; i++)
                {
                    webHooks[i].Execute();
                }
            }
        }
    }
}
