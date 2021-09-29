using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCrawler
{
    public enum Scanstatus
    {
        neu,
        scanned,
        notfound,
        current
    }

    class DataStructure : iDataStructure
    {
        public string URLHash
        {
            get;
            set;
        }

        public string URL
        {
            get;
            set;
        }

        public DateTime ScanTime
        {
            get;
            set;
        }

        public string SiteHash
        {
            get;
            set;
        }

        public string SiteHashOld
        {
            get;
            set;
        }

        public Scanstatus Status
        {
            get;
            set;
        }

        public DataStructure()
        {

        }

        public DataStructure(Scanstatus status, string url, string urlhash)
        {
            Status = status;
            URL = url;
            URLHash = urlhash;
        }

        public DataStructure(object urlhash, object url, object scantime, object sitehash, object sitehashold)
        {
            this.URLHash = (string)urlhash;
            this.URL = (string)url;
            this.ScanTime = ((DateTime)scantime).Date;
            this.SiteHash = sitehash == DBNull.Value ? string.Empty : (string)sitehash;
            this.SiteHashOld = sitehashold == DBNull.Value ? string.Empty : (string)sitehashold;

            this.Status = Scanstatus.current;
        }
    }
}
