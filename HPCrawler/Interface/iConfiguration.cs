using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCrawler
{
    interface iConfiguration
    {
        void SetUpConfig();
        string GetConfigKeyEncrypted(string key);
        void UpdateConfigKeyEncrypted(string key, string value);
        string GetConfigKey(string key);
        void UpdateConfigKey(string key, string value);
        void ReadConfig();
        List<string> _Log { get; }
        string _LogFormat { get; }
        string _LogError { get; }
        string _LogInfo { get; }
        string _LogOldFormat { get; }

        string _SiteNotFound { get; }

        string DataType { get; set; }
        bool IsMailer { get; set; }
        bool IsLogger { get; set;  }
        bool IsProxy { get; set; }

        string ConfigTrue { get; }
        string ConfigFalse { get; }

        string ConfigDataSaveType { get; }
        string ConfigDataSaveTypeDB { get; }
        string ConfigDataSaveTypeTXT { get; }

        string ConfigDBServer { get; }
        string ConfigDBDatabase { get; }
        string ConfigDBUser { get; }
        string ConfigDBPW { get; }
        string ConfigDBTableName { get; }
        string ConfigDBTableURLHashPK { get; }
        string ConfigDBTableNameURL { get; }
        string ConfigDBTableNameLastScan { get; }
        string ConfigDBTableNameHashCurrent{ get; }
        string ConfigDBTableNameHashNew { get; }

        string ConfigTxtPath { get; }

        string ConfigLogLogging { get; }
        string ConfigLogPath { get; }
        string ConfigLogMaxLen { get; }

        string ConfigMailNotification { get; }
        string ConfigMailSMTP { get; }
        string ConfigMailPort { get; }
        string ConfigMailSender { get; }
        string ConfigMailReceiver { get; }

        string ConfigProxy { get; }
        string ConfigProxyServer { get; }
        string ConfigProxyPort { get; }
        string ConfigMainURL { get; }

        string ConfigQueryNeueSeite { get; }
        string ConfigQuerySiteNotFound { get; }
        string ConfigQueryDiffrentSites { get; }
    }
}
