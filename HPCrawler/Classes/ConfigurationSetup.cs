using System;
using System.Collections.Generic;

namespace HPCrawler
{
    partial class Configuration
    {
        public Configuration()
        {
            _Log = new List<string>();
        }

        private void ConfigTool()
        {
            char inputKey;

            //DB or TXT Config
            inputKey = GetInputKey(_SetupDataType, _SetupDataTypeList);
            if (inputKey == '0')
            {
                this.DataType = ConfigDataSaveTypeDB;
                SetupDB();
            }
            else if (inputKey == '1')
            {
                this.DataType = ConfigDataSaveTypeTXT;
                SetupTXTFile();
            }

            //Mail Config
            inputKey = GetInputKey(_SetupMailType, _SetupMailTypeList);
            if (inputKey == '0')
            {
                IsMailer = true;
                SetupMailNotification();
            }
            else if (inputKey == '1')
            {
                IsMailer = false;
                UpdateConfigKey(ConfigMailNotification, ConfigFalse);
            }

            //Log Config
            inputKey = GetInputKey(_SetupLog, _SetupLogTypeList);
            if (inputKey == '0')
            {
                IsLogger = true;
                SetupLogging();
            }
            else if (inputKey == '1')
            {
                IsLogger = false;
                UpdateConfigKey(ConfigLogLogging, ConfigFalse);
            }

            //Proxy Config
            inputKey = GetInputKey(_SetupProxy, _SetupProxyTypeList);
            if (inputKey == '0')
            {
                IsProxy = true;
                SetupProxy();
            }
            else if (inputKey == '1')
            {
                IsProxy = false;
                UpdateConfigKey(ConfigProxy, ConfigFalse);
            }

            //Query Config
            inputKey = GetInputKey(_SetupQueries, _SetupQueryList);
            if (inputKey == '0')
            {
                SetUpQueries();
            }

            //HP Config
            SetupWebsite();

            Console.WriteLine(_SetupDoneText);
        }

        private void SetupDB()
        {
            UpdateConfigKey(ConfigDataSaveType, ConfigDataSaveTypeDB);

            UpdateConfigKey(ConfigDBServer, GetInputString(_ConfigDBServerText));
            UpdateConfigKey(ConfigDBDatabase, GetInputString(_ConfigDBDatabaseText));
            UpdateConfigKey(ConfigDBUser, GetInputString(_ConfigDBUserText));
            UpdateConfigKeyEncrypted(ConfigDBPW, GetInputString(_ConfigDBPWText));

            UpdateConfigKey(ConfigDBTableName, GetInputString(_ConfigDBTabaleNameText));
            UpdateConfigKey(ConfigDBTableURLHashPK, GetInputString(_ConfigDBTableURLHashPKText));
            UpdateConfigKey(ConfigDBTableNameURL, GetInputString(_ConfigDBTableNameURLText));
            UpdateConfigKey(ConfigDBTableNameLastScan, GetInputString(_ConfigDBTableNameLastScanText));
            UpdateConfigKey(ConfigDBTableNameHashCurrent, GetInputString(_ConfigDBTableNameHashCurrentText));
            UpdateConfigKey(ConfigDBTableNameHashNew, GetInputString(_ConfigDBTableNameHashNewText));
        }

        private void SetupTXTFile()
        {
            UpdateConfigKey(ConfigDataSaveType, ConfigDataSaveTypeTXT);
            UpdateConfigKey(ConfigTxtPath, GetInputString(_ConfigTxtPathText));
        }

        private void SetupMailNotification()
        {
            UpdateConfigKey(ConfigMailNotification, ConfigTrue);

            UpdateConfigKey(ConfigMailSMTP, GetInputString(_ConfigMailSMTPText));
            UpdateConfigKey(ConfigMailPort, GetInputString(_ConfigMailPortText));
            UpdateConfigKey(ConfigMailSender, GetInputString(_ConfigMailSenderText));
            UpdateConfigKey(ConfigMailReceiver, GetInputString(_ConfigMailReceiverText));
        }

        private void SetupLogging()
        {
            UpdateConfigKey(ConfigLogLogging, ConfigTrue);
            UpdateConfigKey(ConfigLogPath, GetInputString(_ConfigLogDoText));
            UpdateConfigKey(ConfigLogMaxLen, GetInputString(_ConfigLogMaxLenText));
        }

        private void SetupProxy()
        {
            UpdateConfigKey(ConfigProxy, ConfigTrue);
            UpdateConfigKey(ConfigProxyServer, GetInputString(_ConfigProxyServerText));
            UpdateConfigKey(ConfigProxyPort, GetInputString(_ConfigProxyPortText));
        }

        private void SetUpQueries()
        {
            UpdateConfigKey(ConfigQueryNeueSeite, GetInputString(_ConfigQueryNewSiteText));
            UpdateConfigKey(ConfigQuerySiteNotFound, GetInputString(_ConfigQuerySiteNotFoundText));
            UpdateConfigKey(ConfigQueryDiffrentSites, GetInputString(_ConfigQueryDiffrentSitesText));
        }

        private void SetupWebsite()
        {
            UpdateConfigKey(ConfigMainURL, GetInputString(_ConfigMainURLText));
        }

        public void ReadConfig()
        {
            //DB or TXT Config
            if (GetConfigKey(ConfigDataSaveType) == ConfigDataSaveTypeDB)
            {
                this.DataType = ConfigDataSaveTypeDB;
            }
            else if (GetConfigKey(ConfigDataSaveType) == ConfigDataSaveTypeTXT)
            {
                this.DataType = ConfigDataSaveTypeTXT;
            }

            //Mail Config
            if (GetConfigKey(ConfigMailNotification) == ConfigTrue)
            {
                IsMailer = true;
            }
            else
            {
                IsMailer = false;
            }

            //Log Config
            if (GetConfigKey(ConfigLogLogging) == ConfigTrue)
            {
                IsLogger = true;
            }
            else
            {
                IsLogger = false;
            }

            //Proxy Config
            if (GetConfigKey(ConfigProxy) == ConfigTrue)
            {
                IsProxy = true;
            }
            else
            {
                IsProxy = false;
            }
        }

        private char GetInputKey(string question, List<char> acceptedKeys)
        {
            Console.WriteLine(question);

            int inputKey = Console.Read();
            Console.ReadLine();

            if (acceptedKeys.Contains((char)inputKey))
            {
                return (char)inputKey;
            }
            else
            {
                return GetInputKey(question, acceptedKeys);
            }
        }

        private string GetInputString(string question)
        {
            Console.WriteLine(question);

            string input = Console.ReadLine();

            if (input != string.Empty)
            {
                return input;
            }
            else
            {
                return GetInputString(question);
            }
        }
    }
}
