using System;
using System.IO;
using System.Collections.Generic;

namespace HPCrawler
{
    class Program
    {
        static iConfiguration _Config = new Configuration();
        static Crawler crawl;

        static void Main(string[] args)
        {
            try
            {
                CheckParameter(args);
            }
            catch (Exception ex)
            {
                _Config._Log.Add(string.Format(_Config._LogFormat, _Config._LogError, ex.Message));
            }
            finally
            {
                Logging();
            }
        }

        static void CheckParameter(string[] args)
        {
            if (args.Length == 0)
            {
                ShowParameters();
            }
            else
            {
                WorkParameters(args);
            }
        }

        static void ShowParameters()
        {
            Console.WriteLine(GlobalConstants._ParaInfoParameter);
            Console.WriteLine(GlobalConstants._ParaInfoSetup);
            Console.WriteLine(GlobalConstants._ParaInfoRun);
            Console.WriteLine(GlobalConstants._ParaInfoNotify);
            CheckParameter(Console.ReadLine().Split(' '));
        }

        static void WorkParameters(string[] args)
        {
            bool setup = false, run = false, notify = false, parametrKnown = true;

            foreach(string p in args)
            {
                if(p.ToUpper() == GlobalConstants._ParameterSetup)
                {
                    setup = true;
                }
                else if(p.ToUpper() == GlobalConstants._ParameterRun)
                {
                    run = true;
                }
                else if (p.ToUpper() == GlobalConstants._ParameterSendNotification)
                {
                    notify = true;
                }
                else
                {
                    parametrKnown = false;
                    break;
                }
            }

            if (!parametrKnown)
            {
                ShowParameters();
            }
            else
            {
                if (setup)
                {
                    SetUpConfig();
                }

                if (run)
                {
                    CreateCrawler();
                    RunHPCrawler();
                }

                if (notify && _Config.IsMailer)
                {
                    MailNotificatoin();
                }
            }
        }

        //set up config
        static void SetUpConfig()
        {
            _Config._Log.Add(string.Format(_Config._LogFormat, _Config._LogInfo, "Setup Configuration"));

            _Config.SetUpConfig();
            ShowParameters();
        }

        static void CreateCrawler()
        {
            _Config.ReadConfig();
            crawl = new Crawler(_Config);
        }

        //start site checking
        static void RunHPCrawler()
        {
            _Config._Log.Add(string.Format(_Config._LogFormat, _Config._LogInfo, "Run Crawler"));

            crawl.RunCheck();
            crawl.SaveFiles();
        }

        static void MailNotificatoin()
        {
            if(crawl == null)
            {
                CreateCrawler();
            }

            FillMailBody(GlobalConstants._MailTextDiffrentSites, crawl.CrawlerData.GetDiffrences());
            FillMailBody(GlobalConstants._MailTextNewSites, crawl.CrawlerData.GetNewSites());
            FillMailBody(GlobalConstants._MailTextNotFoundSites, crawl.CrawlerData.GetSiteNotFound());

            crawl.Mailer.Subject = "Homepage Scan für geänderte Seiten";

            if(!crawl.Mailer.SendMail())
            {
                _Config._Log.Add(string.Format(_Config._LogFormat, _Config._LogError, "Fehler beim senden der E-Mail"));
            }
        }

        static void FillMailBody(string infoText, Dictionary<string, iDataStructure> data)
        {
            crawl.Mailer.WriteLineToHTMLBody(infoText);

            foreach (KeyValuePair<string, iDataStructure> kvp in data)
            {
                crawl.Mailer.WriteLineToHTMLBody(kvp.Value.URL);
            }

            crawl.Mailer.WriteLineToHTMLBody(string.Empty);
        }

        static void Logging()
        {
            if (_Config.IsLogger)
            {
                string fileDest = _Config.GetConfigKey(_Config.ConfigLogPath);
                FileInfo fi = new FileInfo(fileDest);

                if (!int.TryParse(_Config.GetConfigKey(_Config.ConfigLogMaxLen), out int maxlen))
                {
                    _Config._Log.Add(string.Format(_Config._LogFormat, _Config._LogError, "Log Max Value is not a Number!"));
                    maxlen = int.MaxValue;
                }

                if (!Directory.Exists(fi.DirectoryName))
                {
                    Directory.CreateDirectory(fi.DirectoryName);
                }
                else if (File.Exists(fileDest))
                {
                    if (fi.Length >= maxlen)
                    {
                        string newName = string.Format(_Config._LogOldFormat, fi.DirectoryName, DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, fi.Name);

                        if (File.Exists(newName))
                        {
                            File.Delete(newName);
                        }

                        fi.MoveTo(newName);
                    }
                }

                _Config._Log.Add(string.Format(_Config._LogFormat, _Config._LogInfo, "-------------------------------------------------------------- New Log --------------------------------------------------------------"));
                File.AppendAllLines(fileDest, _Config._Log);
            }
        }
    }
}
