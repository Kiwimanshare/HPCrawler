using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCrawler
{
    partial class Crawler
    {
        public bool GetData()
        {
            if (Config.DataType == Config.ConfigDataSaveTypeDB)
            {
                return GetFilesFromDB();
            }
            else if (Config.DataType == Config.ConfigDataSaveTypeTXT)
            {
                return GetFilesFromTXT();
            }

            return false;
        }

        private bool GetFilesFromDB()
        {
            CrawlerData = new DBDataProvider(Config);

            return this.CrawlerData.GetLatestData();
        }

        private bool GetFilesFromTXT()
        {
            CrawlerData = new TXTDataProvider(Config);

            return this.CrawlerData.GetLatestData();
        }
    }
}
