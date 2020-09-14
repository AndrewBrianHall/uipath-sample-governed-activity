using System.Activities.Presentation.Metadata;
using System.ComponentModel;

namespace SampleGovernedActivities
{
    public class MetaDataProvider : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            var category = new CategoryAttribute("Business.Mail");
            //var displayName = new DisplayNameAttribute("Tutorial Display");
            builder.AddCustomAttributes(typeof(GovernedSendMail), category);
            //builder.AddCustomAttributes(typeof(TutorialActivity), displayName);
            //builder.AddCustomAttributes(typeof(TutorialActivity), new DesignerAttribute(typeof(TutorialDesigner)));
            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
