using System.Collections.Generic;
using SupermergeMonitor;

namespace SupermergeMonitor_Tests
{
    class EmailServiceMock : IEmailService
    {
        public bool SuccessEmailSent { get; set; }
        public bool FailureEmailSent { get; set; }
        public List<string> EmailRecipientsList { get; set; }

        private readonly List<string> _listOfEmailAddresses = new List<string> { "markbrown@fortegra.com", "lcasillas@fortegra.com", "moliver@fortegra.com" };

        public EmailServiceMock()
        {
            EmailRecipientsList = new List<string>();
        }

        public void SendSuccessEmail()
        {
            FailureEmailSent = false;
            SuccessEmailSent = true;

            EmailRecipientsList = _listOfEmailAddresses;
        }
        public void SendFailureEmail()
        {
            FailureEmailSent = true;
            SuccessEmailSent = false;

            EmailRecipientsList = _listOfEmailAddresses;
        }
    }
}
