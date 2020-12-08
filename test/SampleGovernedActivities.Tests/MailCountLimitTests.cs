using SampleGovernedActivities.Activities.Constraints;
using SampleGovernedActivities.Helpers;
using System;
using System.IO;
using Xunit;

namespace SampleGovernedActivities.Tests
{
    public class MailCountLimitTests
    {

        internal void CleanMailHistory()
        {
            if (File.Exists(DailyEmailHistory.StorageFile))
            {
                lock (DailyEmailHistory.FileLock)
                {
                    File.Delete(DailyEmailHistory.StorageFile);
                }
            }
        }

        [Fact]
        public void NoDailyMailsYet()
        {
            CleanMailHistory();

            var mailCounts = new DailyEmailHistory();
            Assert.Equal(0, mailCounts.GetRollingCount());
        }

        [Fact]
        public void RollingCount()
        {
            CleanMailHistory();

            int currentCount = -1;

            var mailCounts = new DailyEmailHistory();
            mailCounts.NewMailSent(1, false);
            mailCounts.NewMailSent(1, false);
            mailCounts.NewMailSent(5, false);
            currentCount = mailCounts.GetRollingCount();

            Assert.Equal(7, currentCount);
        }

        [Fact]
        public void SimplePersistence()
        {
            CleanMailHistory();

            var mailCounts = new DailyEmailHistory();
            mailCounts.NewMailSent(1, false);
            mailCounts.NewMailSent(1, false);
            mailCounts.NewMailSent(5);

            var newMailCounts = new DailyEmailHistory();

            Assert.Equal(7, newMailCounts.GetRollingCount());
        }

        [Fact]
        public void FilterOnLoad()
        {
            CleanMailHistory();

            var mailCounts = new DailyEmailHistory();
            DateTime emailTimeStamp = DateTime.Now - new TimeSpan(25, 0, 0);

            mailCounts.AddMailRecord(new EmailRecord(1, emailTimeStamp), false);
            mailCounts.AddMailRecord(new EmailRecord(1, emailTimeStamp + new TimeSpan(0, 10, 15)), false);
            mailCounts.AddMailRecord(new EmailRecord(1, emailTimeStamp + new TimeSpan(0, 15, 15)), false);
            mailCounts.AddMailRecord(new EmailRecord(1), true);

            Assert.True(File.Exists(DailyEmailHistory.StorageFile));

            var newMailCounts = new DailyEmailHistory();
            Assert.Equal(1, newMailCounts.GetRollingCount());
        }


        [Theory]
        [InlineData("john.doe@mailinator.com", 1)]
        [InlineData("jane.doe@mailinator.com;john.doe@mailinator.com", 2)]
        public void ValidIndividualMailRecipientLimits(string targets, int recipientLimit)
        {
            var mailHelper = new MailConstraints(new string[] { "mailinator.com" }, false, recipientLimit, 10000, false);
            var recipients = MailHelper.CreateAddressCollection(targets);
            mailHelper.ValidateEmailAddresses(recipients, false);
        }

        [Theory]
        [InlineData("jane.doe@mailinator.com;john.doe@mailinator.com", 1)]
        [InlineData("jane.doe@mailinator.com;john.doe@mailinator.com;bob@mailinator.com", 1)]
        public void InvalidIndividualMailRecipientLimits(string targets, int recipientLimit)
        {
            var mailHelper = new MailConstraints(new string[] { "mailinator.com" }, false, recipientLimit, 10000, false);
            var recipients = MailHelper.CreateAddressCollection(targets);
            var ex = Assert.Throws<TooManyRecipientsException>(() => { mailHelper.ValidateEmailAddresses(recipients, false); });
            Assert.StartsWith("An email may contain a maximum of", ex.Message);
        }


    }
}
