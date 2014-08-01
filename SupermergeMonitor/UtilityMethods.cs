using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SupermergeMonitor
{
    public class UtilityMethods : IUtilityMethods
    {
        /// <summary>
        /// Today's date
        /// </summary>
        public string TodaysDate { get; set; }
        /// <summary>
        /// Log file path
        /// </summary>
        public string LogFilePath { get; set; }
        /// <summary>
        /// Name of the last log file seen
        /// </summary>
        public int LastLogFileName { get; set; }

        private readonly IEmailService _emailService;

        /// <summary>
        /// Constructor for the Utility Methods class
        /// </summary>
        /// <param name="emailService">IEmailService used for sending notification emails</param>
        public UtilityMethods(IEmailService emailService)
        {
            _emailService = emailService;
            LastLogFileName = 0;
        }
        /// <summary>
        /// Handles the logic for determining if Supermerge is running or not.
        /// Supermerge runs every 10 minutes and creates a log file each time
        /// it runs. This method looks to see if there is a new file and compares
        /// the dates in the file names with the date of the last log file seen.
        /// When first started, looks for the creation of a directory with today's
        /// date to determine if Supermerge is running.
        /// </summary>
        public void CheckSupermerge()
        {
            TodaysDate = GetTodaysDate();
            LogFilePath = @"\\fulfillment\c$\APPS_FILES\SuperMerge\LOGS\" + TodaysDate;

            if (DoesLogDirectoryExistForToday())
            {
                int latestLogFileTime = GetLatestLogFileTime();

                if (latestLogFileTime != LastLogFileName)
                {
                    if (LastLogFileName != 0)
                    {
                        Console.WriteLine(DateTime.Now + " - " + "Supermerge is running.");
                        _emailService.SendSuccessEmail();                        
                    }

                    LastLogFileName = latestLogFileTime;
                }
                else
                {
                    Console.WriteLine(DateTime.Now + " - " + "Supermerge is not running or is locked.");
                    _emailService.SendFailureEmail();
                }
            }
            else
            {
                Console.WriteLine(DateTime.Now + " - " + "Supermerge is not running or is locked.");
                _emailService.SendFailureEmail();
            }
        }
        /// <summary>
        /// Returns whether a directory exits for today
        /// </summary>
        /// <returns>Does directory exist: true</returns>
        public bool DoesLogDirectoryExistForToday()
        {
            return Directory.Exists(LogFilePath);
        }
        /// <summary>
        /// Returns the time of the latest file based on the log file names
        /// Filters with the following: 'LOG_fulfillment_Supermerge_*'
        /// </summary>
        /// <returns>Time of latest file: 1300</returns>
        public int GetLatestLogFileTime()
        {
            string[] files = Directory.GetFiles(LogFilePath, "LOG_fulfillment_Supermerge_*");

            return files.Select(GetLogFileTime).Concat(new[] {0}).Max();
        }
        /// <summary>
        /// Parses the time from the log file name
        /// </summary>
        /// <param name="file">Log file name: "LOG_fulfillment_Supermerge_20140610_0100PM"</param>
        /// <returns>Log file time: 1300</returns>
        public int GetLogFileTime(string file)
        {
            string[] fileParts = file.Split('_');

            int lastPartIndex = fileParts.GetUpperBound(0);
            string lastFilePart = fileParts[lastPartIndex];

            string timeLogCreated = lastFilePart.Substring(0, 4);

            int time = Get24Time(timeLogCreated, lastFilePart);

            return time;
        }
        /// <summary>
        /// Converts time from log name into 24 hour format integer
        /// </summary>
        /// <param name="timeLogCreated">Time from log file name: "0100"</param>
        /// <param name="lastFilePart">Raw time from the log file name, specifies AM or PM: "0100AM"</param>
        /// <returns>24 Hour time as integrer: 100</returns>
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
                    else if (time > 1200 && time <= 1259)
                    {
                        time -= 1200;
                    }
                }
            }
            return time;
        }
        /// <summary>
        /// Gets today's date in the Year + Month + Day format
        /// </summary>
        /// <returns>Formated date: 20140630</returns>
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
