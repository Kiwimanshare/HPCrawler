using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCrawler
{
    partial class DBDataProvider : iCrawlerDataProvider
    {
        private const string _tableselect = "{0} = '{1}'";
        private const string _Query = "SELECT * FROM {0}";

        private string _tableName;
        private string _columnURLHashPK;
        private string _columnURL;
        private string _columnLastScan;
        private string _columnSiteHashCurrent;
        private string _columnSiteHashNew;

        private string _QueryNewSites;
        private string _QuerySiteNotFound;
        private string _QueryDiffrentSites;

        public iConfiguration ConfigClass
        {
            get;
            set;
        }

        public Dictionary<string, iDataStructure> ProvidedData
        {
            get;
            set;
        }

        public DBDataProvider(iConfiguration config)
        {
            this.ConfigClass = config;
            InitializeProveiderByConfig();
        }

        public void InitializeProveiderByConfig(iConfiguration config)
        {
            this.ConfigClass = config;
            InitializeProveiderByConfig();
        }

        public void InitializeProveiderByConfig()
        {
            if (this.ConfigClass != null)
            {
                SetNamesFromConfig();

                SetConnectionString();
                CreateDataObject();
            }
        }

        private void SetNamesFromConfig()
        {
            UserID = ConfigClass.GetConfigKey(ConfigClass.ConfigDBUser);
            Server = ConfigClass.GetConfigKey(ConfigClass.ConfigDBServer);
            Database = ConfigClass.GetConfigKey(ConfigClass.ConfigDBDatabase);
            UserPW = ConfigClass.GetConfigKeyEncrypted(ConfigClass.ConfigDBPW);

            _tableName = ConfigClass.GetConfigKey(ConfigClass.ConfigDBTableName);
            _columnURLHashPK = ConfigClass.GetConfigKey(ConfigClass.ConfigDBTableURLHashPK);
            _columnURL = ConfigClass.GetConfigKey(ConfigClass.ConfigDBTableNameURL);
            _columnLastScan = ConfigClass.GetConfigKey(ConfigClass.ConfigDBTableNameLastScan);
            _columnSiteHashCurrent = ConfigClass.GetConfigKey(ConfigClass.ConfigDBTableNameHashCurrent);
            _columnSiteHashNew = ConfigClass.GetConfigKey(ConfigClass.ConfigDBTableNameHashNew);

            _QueryDiffrentSites = ConfigClass.GetConfigKey(ConfigClass.ConfigQueryDiffrentSites);
            _QueryNewSites = ConfigClass.GetConfigKey(ConfigClass.ConfigQueryNeueSeite);
            _QuerySiteNotFound = ConfigClass.GetConfigKey(ConfigClass.ConfigQuerySiteNotFound);
        }

        public bool GetLatestData()
        {
            if (FillData(GenerateQuery(), _tableName))
            {
                return FillDictionaryFromDB(_tableName);
            }

            return false;
        }

        private string GenerateQuery()
        {
            return string.Format(_Query, _tableName);
        }

        public bool SetNewestData()
        {
            return FillDBFromDirectory();
        }
    }
}
