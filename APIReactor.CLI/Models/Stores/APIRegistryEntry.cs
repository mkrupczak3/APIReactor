//-----------------------------------------------------------------------
// <copyright file="APIRegistryEntry.cs" company="https://dudley.codes">
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

namespace DudleyCodes.APIReactorCLI
{
    using System;

    /// <summary>Represents a query-able API.</summary>
    public class APIRegistryEntry
    {
        /// <summary>Gets or sets the minimum amount of time between requests.</summary>
        public TimeSpan FloodDelay { get; set; }

        /// <summary>Gets or sets the last time the API was last queried.</summary>
        public DateTime LastQueried { get; set; } = default(DateTime);

        /// <summary>Gets the next DateTime that a query can be preformed.</summary>
        /// <returns>DateTime containing a value that indicates when a query can be preformed.</returns>
        public DateTime GetNextQueryTime()
        {
            return (this.LastQueried == default(DateTime)) ? DateTime.Now : this.LastQueried.Add(this.FloodDelay);
        }
    }
}
