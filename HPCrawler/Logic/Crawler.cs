using System;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

using System.Collections.Generic;

namespace HPCrawler
{
    partial class Crawler : iCrawler
    {
        private string _mainSite;
        private string _URLBuilder = "{0}{1}";

        private WebProxy _webProxy;

        public iConfiguration Config
        {
            get;
            set;
        }
        public iMailer Mailer
        {
            get;
            set;
        }

        public iCrawlerDataProvider CrawlerData
        {
            get;
            set;
        }

        public Crawler(iConfiguration config)
        {
            Config = config;
            SetUpCrawlerByConfig();
        }

        private void SetUpCrawlerByConfig()
        {
            _mainSite = Config.GetConfigKey(Config.ConfigMainURL).TrimEnd('/');

            SetUpProxy();
            SetUpMailer();
            GetData();
        }

        private void SetUpProxy()
        {
            if (Config.IsProxy)
            {
                string proxyadress = string.Format("{0}:{1}",
                    Config.GetConfigKey(Config.ConfigProxyServer),
                    Config.GetConfigKey(Config.ConfigProxyPort));

                _webProxy = new WebProxy(proxyadress);
                _webProxy.UseDefaultCredentials = true;
                _webProxy.Credentials = CredentialCache.DefaultCredentials;
            }
            else
            {
                _webProxy = null;
            }
        }

        private void SetUpMailer()
        {
            if (Config.IsMailer)
            {
                Mailer = new Mailer(Config.GetConfigKey(Config.ConfigMailSMTP), Config.GetConfigKey(Config.ConfigMailPort), 
                    Config.GetConfigKey(Config.ConfigMailSender), Config.GetConfigKey(Config.ConfigMailReceiver));
            }
        }

        public bool RunCheck()
        {
            if (CrawlerData.ProvidedData != null)
            {
                ScanURL(FixAndCheckURL(_mainSite));
                return true;
            }

            return false;
        }

        public bool SaveFiles()
        {
            CrawlerData.SetNewestData();

            return false;
        }

        private string FixAndCheckURL(string URL)
        {
            string lowerURL;

            Config._Log.Add(string.Format(Config._LogFormat, Config._LogInfo, "Checking URL:"));
            Config._Log.Add(string.Format(Config._LogFormat, Config._LogInfo, URL));

            URL = URL.Replace("href=\"", "").Replace("href='", "").Replace("\"", "").Replace("'", "").TrimEnd('/');

            if (URL.StartsWith(_mainSite))
            {
                return URL;
            }
            else if (URL.StartsWith("/"))
            {
                URL = string.Format(_URLBuilder, _mainSite, URL);
            }
            else
            {
                lowerURL = URL.ToLower();

                if (lowerURL.StartsWith("http") || lowerURL.StartsWith("www") || lowerURL.StartsWith("#") || lowerURL.StartsWith("tel") 
                    || lowerURL.StartsWith("mailto") || lowerURL.StartsWith("fax") || URL == string.Empty)
                {
                    return null;
                }

                URL = "/" + URL;
                URL = string.Format(_URLBuilder, _mainSite, URL);
            }

            if (Uri.TryCreate(URL, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
                return URL;
            }
            else
            {
                Config._Log.Add(string.Format(Config._LogFormat, Config._LogInfo, "Ungültige URL:"));
                Config._Log.Add(string.Format(Config._LogFormat, Config._LogInfo, URL));
                return null;
            }
        }

        private void Getlinks(string site)
        {
            string pattern = @"href=(""|')(.+?)(""|')";

            Regex r = new Regex(pattern);
            MatchCollection mc1 = r.Matches(site);

            foreach(Match link in mc1)
            {
                ScanURL(FixAndCheckURL(link.Value));
            }
        }

        private string ReplaceDynamicString(string input)
        {
            string patternFormulare = "input autocomplete=\"off\" data-drupal-selector=\"[\\w\\-]+\" type=\"hidden\" name=\"form_build_id\" value=\"[\\w\\-]+\"";
            string patternTopaxA = "meta name=\"csrf-token\" content=\"[\\/\\=\\-\\+\\w]+\"";
            string patternTopaxB = "input type=\"hidden\" name=\"authenticity_token\" value=\"[\\/\\=\\-\\+\\w]+\"";

            input = Regex.Replace(input, patternFormulare, string.Empty);
            input = Regex.Replace(input, patternTopaxA, string.Empty);
            return Regex.Replace(input, patternTopaxB, string.Empty);
        }

        private string ScanCheck(string URL)
        {
            if (URL != null)
            {
                string URLHash = ComputeSha256Hash(URL);

                if (!CrawlerData.ProvidedData.ContainsKey(URLHash))
                {
                    CrawlerData.ProvidedData.Add(URLHash, new DataStructure(Scanstatus.neu, URL, URLHash));
                    CrawlerData.ProvidedData[URLHash].Status = Scanstatus.neu;
                    return URLHash;
                }
                else if (CrawlerData.ProvidedData[URLHash].Status != Scanstatus.scanned)
                {
                    CrawlerData.ProvidedData[URLHash].Status = Scanstatus.current;
                    return URLHash;
                }
            }
            return null;
        }

        private void ScanURL(string URL)
        {
            string URLHash = ScanCheck(URL);

            if (URLHash != null)
            {
                Config._Log.Add(string.Format(Config._LogFormat, Config._LogInfo, "Scan URL:"));
                Config._Log.Add(string.Format(Config._LogFormat, Config._LogInfo, URL));

                using (WebClient client = new WebClient())
                {
                    client.Proxy = _webProxy;

                    try
                    {
                        string downloadString = ReplaceDynamicString(client.DownloadString(URL));

                        CrawlerData.ProvidedData[URLHash].SiteHash = ComputeSha256Hash(downloadString);
                        CrawlerData.ProvidedData[URLHash].Status = Scanstatus.scanned;
                        CrawlerData.ProvidedData[URLHash].ScanTime = DateTime.Now;

                        Getlinks(downloadString);
                    }
                    catch (Exception ex)
                    {
                        CrawlerData.ProvidedData[URLHash].SiteHash = Config._SiteNotFound;
                        CrawlerData.ProvidedData[URLHash].Status = Scanstatus.scanned;
                        CrawlerData.ProvidedData[URLHash].ScanTime = DateTime.Now;
                    }
                }
            }
        }

        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
