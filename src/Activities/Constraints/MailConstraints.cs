using System;
using System.Linq;
using System.Net.Mail;

namespace SampleGovernedActivities.Activities.Constraints
{
    public class ProhibitedEmailRecipientException : Exception
    {
        public ProhibitedEmailRecipientException(string message) : base(message) { }
    }

    public class TooManyRecipientsException : Exception
    {
        public TooManyRecipientsException(string message) : base(message) { }
    }

    internal class MailConstraints
    {
        public readonly string[] AllowedRecipientDomains;
        public readonly string ErrorMessage;

        bool _allowAllDomainsIfDraft;
        public bool AllowAllDomainsIfDraft { get { return _allowAllDomainsIfDraft; } }

        int _emailRecipientLimit;
        int _rolling24hRecipientLimit;
        bool _persistHistory;

        public MailConstraints(string[] allowedDomains, bool allowAllDomainsIfDraft, int emailRecipientLimit, int rolling24hRecipientLimit, bool persistHistory)
        {
            this.AllowedRecipientDomains = allowedDomains;
            this.ErrorMessage = $"You may only send emails to email addresses addressed to users matching @{string.Join(", or @*", this.AllowedRecipientDomains)}. " +
                "You may however, save emails to anyone as a draft and then manually send from your Drafts folder.";

            _allowAllDomainsIfDraft = allowAllDomainsIfDraft;
            _emailRecipientLimit = emailRecipientLimit;
            _rolling24hRecipientLimit = rolling24hRecipientLimit;
            _persistHistory = persistHistory;
        }

        public void ValidateEmailAddresses(MailAddressCollection addressCollection, bool isDraft)
        {
            //If email is set to 'Save as draft', no domain validation is needed
            if (this.AllowAllDomainsIfDraft && isDraft)
            {
                return;
            }

            foreach (MailAddress emailRecipient in addressCollection)
            {
                string recipientDomain = emailRecipient.Host;
                bool recipientOk = false;

                foreach (string allowedDomain in this.AllowedRecipientDomains)
                {
                    if (!string.IsNullOrEmpty(recipientDomain) && recipientDomain.EndsWith(allowedDomain, StringComparison.OrdinalIgnoreCase))
                    {
                        recipientOk = true;
                        break;
                    }
                }

                if (!recipientOk)
                {
                    throw new ProhibitedEmailRecipientException(this.ErrorMessage);
                }
            }

            ValidateRecipientCounts(addressCollection);
        }

        protected void ValidateRecipientCounts(MailAddressCollection addressCollection)
        {
            if (addressCollection.Count > _emailRecipientLimit)
            {
                throw new TooManyRecipientsException($"An email may contain a maximum of {_emailRecipientLimit} recipients");
            }

            DailyEmailHistory dailyHistory = new DailyEmailHistory();
            if (dailyHistory.GetRollingCount() + addressCollection.Count > _rolling24hRecipientLimit)
            {
                throw new TooManyRecipientsException($"You have exceeded your limit of {String.Format("{0:n0}", _rolling24hRecipientLimit)} email recipients in a 24 hour period");
            }
            else
            {
                dailyHistory.NewMailSent(addressCollection.Count, _persistHistory);
            }
        }
    }


}
