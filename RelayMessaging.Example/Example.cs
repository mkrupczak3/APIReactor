//-----------------------------------------------------------------------
// <copyright file="Example.cs" company="https://dudley.codes">
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
//-----------------------------------------------------------------------

namespace DudleyCodes.RelayMessaging
{
    using System;

    /// <summary>Example class to demonstrate the usage of <see cref="DudleyCodes.RelayMessaging"/>.</summary>
    public class Example
    {
        /// <summary>Entry point to the example application.</summary>
        /// <param name="args">Array of command line arguments.</param>
        internal static void Main(string[] args)
        {
            SlackTeam slackTeam = new SlackTeam("services/XXXXXXXXX/YYYYYYYYY/ZZZZZZZZZZZZZZZZZZZZZZZZ");
            SlackChannel slackChannel = new SlackChannel(slackTeam, "general", "Jane", ":innocent:");
            SlackChannel slackChannel2 = new SlackChannel(slackTeam, "random");

            RelayMessage message = new RelayMessage();
            message.AddLine("line one", "line two")
                .AddText("hello")
                .AddLineBreak()
                .AddUrl("https://example.com", "description")
                .Send(slackChannel, slackChannel2);

            Console.Write("\n\nfin");
            Console.ReadKey();
        }
    }
}
