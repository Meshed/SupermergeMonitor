using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Fortegra.Utilities.Email;

namespace SupermergeMonitor
{
    public class EmailService : IEmailService
    {
        private readonly IEmailManager _emailManager;
        private List<string> _emailAddressList = new List<string>();

        /// <summary>
        /// Inspection property used for testing
        /// </summary>
        public bool SuccessEmailSent { get; set; }
        /// <summary>
        /// Inspection property used for testing
        /// </summary>
        public bool FailureEmailSent { get; set; }

        /// <summary>
        /// Inspection property used for testing
        /// </summary>
        public List<string> EmailRecipientsList
        {
            get { return _emailAddressList; }
            set { _emailAddressList = value; }
        }

        /// <summary>
        /// Constructor for the Email Service
        /// </summary>
        /// <param name="emailManager">IEmailManager</param>
        public EmailService(IEmailManager emailManager)
        {
            _emailManager = emailManager;
            _emailAddressList = GetEmailRecipients();
        }

        /// <summary>
        /// Sends an email specifying Supermerge is up and running
        /// </summary>
        public void SendSuccessEmail()
        {
            const string emailBody = "Supermerge is running.";

            SendNotificationEmail("Supermerge Is Up", emailBody);
            SuccessEmailSent = true;
            FailureEmailSent = false;
        }
        /// <summary>
        /// Sends an email specifying Supermerge is down
        /// </summary>
        public void SendFailureEmail()
        {
            const string emailBody = "Supermerge has not created a new log file in the last ten minutes.";

            SendNotificationEmail("Supermerge Is Down", emailBody);
            FailureEmailSent = true;
            SuccessEmailSent = false;
        }

        /// <summary>
        /// Sends a notification email
        /// </summary>
        /// <param name="subject">Subject used for the notification email</param>
        /// <param name="body">Body used for the notification email</param>
        private void SendNotificationEmail(string subject, string body)
        {
            _emailManager.SendMail("Supermerge Monitor",
                "DoNotReply@Fortegra.com", // From address
                "Pass5777", // SMTP Server password
                _emailAddressList, // List of recipients
                null,
                null,
                subject, // Email subject
                body, // Email body
                null);
        }

        private List<string> GetEmailRecipients()
        {
            string recipients = ConfigurationManager.AppSettings["NotificationRecipients"];
            String[] recipientList = recipients.Split(',');
            return recipientList.ToList();
        }
    }
}
