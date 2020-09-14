using System;
using System.Linq;
using System.Net.Mail;

namespace SampleGovernedActivities.Activities.Constraints
{
    public class ProhibitedEmailRecipientException : Exception
    {
        public ProhibitedEmailRecipientException(string message): base(message) { }
    }

    internal class MailConstraints
    {
        public readonly string[] AllowedRecipientDomains;
        public readonly string ErrorMessage;

        public MailConstraints(string[] allowedDomains)
        {
            this.AllowedRecipientDomains = allowedDomains;
            this.ErrorMessage = $"You may only send emails to email addresses addressed to users matching @{string.Join(", or @", this.AllowedRecipientDomains)}";
        }

        public void ValidateEmailAddresses(MailAddressCollection addressCollection)
        {
            foreach (var emailRecipient in addressCollection)
            {
                string recipientDomain = emailRecipient.Host;

                if (!string.IsNullOrEmpty(recipientDomain) && !this.AllowedRecipientDomains.Contains(recipientDomain, StringComparer.OrdinalIgnoreCase))
                {
                    throw new ProhibitedEmailRecipientException(this.ErrorMessage);
                }
            }
        }
    }
}
