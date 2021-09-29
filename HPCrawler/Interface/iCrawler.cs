using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCrawler
{
    interface iCrawler
    {
        iConfiguration Config { get; set; }
        iMailer Mailer { get; set; }
        iCrawlerDataProvider CrawlerData { get; set; }
        bool RunCheck();
        bool SaveFiles();
    }
}
