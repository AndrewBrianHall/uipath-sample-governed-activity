using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleGovernedActivities.Tests
{
    public class MailTests
    {
        [Theory]
        [InlineData("person@mailinator.com", "@mailinator.com")]
        [InlineData("john.p.doe@mailinator.com", "@mailinator.com")]
        public void FindSingleDomain(string email, string expected)
        {
            var domain = MailGovernanceHelper.FindDomain(email);
            Assert.Equal(expected, domain);
        }

        [Theory]
        [InlineData("person@mailinator.com;john.doe@uipath.com,jane.doe@mailinator.com", new string[] { "@mailinator.com", "@uipath.com", "@mailinator.com" })]
        public void FindMultipleDomains(string emails, string[] expectedDomains)
        {
            var parsedEmails = MailGovernanceHelper.ParseEmailAddresses(emails);

            for (int i = 0; i < parsedEmails.Length; i++)
            {
                var email = parsedEmails[i];
                var expected = expectedDomains[i];

                var domain = MailGovernanceHelper.FindDomain(email);
                Assert.Equal(expected, domain);
            }
        }

        [Theory]
        [InlineData("person@mailinator.com,jane.doe@mailinator.com")]
        public void PermittedDomainTargets(string target)
        {
            MailGovernanceHelper.ValidateEmailAddresses(new string[] { target });
        }

        [Theory]
        [InlineData("john.doe@uipath.com")]
        public void ProhobitedDomainTargets(string targets)
        {
            Assert.Throws<InvalidOperationException>(() => { MailGovernanceHelper.ValidateEmailAddresses(new string[] { targets }); });
        }
    }
}
