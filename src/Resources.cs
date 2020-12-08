using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGovernedActivities
{
    internal static class Resources
    {
        public const string RequiredActivityIdentifier = "(Use This)";
        public static readonly string ForwardMailXDisplayName = $"EY Forward Mail {Resources.RequiredActivityIdentifier}";
        public static readonly string ReplyToMailXDisplayName = $"EY Reply to Email {Resources.RequiredActivityIdentifier}";
        public static readonly string SendMailXDisplayName = $"EY Send Email {Resources.RequiredActivityIdentifier}";
    }
}
