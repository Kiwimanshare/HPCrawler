using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCrawler
{
    interface iCrawlerDataProvider
    {
        iConfiguration ConfigClass
        {
            get;
            set;
        }

        Dictionary<string, iDataStructure> ProvidedData
        {
            get;
            set;
        }

        void InitializeProveiderByConfig();

        void InitializeProveiderByConfig(iConfiguration config);

        bool GetLatestData();

        bool SetNewestData();

        Dictionary<string, iDataStructure> GetDiffrences();

        Dictionary<string, iDataStructure> GetNewSites();

        Dictionary<string, iDataStructure> GetSiteNotFound();
    }
}
