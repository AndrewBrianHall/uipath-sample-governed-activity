using SampleGovernedActivities.Activities.Constraints;
using System.Net.Mail;

namespace SampleGovernedActivities.Helpers
{
    internal class MailHelper
    {
        public MailAddressCollection Addresses { get; protected set; }

        public MailHelper()
        {
            this.Addresses = new MailAddressCollection(); 
        }

        public MailHelper(MailAddressCollection mailAddresses)
        {
            this.Addresses = mailAddresses;
        }

        public void AddAddresses(MailAddressCollection addresses)
        {
            foreach(var address in addresses)
            {
                this.Addresses.Add(address);
            }
        }

        public void AddAddresses(string addresses)
        {
            var addressCollection = ParseEmailAddresses(addresses);
            this.AddAddresses(addressCollection);
        }


        protected static MailAddressCollection MergeAddressCollections(MailAddressCollection collection1, MailAddressCollection collection2)
        {
            MailAddressCollection merged = new MailAddressCollection();
            foreach (var mailAddress in collection1)
            {
                merged.Add(mailAddress);
            }
            foreach (var mailAddress in collection2)
            {
                merged.Add(mailAddress);
            }

            return merged;
        }

        protected static string NormalizeAddresses(string addressSet)
        {
            if (!string.IsNullOrEmpty(addressSet))
            {
                //MailMessage collection will only accept , separators so normalize any ;'s to ,
                addressSet = addressSet.Replace(';', ',');
            }

            return addressSet;
        }

        internal static MailAddressCollection ParseEmailAddresses(string addressSet)
        {
            addressSet = NormalizeAddresses(addressSet);

            //System.Net.MailAddressCollection will handle parsing addresses into individual mails
            MailAddressCollection collection = new MailAddressCollection();
            collection.Add(addressSet);
            return collection;
        }

        public void ValidateAddresses(MailConstraints constraints)
        {
            constraints.ValidateEmailAddresses(this.Addresses);
        }
    }
}
