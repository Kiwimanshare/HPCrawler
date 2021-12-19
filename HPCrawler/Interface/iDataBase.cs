using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace HPCrawler
{
    interface iDataBase
    {
        //string UserID { get; set;  }
        //string UserPW { get; set; }
        //string Database { get; set; }
        //string Server { get; set; }


        //void SetConnectionString();
        //void SetConnectionString(string ConectionString);
        //void SetConnectionString(string UserID, string Password, string Server, string Datenbank);
        //string GetConnectionString();
        //SqlConnection GetConnection();
        //void CreateDataObject();
        bool FillData(string Query, string Tabelle);
        DataTable FillDataTable(string Query, string Tabelle);
        bool SafeChanges(string TableName);
    }
}
