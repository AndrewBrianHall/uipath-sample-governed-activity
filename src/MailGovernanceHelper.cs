using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SampleGovernedActivities
{
    internal class MailGovernanceHelper
    {
        public static readonly string[] AllowedRecipientDomains = { "@mailinator.com" };
        internal static readonly string ErrorMessage = $"You may only send emails to email addresses addressed to users matching {string.Join(", or ", MailGovernanceHelper.AllowedRecipientDomains)}";

        internal static string FindDomain(string address)
        {
            if (string.IsNullOrEmpty(address) || !address.Contains("@"))
            {
                return null;
            }

            return address.Trim().Substring(address.IndexOf('@'));
        }


        internal static void ValidateEmailAddresses(string[] addresses)
        {
            foreach (var addressSet in addresses)
            {
                string[] emailsInSet = ParseEmailAddresses(addressSet);

                foreach (var emailRecipient in emailsInSet)
                {
                    string recipientDomain = MailGovernanceHelper.FindDomain(emailRecipient);

                    if (!string.IsNullOrEmpty(recipientDomain) && !MailGovernanceHelper.AllowedRecipientDomains.Contains(recipientDomain, StringComparer.OrdinalIgnoreCase))
                    {
                        throw new InvalidOperationException(MailGovernanceHelper.ErrorMessage);
                    }
                }
            }
        }

        internal static string[] ParseEmailAddresses(string addressSet)
        {
            return addressSet.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
