using System;
using System.Globalization;
using NUnit.Framework;
using SupermergeMonitor;

namespace SupermergeMonitor_Tests
{
    [TestFixture]
    public class UtilityMethodsTests
    {
        private UtilityMethods _utilityMethods;

        [TestFixtureSetUp]
        public void Setup()
        {
            IEmailService emailService = new EmailServiceMock();
            _utilityMethods = new UtilityMethods(emailService);
        }

        [TestFixture]
        public class TheGetTodaysDateMethod : UtilityMethodsTests
        {
            [Test]
            public void ReturnsTodaysDate()
            {
                string year = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                string month = DateTime.Now.Month.ToString(CultureInfo.InvariantCulture).Length == 1
                    ? "0" + DateTime.Now.Month
                    : DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
                string day = DateTime.Now.Day.ToString(CultureInfo.InvariantCulture).Length == 1
                    ? "0" + DateTime.Now.Day
                    : DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);
                string currentDate = year + month + day;

                Assert.AreEqual(currentDate, _utilityMethods.GetTodaysDate());
            }
        }

        [TestFixture]
        public class TheGet24TimeMethod : UtilityMethodsTests
        {
            [Test]
            public void Returns100For1AM()
            {
                Assert.AreEqual(100, _utilityMethods.Get24Time("0100", "0100AM"));
            }

            [Test]
            public void Returns1100For11AM()
            {
                Assert.AreEqual(1100, _utilityMethods.Get24Time("1100", "1100AM"));
            }

            [Test]
            public void Returns1200For12PM()
            {
                Assert.AreEqual(1200, _utilityMethods.Get24Time("1200", "1200PM"));
            }

            [Test]
            public void Returns1220For1220PM()
            {
                Assert.AreEqual(1220, _utilityMethods.Get24Time("1220", "1220PM"));
            }

            [Test]
            public void Returns1300For1PM()
            {
                Assert.AreEqual(1300, _utilityMethods.Get24Time("0100", "0100PM"));
            }

            [Test]
            public void Returns1330For130PM()
            {
                Assert.AreEqual(1330, _utilityMethods.Get24Time("0130", "0130PM"));
            }

            [Test]
            public void Returns1400For0200PM()
            {
                Assert.AreEqual(1400, _utilityMethods.Get24Time("0200", "0200PM"));
            }

            [Test]
            public void Returns0For12AM()
            {
                Assert.AreEqual(0, _utilityMethods.Get24Time("1200", "1200AM"));
            }

            [Test]
            public void Returns30For1230AM()
            {
                Assert.AreEqual(30, _utilityMethods.Get24Time("1230", "1230AM"));
            }
        }

        [TestFixture]
        public class TheGetLogFileTimeMethod : UtilityMethodsTests
        {
            [Test]
            public void Returns1300ForLOG_fulfillment_Supermerge_20140610_0100PM()
            {
                Assert.AreEqual(1300, _utilityMethods.GetLogFileTime("LOG_fulfillment_Supermerge_20140610_0100PM"));
            }
            [Test]
            public void Returns0100ForLOG_fulfillment_Supermerge_20140610_0100AM()
            {
                Assert.AreEqual(100, _utilityMethods.GetLogFileTime("LOG_fulfillment_Supermerge_20140610_0100AM"));
            }
        }
    }
}
