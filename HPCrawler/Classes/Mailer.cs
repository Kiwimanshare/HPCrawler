using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace HPCrawler
{
    class Mailer : iMailer
    {
        public string SMTP 
        { 
            get; 
            set; 
        }
        public string Port
        {
            get;
            set;
        }
        public string SenderMail
        {
            get;
            set;
        }
        public string ReceiverMail
        {
            get;
            set;
        }
        public string Subject 
        { 
            get; 
            set; 
        }

        private string _HTMLbodyText = string.Empty;

        public Mailer(string smtp, string port, string sendermail, string receivermail)
        {
            SMTP = smtp;
            Port = port;
            SenderMail = sendermail;
            ReceiverMail = receivermail;
        }

        public void WriteLineToHTMLBody(string text)
        {
            _HTMLbodyText += text + "<br>";
        }

        public bool SendMail()
        {
            try
            {
                if (!int.TryParse(Port, out int port))
                {
                    port = 25; //defaultport
                }

                SmtpClient client = new SmtpClient(SMTP, port);

                MailMessage mail = new MailMessage(SenderMail, ReceiverMail);
                mail.IsBodyHtml = true;

                mail.Subject = Subject;

                mail.Body = _HTMLbodyText;

                client.Send(mail);

                client.Dispose();
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
