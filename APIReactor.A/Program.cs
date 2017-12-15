using System;

namespace DudleyCodes.APIReactor
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildData.BuildReactors();

            Console.WriteLine("press any key to continue");
            Console.ReadKey();
        }
    }
}