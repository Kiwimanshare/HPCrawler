using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace HPCrawler
{
    partial class DBDataProvider : iDataBase
    {
        public DataSet Data;
        public List<string> TabellenNamen;

        private SqlConnection DBZugriff;
        private string ConectionSting;
        private Dictionary<string, SqlDataAdapter> AdapterDict;

        private string UserID;
        private string UserPW;
        private string Database;
        private string Server;

        public DBDataProvider(string ConectionString)
        {
            this.ConectionSting = ConectionString;
            CreateDataObject();
        }

        public DBDataProvider(string sqluser, string sqlpassword, string sqlhost, string sqldatabase)
        {
            SetConnectionString(sqluser, sqlpassword, sqlhost, sqldatabase);
            CreateDataObject();
        }

        private void SetConnectionString()
        {
            SetConnectionString(UserID, UserPW, Server, Database);
        }

        private void SetConnectionString(string ConectionString)
        {
            this.ConectionSting = ConectionString;
        }

        private void SetConnectionString(string UserID, string Password, string Server, string Datenbank)
        {
            this.ConectionSting = string.Format("user id = {0}; password = {1}; server = {2}; Database = {3};", UserID, Password, Server, Datenbank);
        }

        private string GetConnectionString()
        {
            return ConectionSting;
        }

        private SqlConnection GetConnection()
        {
            DBZugriff = new SqlConnection(ConectionSting);
            return this.DBZugriff;
        }

        private void CreateDataObject()
        {
            DBZugriff = new SqlConnection(ConectionSting);
            Data = new DataSet();
            TabellenNamen = new List<string>();
            AdapterDict = new Dictionary<string, SqlDataAdapter>();
        }

        public bool FillData(string Query, string Tabelle)
        {
            try
            {
                if (!AdapterDict.ContainsKey(Tabelle))
                {
                    AdapterDict.Add(Tabelle, new SqlDataAdapter(Query, DBZugriff));
                }
                else
                {
                    AdapterDict[Tabelle] = new SqlDataAdapter(Query, DBZugriff);
                }

                AdapterDict[Tabelle].Fill(Data, Tabelle);
                TabellenNamen.Add(Tabelle);
                return true;
            }
            catch (Exception ex)
            {
                ConfigClass._Log.Add(string.Format(ConfigClass._LogFormat, ConfigClass._LogError, ex.Message));
                return false;
            }
        }

        public DataTable FillDataTable(string Query, string Tabelle)
        {
            DataTable DT = new DataTable(Tabelle);

            try
            {
                if (!AdapterDict.ContainsKey(Tabelle))
                {
                    AdapterDict.Add(Tabelle, new SqlDataAdapter(Query, DBZugriff));
                }
                else
                {
                    AdapterDict[Tabelle] = new SqlDataAdapter(Query, DBZugriff);
                }

                AdapterDict[Tabelle].Fill(DT);
                TabellenNamen.Add(Tabelle);
            }
            catch (Exception ex)
            {
                ConfigClass._Log.Add(string.Format(ConfigClass._LogFormat, ConfigClass._LogError, ex.Message));
                return null;
            }

            return DT;
        }

        public bool SafeChanges(string TableName)
        {
            SqlCommandBuilder SCB = new SqlCommandBuilder(AdapterDict[TableName]);

            try
            {
                AdapterDict[TableName].AcceptChangesDuringUpdate = true;

                AdapterDict[TableName].UpdateCommand = SCB.GetUpdateCommand(true);
                AdapterDict[TableName].InsertCommand = SCB.GetInsertCommand(true);
                AdapterDict[TableName].InsertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
                AdapterDict[TableName].DeleteCommand = SCB.GetDeleteCommand(true);

                AdapterDict[TableName].Update(Data, TableName);

                return true;
            }
            catch (Exception ex)
            {
                ConfigClass._Log.Add(string.Format(ConfigClass._LogFormat, ConfigClass._LogError, ex.Message));
                return false;
            }
        }

        private bool FillDictionaryFromDB(string tableName)
        {
            ProvidedData = new Dictionary<string, iDataStructure>();

            try
            {
                foreach (DataRow dr in Data.Tables[tableName].Rows)
                {
                    ProvidedData.Add((string)dr[_columnURLHashPK],
                        new DataStructure(
                            dr[_columnURLHashPK],
                            dr[_columnURL],
                            dr[_columnLastScan],
                            dr[_columnSiteHashCurrent],
                            dr[_columnSiteHashNew]));
                }

                return true;
            }
            catch (Exception ex)
            {
                ConfigClass._Log.Add(string.Format(ConfigClass._LogFormat, ConfigClass._LogError, ex.Message));
                return false;
            }
        }

        private bool FillDBFromDirectory()
        {
            DataRow tempRow;
            DataRow[] datarows;

            foreach (KeyValuePair<string, iDataStructure> valuePair in ProvidedData)
            {
                datarows = Data.Tables[_tableName].Select(string.Format(_tableselect,
                    _columnURLHashPK, valuePair.Key));

                if (datarows.Length == 1) //select PrimaryKey could only be 1 or 0
                {
                    datarows[0][_columnSiteHashNew] = valuePair.Value.SiteHash;
                    datarows[0][_columnSiteHashCurrent] = valuePair.Value.SiteHashOld;
                    datarows[0][_columnLastScan] = valuePair.Value.ScanTime; // DateTime.Now.Date;
                }
                else
                {
                    tempRow = Data.Tables[_tableName].NewRow();

                    tempRow[_columnURLHashPK] = valuePair.Key;
                    tempRow[_columnURL] = valuePair.Value.URL;
                    tempRow[_columnLastScan] = valuePair.Value.ScanTime;
                    tempRow[_columnSiteHashNew] = valuePair.Value.SiteHash;
                    tempRow[_columnSiteHashCurrent] = null;

                    Data.Tables[_tableName].Rows.Add(tempRow);
                }
            }

            return SafeChanges(_tableName);
        }

        public Dictionary<string, iDataStructure> GetDiffrences()
        {
            string table = "DiffrentSites";

            FillData(_QueryDiffrentSites, table);
            FillDictionaryFromDB(table);
            return ProvidedData;
        }

        public Dictionary<string, iDataStructure> GetNewSites()
        {
            string table = "NewSites";

            FillData(_QueryNewSites, table);
            FillDictionaryFromDB(table);
            return ProvidedData;
        }

        public Dictionary<string, iDataStructure> GetSiteNotFound()
        {
            string table = "NotFoundSites";

            FillData(_QuerySiteNotFound, table);
            FillDictionaryFromDB(table);
            return ProvidedData;
        }
    }
}
