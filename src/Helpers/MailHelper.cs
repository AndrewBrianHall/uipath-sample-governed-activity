using SampleGovernedActivities.Activities.Constraints;
using System.Net.Mail;

namespace SampleGovernedActivities.Helpers
{
    // This class is simply a helper for managing the MailAddressCollection.
    // Due to activity differences building a complete collection requires
    // dealing with both AddressCollections and strings, and converting and 
    // merging them to do the final validation check.
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

        // Merge an additional set of addresses
        public void AddAddresses(MailAddressCollection addresses)
        {
            foreach (var address in addresses)
            {
                this.Addresses.Add(address);
            }
        }

        // Add addresses represented as a string to the collection
        public void AddAddresses(string addresses)
        {
            var addressCollection = CreateAddressCollection(addresses);
            this.AddAddresses(addressCollection);
        }

        internal static MailAddressCollection CreateAddressCollection(string addressSet)
        {
            addressSet = NormalizeAddresses(addressSet);

            //System.Net.MailAddressCollection will handle parsing addresses
            MailAddressCollection collection = new MailAddressCollection();
            collection.Add(addressSet);
            return collection;
        }


        // Substitute characters acccepted by most mail systems to alternatives
        // supported by MailAddressCollection (e.g. addresses must be separated by , not ;)
        protected static string NormalizeAddresses(string addressSet)
        {
            if (!string.IsNullOrEmpty(addressSet))
            {
                addressSet = addressSet.Replace(';', ',');
            }

            return addressSet;
        }

        public void ValidateAddresses(MailConstraints constraints)
        {
            constraints.ValidateEmailAddresses(this.Addresses);
        }
    }
}
