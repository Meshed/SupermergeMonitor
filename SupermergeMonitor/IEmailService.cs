using System.Collections.Generic;

namespace SupermergeMonitor
{
    public interface IEmailService
    {
        /// <summary>
        /// Inspection property used for testing
        /// </summary>
        bool SuccessEmailSent { get; set; }
        /// <summary>
        /// Inspection property used for testing
        /// </summary>
        bool FailureEmailSent { get; set; }
        /// <summary>
        /// Inspection property used for testing
        /// </summary>
        List<string> EmailRecipientsList { get; set; }

        /// <summary>
        /// Sends an email specifying Supermerge is up and running
        /// </summary>
        void SendSuccessEmail();
        /// <summary>
        /// Sends an email specifying Supermerge is down
        /// </summary>
        void SendFailureEmail();
    }
}