using SampleGovernedActivities.Activities.Constraints;
using SampleGovernedActivities.Helpers;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UiPath.Mail.Activities.Business;
using UiPath.Shared.Activities;

namespace SampleGovernedActivities.Activities
{
    //Creates a common set of constraints used in the all governed mail activities
    internal static class MailConstraintSettings
    {
        //This list is the domains (@<domain>) that mails may be sent to. Can be one or more
        public static readonly string[] AllowedDomains = new string[] { "mailinator.com" };
        public static readonly MailConstraints MailHelper = new MailConstraints(AllowedDomains);
    }

    //Creates a governed "Forward Email" activity for use in StudioX
    [DisplayName("Forward Mail - Required")]
    public class GovernedForwardMailX : ForwardMailX
    {
        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            MailHelper recipients = new MailHelper();

            recipients.AddAddresses(base.To.Get(context));
            recipients.AddAddresses(base.Cc.Get(context));
            recipients.AddAddresses(base.Bcc.Get(context));

            //Validate will throw an exception if any recipient's domain is not in the permitted list
            //If an exception is throw, the function will exit prior to sending in the next line
            recipients.ValidateAddresses(MailConstraintSettings.MailHelper);

            //If no exception is thrown by the validation check, call the base classes "ExecuteAsync"
            //which will send the message per the settings
            await base.ExecuteAsync(context, cancellationToken);
            return _ => { };
        }
    }

    //Creates a governed "Reply to Email" activity for use in StudioX
    [DisplayName("Reply to Mail - Required")]
    public class GovernedReplyToMailX : ReplyToMailX
    {

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            MailMessage message = base.MailMessage.Get(context);

            MailHelper recipients = new MailHelper(message.To);

            //Create collection of original recipients
            recipients.AddAddresses(message.CC);
            recipients.AddAddresses(message.Bcc);

            //Add new recipients to collection
            recipients.AddAddresses(base.AdditionalCc.Get(context));
            recipients.AddAddresses(base.AdditionalCc.Get(context));
            recipients.AddAddresses(base.Bcc.Get(context));

            //Validate will throw an exception if any recipient's domain is not in the permitted list
            //If an exception is throw, the function will exit prior to sending in the next line
            recipients.ValidateAddresses(MailConstraintSettings.MailHelper);

            //If no exception is thrown by the validation check, call the base classes "ExecuteAsync"
            //which will send the message per the settings
            await base.ExecuteAsync(context, cancellationToken);
            return _ => { };
        }
    }

    //Creates a governed "Send Email" activity for use in StudioX
    [DisplayName("Send Mail - Required")]
    public class GovernedSendMailX : SendMailX
    {

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            MailHelper recipients = new MailHelper();

            //Retrieve the values stored in the activities To, Cc, and Bcc properties
            recipients.AddAddresses(base.To.Get(context));
            recipients.AddAddresses(base.Cc.Get(context));
            recipients.AddAddresses(base.Bcc.Get(context));

            //Validate will throw an exception if any recipient's domain is not in the permitted list
            //If an exception is throw, the function will exit prior to sending in the next line
            recipients.ValidateAddresses(MailConstraintSettings.MailHelper);

            //If no exception is thrown by the validation check, call the base classes "ExecuteAsync"
            //which will send the message per the settings
            await base.ExecuteAsync(context, cancellationToken);
            return _ => { };
        }

    }

}
