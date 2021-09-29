using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCrawler
{
    interface iDataStructure
    {
        string URLHash
        {
            get;
            set;
        }

        string URL
        {
            get;
            set;
        }

        DateTime ScanTime
        {
            get;
            set;
        }

        string SiteHash
        {
            get;
            set;
        }

        string SiteHashOld
        {
            get;
            set;
        }

        Scanstatus Status
        {
            get;
            set;
        }
    }
}
