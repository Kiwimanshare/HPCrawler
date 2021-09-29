using System;
using System.Collections.Generic;

namespace HPCrawler
{
    partial class Configuration
    {
        public List<string> _Log
        {
            get;
        }

        public string _LogFormat { get { return "[" + DateTime.Now + "][{0}]\t - {1}"; } }
        public string _LogError { get { return "Error"; } }
        public string _LogInfo { get { return "Info"; } }
        public string _LogOldFormat { get { return "{0}\\{1}_{2}_{3} - {4}"; } }

        public string _SiteNotFound { get { return "Site not Found"; } }

        private const string _NotFound = "Not Found";
        private const string _ErrorReadingAppSettings = "Error reading app settings";
        private const string _ErrorWritingAppSettings = "Error writing app settings";

        private const string _SetupDoneText = "Setup done!";

        private const string _EncryptionError = "Encryption Error, String not in the correct form";

        private const string _SetupDataType = "0 - Use Database, 1 - Use Text-File, X - Keep current Value:";
        private readonly List<char> _SetupDataTypeList = new List<char>() { '0', '1', 'x', 'X' };

        private const string _SetupMailType = "0 - Use E-Mail notification, 1 - No E-Mail notification, X - Keep current Value:";
        private readonly List<char> _SetupMailTypeList = new List<char>() { '0', '1', 'x', 'X' };

        private const string _SetupLog = "0 - Log Crawler, 1 - Do not log Crawler, X - Keep current Value:";
        private readonly List<char> _SetupLogTypeList = new List<char>() { '0', '1', 'x', 'X' };

        private const string _SetupProxy = "0 - Use Web-Proxy, 1 - Do not use Web-Proxyr, X - Keep current Value:";
        private readonly List<char> _SetupProxyTypeList = new List<char>() { '0', '1', 'x', 'X' };

        private const string _SetupQueries = "0 - Setup Queries, 1 - Do not Setup Queries, X - Keep current Value:";
        private readonly List<char> _SetupQueryList = new List<char>() { '0', '1', 'x', 'X' };

        private const string _ConfigInfoText = "Set up Configuration for HPCrawler:";

        private const string _ConfigDBServerText = "Insert DB-Server, X - Keep current Value:";
        private const string _ConfigDBDatabaseText = "Insert DB-Database, X - Keep current Value:";
        private const string _ConfigDBUserText = "Insert DB-User, X - Keep current Value:";
        private const string _ConfigDBPWText = "Insert DB-Password, X - Keep current Value:";

        private const string _ConfigDBTabaleNameText = "Insert Table Name, X - Keep current Value:";
        private const string _ConfigDBTableURLHashPKText = "Insert URL Primary Key Field Name, X - Keep current Value:";
        private const string _ConfigDBTableNameURLText = "Insert URL Field Name, X - Keep current Value:";
        private const string _ConfigDBTableNameLastScanText = "Insert Last Scan Time Field Name, X - Keep current Value:";
        private const string _ConfigDBTableNameHashCurrentText = "Insert Current Hash Field Name, X - Keep current Value:";
        private const string _ConfigDBTableNameHashNewText = "Insert Last Hash Field Name, X - Keep current Value:";

        private const string _ConfigTxtPathText = "Insert File-Path, X - Keep current Value:";

        private const string _ConfigMailSMTPText = "Insert SMTP-Server, X - Keep current Value:";
        private const string _ConfigMailPortText = "Insert SMTP-Port, X - Keep current Value:";
        private const string _ConfigMailSenderText = "Insert sender E-Mail, X - Keep current Value:";
        private const string _ConfigMailReceiverText = "Insert receiver E-Mail, X - Keep current Value:";

        private const string _ConfigLogDoText = "Insert Log-File-Path, X - Keep current Value:";
        private const string _ConfigLogMaxLenText = "Insert Log-Maximum-File-Size (1024 - 2147483647), X - Keep current Value:";

        private const string _ConfigProxyServerText = "Insert Web-Proxy-Adress, X - Keep current Value:";
        private const string _ConfigProxyPortText = "Insert Web-Proxy-Port, X - Keep current Value:";

        private const string _ConfigMainURLText = "Insert Webpage URL (Entrypoint), X - Keep current Value:";

        private const string _ConfigQueryNewSiteText = "Insert Query to get New Sites, X - Keep current Value:";
        private const string _ConfigQuerySiteNotFoundText= "Insert Query to get Not Found Sites, X - Keep current Value:";
        private const string _ConfigQueryDiffrentSitesText = "Insert Query to get Diffrent Sites, X - Keep current Value:";

        public string DataType { get; set; }
        public bool IsMailer { get; set;  }
        public bool IsLogger { get; set; }
        public bool IsProxy { get; set; }

        public string ConfigTrue { get { return "true"; } }
        public string ConfigFalse { get { return "false"; } }

        public string ConfigDataSaveType { get { return "DataSaveType"; } }
        public string ConfigDataSaveTypeDB { get { return "Database"; } }
        public string ConfigDataSaveTypeTXT { get { return "TextFile"; } }

        public string ConfigDBServer { get { return "DBServer"; } }
        public string ConfigDBDatabase { get { return "DBDatabase"; } }
        public string ConfigDBUser { get { return "DBUser"; } }
        public string ConfigDBPW { get { return "DBPW"; } }

        public string ConfigDBTableName { get { return "TableName"; } }
        public string ConfigDBTableURLHashPK { get { return "URLHashPKTableName"; } }
        public string ConfigDBTableNameURL { get { return "URLTableName"; } }
        public string ConfigDBTableNameLastScan { get { return "LastScanTableName"; } }
        public string ConfigDBTableNameHashCurrent { get { return "HashCurrentTableName"; } }
        public string ConfigDBTableNameHashNew { get { return "HashNewTableName"; } }

        public string ConfigTxtPath { get { return "TxtPath"; } }

        public string ConfigLogLogging { get { return "Logging"; } }
        public string ConfigLogPath { get { return "LogPath"; } }
        public string ConfigLogMaxLen { get { return "LogMaxLen"; } }

        public string ConfigMailNotification { get { return "MailNotification"; } }
        public string ConfigMailSMTP { get { return "MailSMTP"; } }
        public string ConfigMailPort { get { return "MailPort"; } }
        public string ConfigMailSender { get { return "MailSender"; } }
        public string ConfigMailReceiver { get { return "MailReceiver"; } }

        public string ConfigProxy { get { return "UseWebProxy"; } }
        public string ConfigProxyServer { get { return "WebProxy"; } }
        public string ConfigProxyPort { get { return "WebProxyPort"; } }

        public string ConfigMainURL { get { return "MainURL"; } }

        public string ConfigQueryNeueSeite { get { return "QueryNeueSeite"; } }
        public string ConfigQuerySiteNotFound { get { return "QuerySiteNotFound"; } }
        public string ConfigQueryDiffrentSites { get { return "QueryDiffrentSites"; } }
    }
}
