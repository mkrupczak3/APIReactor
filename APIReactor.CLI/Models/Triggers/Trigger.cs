//-----------------------------------------------------------------------
// <copyright file="Trigger.cs" company="https://dudley.codes">
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

namespace DudleyCodes.APIReactor.Triggers
{
    using System;
    using Newtonsoft.Json;

    /// <summary>Base class to reactor triggers.</summary>
    public abstract class Trigger
    {
        /// <summary>Gets or sets the name of the API this trigger uses.</summary>
        public string APIName { get; protected set; } = string.Empty;

        /// <summary>Gets or sets a human-readable summary of the result of most recent check.</summary>
        public string CurrentCheckInfo { get; protected set; } = string.Empty;

        /// <summary>Gets or sets a human-readable description of what this trigger is about.</summary>
        public string Description { get; protected set; } = string.Empty;

        /// <summary>The Date and Time the trigger's parent was activated.</summary>
        public virtual DateTime ParentLastActivated { get; set; }

        /// <summary>The value from the check</summary>
        /// <remarks>Use value to determine if things have changed since the last trigger.</remarks>
        public string PreviousCheckResult = string.Empty;

        /// <summary>Checks the conditions of the trigger to determine if it should be activated.</summary>
        /// <returns>True if trigger activated; otherwise false.</returns>
        public abstract bool Check();

        /// <summary>Validates that the trigger has properly built by the end user.</summary>
        /// <returns>True if the trigger is valid; otherwise false.</returns>
        public abstract bool Validate();
    }
}
