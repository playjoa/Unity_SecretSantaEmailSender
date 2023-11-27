using System.Collections.Generic;

namespace AppWideSystems.EmailSystem.Data
{
    public struct EmailContentData
    {
        public List<string> EmailTargets;
        public bool IsHtmlBody;
        public string EmailDisplayName;
        public string EmailSubject;
        public string EmailBody;
    }
}