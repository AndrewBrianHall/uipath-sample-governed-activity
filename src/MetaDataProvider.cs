using SampleGovernedActivities.Activities;
using System.Activities;
using System.Activities.Presentation.Metadata;
using System.ComponentModel;

namespace SampleGovernedActivities
{
    //This class registers the activities so they can be found in Studio(X).
    public class MetaDataProvider : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();

            // In StudioX, an activity's catgory must start with "Business.". 
            // In this case, we'll place the "Mail" activities into the "Mail" category since
            // we want users to use these instead of the included ones
            var studioXMailCategory = new CategoryAttribute("Business.Mail");

            // Register the mail activities            
            builder.AddCustomAttributes(typeof(GovernedForwardMailX), studioXMailCategory);
            builder.AddCustomAttributes(typeof(GovernedReplyToMailX), studioXMailCategory);
            builder.AddCustomAttributes(typeof(GovernedSendMailX), studioXMailCategory);

            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
