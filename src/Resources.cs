using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGovernedActivities
{
    internal static class Resources
    {
        public const string RequiredActivityIdentifier = "(Permitted)";
        public static readonly string ForwardMailXDisplayName = $"Forward Mail {Resources.RequiredActivityIdentifier}";
        public static readonly string ReplyToMailXDisplayName = $"Reply to Email {Resources.RequiredActivityIdentifier}";
        public static readonly string SendMailXDisplayName = $"Send Email {Resources.RequiredActivityIdentifier}";
    }
}
