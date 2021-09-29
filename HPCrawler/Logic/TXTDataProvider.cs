using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HPCrawler
{
    class TXTDataProvider : iCrawlerDataProvider
    {
        private string _txtFilePath;

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

        public TXTDataProvider(iConfiguration config)
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
            }
        }

        private void SetNamesFromConfig()
        {
            this._txtFilePath = ConfigClass.GetConfigKey(ConfigClass.ConfigTxtPath);
        }

        private bool CheckIfPathIsValid()
        {
            FileInfo path = null;

            try
            {
                path = new FileInfo(_txtFilePath);

                if (!path.Exists)
                {
                    FileStream fs = path.Create();
                    fs.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                //fehler pfad ungültig!!
                ConfigClass._Log.Add(string.Format(ConfigClass._LogFormat, ConfigClass._LogError, "Ungültiger Pfad"));
                ConfigClass._Log.Add(string.Format(ConfigClass._LogFormat, ConfigClass._LogError, ex.Message));
                return false;
            }
        }

        public bool GetLatestData()
        {
            ProvidedData = new Dictionary<string, iDataStructure>();

            if (CheckIfPathIsValid())
            {
                StreamReader sr = new StreamReader(_txtFilePath);

                while (!sr.EndOfStream)
                {
                    FillDictionaryFromTXT(sr.ReadLine());
                }

                sr.Close();

                return true;
            }
            else
            {
                return false;
            }
        }

        private bool FillDictionaryFromTXT(string row)
        {
            try
            {
                string[] data = row.Split(';');

                ProvidedData.Add((string)data[0],
                    new DataStructure(
                        data[0],
                        data[1],
                        DateTime.Parse(data[2]),
                        null,
                        data[3]));
                return true;
            }
            catch (Exception ex)
            {
                ConfigClass._Log.Add(string.Format(ConfigClass._LogFormat, ConfigClass._LogError, "Fehler beim füllen des Data-Directories"));
                ConfigClass._Log.Add(string.Format(ConfigClass._LogFormat, ConfigClass._LogError, ex.Message));
                return false;
            }
        }

        public bool SetNewestData()
        {
            StringBuilder sb = new StringBuilder();
            StreamWriter sw = new StreamWriter(_txtFilePath, false);

            foreach (KeyValuePair<string, iDataStructure> valuePair in ProvidedData)
            {
                sb.Append(valuePair.Key);
                sb.Append(";");
                sb.Append(valuePair.Value.URL);
                sb.Append(";");
                sb.Append(valuePair.Value.ScanTime.ToString().Split(' ')[0]);
                sb.Append(";");
                sb.Append(valuePair.Value.SiteHash);
                sb.Append(";");
                sb.Append(valuePair.Value.SiteHashOld);

                sw.WriteLine(sb.ToString());

                sb.Clear();
            }

            sw.Close();

            return true;
        }

        public Dictionary<string, iDataStructure> GetDiffrences()
        {
            Dictionary<string, iDataStructure> diff = new Dictionary<string, iDataStructure>();

            foreach (KeyValuePair<string, iDataStructure> kvp in ProvidedData)
            {
                if (kvp.Value.SiteHashOld != null && kvp.Value.SiteHash != ConfigClass._SiteNotFound && kvp.Value.SiteHash != kvp.Value.SiteHashOld)
                {
                    diff.Add(kvp.Key, kvp.Value);
                }
            }

            return diff;
        }

        public Dictionary<string, iDataStructure> GetNewSites()
        {
            Dictionary<string, iDataStructure> newSites = new Dictionary<string, iDataStructure>();

            foreach (KeyValuePair<string, iDataStructure> kvp in ProvidedData)
            {
                if (kvp.Value.SiteHashOld == null && kvp.Value.SiteHash != string.Empty)
                {
                    newSites.Add(kvp.Key, kvp.Value);
                }
            }

            return newSites;
        }

        public Dictionary<string, iDataStructure> GetSiteNotFound()
        {
            Dictionary<string, iDataStructure> deletedSites = new Dictionary<string, iDataStructure>();

            foreach (KeyValuePair<string, iDataStructure> kvp in ProvidedData)
            {
                if (kvp.Value.SiteHash == ConfigClass._SiteNotFound)
                {
                    deletedSites.Add(kvp.Key, kvp.Value);
                }
            }

            return deletedSites;
        }
    }
}
