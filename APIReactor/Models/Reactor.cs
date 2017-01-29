//-----------------------------------------------------------------------
// <copyright file="Reactor.cs" company="n/a">
//     Applicable rights reserved
// </copyright>
//-----------------------------------------------------------------------

namespace APIReactor
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using APIReactor.Triggers;
    using APIReactor.WebHooks;

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

        /// <summary>Start the reactor; checking all triggers for activations and (if applicable) executing all Web Hooks.</summary>
        public void Start()
        {
            try
            {
                if (this.CheckForActivations(this.Triggers))
                {
                    Reactor.ExecuteWebHooks(this.WebHooks);
                }
            }
            catch (Exception ex)
            {
                if (this.ExceptionWebHooks != null && this.ExceptionWebHooks.Count > 0)
                {
                    Reactor.ExecuteWebHooks(this.ExceptionWebHooks);
                    this.LastTriggered = DateTime.Now;
                }
                else
                {
                    Console.WriteLine("Exception went uncaught in Reactor.Start()");
                    throw ex;
                }
            }
        }

        /// <summary>Executes all passed Web Hooks</summary>
        /// <param name="webHooks">Collection of Web Hooks to Execute</param>
        internal static void ExecuteWebHooks(List<IWebHook> webHooks)
        {
            if (webHooks != null && webHooks.Count > 0)
            {
                for (int i = 0; i < webHooks.Count; i++)
                {
                    if (Program.DevDisableWebHooks)
                    {
                        Debug.WriteLine("Skipping web-hook: " + webHooks[i].GetType().Name);
                    }
                    else
                    {
                        webHooks[i].Execute();
                    }
                }
            }
        }

        /// <summary>Checks all triggers in a collection for activations.</summary>
        /// <param name="triggers">Collection of triggers to check.</param>
        /// <returns>True if one or more triggers activated; otherwise false.</returns>
        internal bool CheckForActivations(List<ITrigger> triggers)
        {
            int triggersActivated = 0;
            bool returnResult = false;
            DateTime triggerTime;

            for (int i = 0; i < triggers.Count; i++)
            {
                triggerTime = APIRegistry.GetNextQueryTime(triggers[i].APIName);
                Terminal.CountdownTo("Querying In", triggerTime);
                bool result = triggers[i].Check();
                APIRegistry.AddQueryRecord(triggers[i].APIName);

                if (result)
                {
                    triggersActivated++;
                    returnResult = true;
                }

                if (i == 0)
                {
                    Terminal.PrintLine(this.Name, triggers[i].Description, result, triggers[i].CurrentCheckInfo, '·');
                }
                else
                {
                    Terminal.PrintLine(string.Empty, triggers[i].Description, result, triggers[i].CurrentCheckInfo);
                }
            }

            return returnResult;
        }
    }
}
