using NUnit.Framework;
using SupermergeMonitor;

namespace SupermergeMonitor_Tests
{
    [TestFixture]
    class EmailServiceTests
    {
        private IEmailService _emailService;

        [SetUp]
        public void SetupEmailServiceTests()
        {
            _emailService = new EmailServiceMock();
        }

        [TestFixture]
        public class TheSendSuccessEmailMethod : EmailServiceTests
        {
            [Test]
            public void SetsTheSuccessEmailSentFlagToTrue()
            {
                _emailService.SendSuccessEmail();

                Assert.IsTrue(_emailService.SuccessEmailSent);
            }

            [Test]
            public void SetsTheFailureEmailSentFlagToFalse()
            {
                _emailService.SendSuccessEmail();

                Assert.IsFalse(_emailService.FailureEmailSent);
            }

            [Test]
            public void PopulatesTheEmailRecipientList()
            {
                Assert.AreEqual(0, _emailService.EmailRecipientsList.Count);

                _emailService.SendSuccessEmail();
                
                Assert.Greater(_emailService.EmailRecipientsList.Count, 0);
            }
        }

        [TestFixture]
        public class TheSendFailureEmailMethod : EmailServiceTests
        {
            [Test]
            public void SetsTheSuccessEmailSentFlagToFalse()
            {
                _emailService.SendFailureEmail();

                Assert.IsFalse(_emailService.SuccessEmailSent);
            }

            [Test]
            public void SetsTheFailureEmailSentFlagToTrue()
            {
                _emailService.SendFailureEmail();

                Assert.IsTrue(_emailService.FailureEmailSent);
            }

            [Test]
            public void PopulatesTheEmailRecipientList()
            {
                Assert.AreEqual(0, _emailService.EmailRecipientsList.Count);

                _emailService.SendFailureEmail();

                Assert.Greater(_emailService.EmailRecipientsList.Count, 0);
            }
        }
    }
}
