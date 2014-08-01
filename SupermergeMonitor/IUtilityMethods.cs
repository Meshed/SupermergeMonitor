namespace SupermergeMonitor
{
    public interface IUtilityMethods
    {
        /// <summary>
        /// Today's date
        /// </summary>
        string TodaysDate { get; set; }
        /// <summary>
        /// Log file path
        /// </summary>
        string LogFilePath { get; set; }
        /// <summary>
        /// Name of the last log file seen
        /// </summary>
        int LastLogFileName { get; set; }

        /// <summary>
        /// Handles the logic for determining if Supermerge is running or not
        /// </summary>
        void CheckSupermerge();
        /// <summary>
        /// Returns whether a directory exits for today
        /// </summary>
        /// <returns>Does directory exist?</returns>
        bool DoesLogDirectoryExistForToday();
        /// <summary>
        /// Returns the time of th latest file based on the log file names
        /// Filters with the following: 'LOG_fulfillment_Supermerge_*'
        /// </summary>
        /// <returns>Time of latest file</returns>
        int GetLatestLogFileTime();
        /// <summary>
        /// Parses the log file time from the log file name
        /// </summary>
        /// <param name="file">Log file name</param>
        /// <returns>Log file time</returns>
        int GetLogFileTime(string file);
        /// <summary>
        /// Converts time from log name into 24 hour time format
        /// </summary>
        /// <param name="timeLogCreated">Time from log file name</param>
        /// <param name="lastFilePart">Raw time from the log file name, specifies AM or PM</param>
        /// <returns>24 Hour time as integrer</returns>
        int Get24Time(string timeLogCreated, string lastFilePart);
        /// <summary>
        /// Gets today's date in the Year + Month + Day format
        /// </summary>
        /// <returns>Formated date</returns>
        string GetTodaysDate();
    }
}