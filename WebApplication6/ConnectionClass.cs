using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
namespace WebApplication6
{
    public class ConnectionClass
    {
       
            public ConnectionClass()
            {

                string server = "bijlibachao.database.windows.net";
                string sqldb = "dbname_fahad";
                string uid = "bijlibachao_user";
                string password = "Password4me";
                string connString;
                connString = "SERVER=" + server + " ;" + "DATABASE=" + sqldb + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";MultipleActiveResultSets=True";
                Sqlconn = new SqlConnection(connString);
            }
            internal bool Open()
            {
                try
                {
                    if (IsOpen()) { check2 = true; }
                    else
                    {
                        Sqlconn.Open();
                        check2 = true;
                    }

                }
                catch (Exception d) { check2 = false; }
                return check2;
            }
            bool check = false;
            bool check2 = false;

            internal bool Close()
            {
                try
                {
                    if (IsOpen())
                    {
                        Sqlconn.Close();
                        check = true;
                    }

                }
                catch (Exception d) { check = false; }
                return check;
            }

            internal bool IsOpen()
            {
                if (Sqlconn != null && Sqlconn.State == ConnectionState.Closed)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }


            internal SqlConnection Sqlconn { get; set; }
        }
    
}