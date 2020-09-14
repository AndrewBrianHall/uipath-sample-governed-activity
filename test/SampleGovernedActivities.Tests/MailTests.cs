using SampleGovernedActivities.Activities.Constraints;
using SampleGovernedActivities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleGovernedActivities.Tests
{
    public class MailTests
    {
        [Theory]
        [InlineData("person@mailinator.com", "mailinator.com")]
        [InlineData("john.p.doe@mailinator.com", "mailinator.com")]
        public void FindSingleDomain(string email, string expected)
        {
            var addressCollection = MailHelper.CreateAddressCollection(email);
            var domain = addressCollection[0].Host;
            Assert.Equal(expected, domain);
        }

        [Theory]
        [InlineData("person@mailinator.com;john.doe@uipath.com,jane.doe@mailinator.com", new string[] { "mailinator.com", "uipath.com", "mailinator.com" })]
        public void FindMultipleDomains(string emails, string[] expectedDomains)
        {
            var emailCollection = MailHelper.CreateAddressCollection(emails);

            for (int i = 0; i < emailCollection.Count; i++)
            {
                var email = emailCollection[i];
                var expected = expectedDomains[i];

                Assert.Equal(expected, email.Host);
            }
        }

        [Theory]
        [InlineData("person@mailinator.com,jane.doe@mailinator.com", new string[] { "mailinator.com" })]
        [InlineData("Person <person@mailinator.com>, Jane H. Doe <jane.doe@mailinator.com>;bob@contoso.com", new string[] { "mailinator.com", "contoso.com" })]
        public void PermittedDomainTargets(string target, string[] allowedDomains)
        {
            var mailHelper = new MailConstraints(allowedDomains);
            var recipients = MailHelper.CreateAddressCollection(target);
            mailHelper.ValidateEmailAddresses(recipients);
        }

        [Theory]
        [InlineData("john.doe@uipath.com", new string[] { "mailinator.com" })]
        [InlineData("john.doe@uipath.com;bob@contoso.com", new string[] { "mailinator.com" })]
        public void ProhobitedDomainTargets(string targets, string[] allowedDomains)
        {
            var mailHelper = new MailConstraints(allowedDomains);
            var recipients = MailHelper.CreateAddressCollection(targets);
            Assert.Throws<ProhibitedEmailRecipientException>(() => { mailHelper.ValidateEmailAddresses(recipients); });
        }
        
        [Theory]
        [InlineData("john.doe@uipath.com", new string[] { "mailinator.com" })]
        public void SingleDomainErrorMessage(string targets, string[] allowedDomains)
        {
            string message = null;
            var mailHelper = new MailConstraints(allowedDomains);
            try
            {
                var recipients = MailHelper.CreateAddressCollection(targets);
                mailHelper.ValidateEmailAddresses(recipients);
            }
            catch(ProhibitedEmailRecipientException e)
            {
                message = e.Message;
            }

            Assert.Equal("You may only send emails to email addresses addressed to users matching @mailinator.com", message);
        }
        
        [Theory]
        [InlineData("john.doe@uipath.com", new string[] { "mailinator.com", "contoso.com" })]
        public void MultipleDomainErrorMessage(string targets, string[] allowedDomains)
        {
            string message = null;
            var mailHelper = new MailConstraints(allowedDomains);
            try
            {
                var recipients = MailHelper.CreateAddressCollection(targets);
                mailHelper.ValidateEmailAddresses(recipients);
            }
            catch(ProhibitedEmailRecipientException e)
            {
                message = e.Message;
            }

            Assert.Equal("You may only send emails to email addresses addressed to users matching @mailinator.com, or @contoso.com", message);
        }

        [Fact]
        public void MailMessageTest()
        {
            //MailMessage message = new MailMessage("test@mailinator.com", "John Doe <john.doe@mailaintor.com>");
            MailAddressCollection collection = new MailAddressCollection();
            collection.Add("John Doe <john.doe@mailaintor.com>,merrick.bob@uipath.com");
        }
    }
}
