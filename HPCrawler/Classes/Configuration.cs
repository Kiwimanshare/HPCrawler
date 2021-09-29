using System;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;

namespace HPCrawler
{
    partial class Configuration : iConfiguration
    {
        private const string _NonPublicCryptoKey = "HPC";

        public void SetUpConfig()
        {
            Console.WriteLine(_ConfigInfoText);
            this.ConfigTool();
        }

        public string GetConfigKeyEncrypted(string key)
        {
            return Decrypt(GetConfigKey(key), _NonPublicCryptoKey);
        }

        public void UpdateConfigKeyEncrypted(string key, string value)
        {
            if (value.ToUpper() != "X")
            {
                UpdateConfigKey(key, Encrypt(value, _NonPublicCryptoKey));
            }
        }

        public string GetConfigKey(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings[key] ?? _NotFound;
            }
            catch (ConfigurationErrorsException ex)
            {
                _Log.Add(string.Format(_LogFormat, _LogError, _ErrorReadingAppSettings));
                _Log.Add(string.Format(_LogFormat, _LogError, ex.Message));
                return null;
            }
        }

        public void UpdateConfigKey(string key, string value)
        {
            if (value.ToUpper() != "X")
            {
                try
                {
                    var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    var settings = configFile.AppSettings.Settings;

                    if (settings[key] == null)
                    {
                        settings.Add(key, value);
                    }
                    else
                    {
                        settings[key].Value = value;
                    }

                    configFile.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                }
                catch (ConfigurationErrorsException ex)
                {
                    _Log.Add(string.Format(_LogFormat, _LogError, _ErrorWritingAppSettings));
                    _Log.Add(string.Format(_LogFormat, _LogError, ex.Message));
                    Console.WriteLine(_ErrorWritingAppSettings);
                }
            }
        }

        private string Encrypt(string source, string key)
        {
            using (TripleDESCryptoServiceProvider tripleDESCryptoService = new TripleDESCryptoServiceProvider())
            {
                using (MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider())
                {
                    byte[] byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
                    tripleDESCryptoService.Key = byteHash;
                    tripleDESCryptoService.Mode = CipherMode.ECB;
                    byte[] data = Encoding.UTF8.GetBytes(source);
                    return Convert.ToBase64String(tripleDESCryptoService.CreateEncryptor().TransformFinalBlock(data, 0, data.Length));
                }
            }
        }

        private string Decrypt(string encrypt, string key)
        {
            using (TripleDESCryptoServiceProvider tripleDESCryptoService = new TripleDESCryptoServiceProvider())
            {
                using (MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider())
                {
                    try
                    {
                        byte[] byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
                        tripleDESCryptoService.Key = byteHash;
                        tripleDESCryptoService.Mode = CipherMode.ECB;
                        byte[] data = Convert.FromBase64String(encrypt);
                        return Encoding.UTF8.GetString(tripleDESCryptoService.CreateDecryptor().TransformFinalBlock(data, 0, data.Length));
                    }
                    catch (Exception ex)
                    {
                        _Log.Add(string.Format(_LogFormat, _LogError, _EncryptionError));
                        _Log.Add(string.Format(_LogFormat, _LogError, ex.Message));
                        return _EncryptionError;
                    }
                }
            }
        }
    }
}
