using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleGovernedActivities
{

    [DisplayName("Custom Send Mail")]
    public class GovernedSendMail : UiPath.Mail.Activities.Business.SendMailX
    {

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            string toRecipients = To.Get(context);
            string ccRecipients = Cc.Get(context);
            string bccRecipients = Bcc.Get(context);
            var allRecipients = new string[] { toRecipients, ccRecipients, bccRecipients };
            MailGovernanceHelper.ValidateEmailAddresses(allRecipients);


            await base.ExecuteAsync(context, cancellationToken);
            return _ => { };
        }

    }
}
