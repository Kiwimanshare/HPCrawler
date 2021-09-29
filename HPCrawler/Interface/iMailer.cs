using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCrawler
{

    interface iMailer
    {
        string SMTP { get; set; }
        string Port { get; set; }
        string SenderMail { get; set; }
        string ReceiverMail { get; set; }

        string Subject { get; set; }

        void WriteLineToHTMLBody(string text);
        bool SendMail();

    }
}
