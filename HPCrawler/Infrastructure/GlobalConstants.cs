using System.Collections.Generic;

namespace HPCrawler
{
    class GlobalConstants
    {
        public const string _ParameterRun = "-R";
        public const string _ParameterSetup = "-S";
        public const string _ParameterSendNotification = "-N";
        public const string _ParaInfoParameter = "Parameter :";
        public const string _ParaInfoSetup = "-S - Setup";
        public const string _ParaInfoRun = "-R - Run";
        public const string _ParaInfoNotify = "-N - Notify";

        public const string _MailTextDiffrentSites = "Seiten, welche sich seit dem letzten Scan geändert haben:";
        public const string _MailTextNewSites = "Seiten, welche seit dem letzten Scan neu hinzugefügt wurden:";
        public const string _MailTextNotFoundSites = "Seiten, welche nicht gefunden werden konnten (Error 404):";
    }
}
