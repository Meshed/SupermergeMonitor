using System;
using System.Threading;
using Fortegra.Utilities.Email;

namespace SupermergeMonitor
{
    class Program
    {
        /// <summary>
        /// Main method containing the application loop. Controls how often a check is done
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            IEmailService emailService = new EmailService(new EmailManager());
            var utilityMethods = new UtilityMethods(emailService) {LastLogFileName = 0};
            var timeSpan = new TimeSpan(0, 0, 10, 0);

            do
            {
                Console.WriteLine(DateTime.Now + " - " + "Checking Supermerge state...");
                utilityMethods.CheckSupermerge();
                Console.WriteLine(DateTime.Now + " - " + "Supermerge check complete");
                Console.WriteLine(DateTime.Now + " - " + "Sleeping for 10 minutes...");
                Thread.Sleep(timeSpan);
            } while (true);
        }
    }
}
