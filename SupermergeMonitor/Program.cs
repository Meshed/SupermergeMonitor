using System;
using System.Threading;

namespace SupermergeMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            var utilityMethods = new UtilityMethods {LastLogFileName = 0};

            do
            {
                Console.WriteLine(DateTime.Now + " - " + "Checking Supermerge state...");
                utilityMethods.CheckSupermerge();
                Console.WriteLine(DateTime.Now + " - " + "Supermerge check complete");
                Console.WriteLine(DateTime.Now + " - " + "Sleeping for 10 minutes...");
                Thread.Sleep(600000);
            } while (true);
        }
    }
}
