using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Fortegra.Utilities.Email;

namespace SupermergeMonitor
{
    public class UtilityMethods
    {
        public string TodaysDate { get; set; }
        public string LogFilePath { get; set; }
        public int LastLogFileName { get; set; }

        public void CheckSupermerge()
        {
            TodaysDate = GetTodaysDate();
            LogFilePath = @"\\fulfillment\c$\APPS_FILES\SuperMerge\LOGS\" + TodaysDate;

            if (DoesLogDirectoryExistForToday())
            {
                int latestLogFileTime = GetLatestLogFileName();

                if (latestLogFileTime != LastLogFileName)
                {
                    LastLogFileName = latestLogFileTime;
                    Console.WriteLine(DateTime.Now + " - " + "Supermerge is running.");
                }
                else
                {
                    Console.WriteLine(DateTime.Now + " - " + "Supermerge is not running or is locked.");
                    var emailManager = new EmailManager();

                    emailManager.SendMail("Supermerge Monitor", "DoNotReply@Fortegra.com", "Pass5777", new List<string> { "markbrown@fortegra.com" }, null, null, "Supermerge Down", "Supermerge has not created a new log file in the last ten minutes.", null);
                }
            }
            else
            {
                Console.WriteLine(DateTime.Now + " - " + "Supermerge is not running or is locked.");
                var emailManager = new EmailManager();

                emailManager.SendMail("Supermerge Monitor", "DoNotReply@Fortegra.com", "Pass5777", new List<string> { "markbrown@fortegra.com" }, null, null, "Supermerge Down", "Supermerge has not created a new log file in the last ten minutes.", null);
            }
        }

        public bool DoesLogDirectoryExistForToday()
        {
            return Directory.Exists(LogFilePath);
        }

        public int GetLatestLogFileName()
        {
            string[] files = Directory.GetFiles(LogFilePath, "LOG_fulfillment_Supermerge_*");

            return files.Select(GetLogFileTime).Concat(new[] {0}).Max();
        }

        public int GetLogFileTime(string file)
        {
            int time;

            string[] fileParts = file.Split('_');

            int lastPartIndex = fileParts.GetUpperBound(0);
            string lastFilePart = fileParts[lastPartIndex];

            string timeLogCreated = lastFilePart.Substring(0, 4);

            time = Get24Time(timeLogCreated, lastFilePart);

            return time;
        }

        public int Get24Time(string timeLogCreated, string lastFilePart)
        {
            int time;
            if (Int32.TryParse(timeLogCreated, out time))
            {
                if (lastFilePart.Contains("PM"))
                {
                    if (time >= 100 && time < 1200)
                    {
                        time += 1200;
                    }
                }
                else
                {
                    if (time == 1200)
                    {
                        time = 0;
                    }
                }
            }
            return time;
        }

        public string GetTodaysDate()
        {
            string todaysDate = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
            todaysDate += DateTime.Now.Month.ToString(CultureInfo.InvariantCulture).Length == 1
                ? "0" + DateTime.Now.Month
                : DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
            todaysDate += DateTime.Now.Day.ToString(CultureInfo.InvariantCulture).Length == 1
                ? "0" + DateTime.Now.Day
                : DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);

            return todaysDate;
        }
    }
}
