using System.Data.SqlClient;
using System.Data;
using System;

namespace WebApplication6.Models
{
    public class ConnectionClass
    {
        public ConnectionClass()
        {
            String none = "";
            string server = "65.108.97.18";
            string sqldb = "jnzngjes_esp_data";
            string uid = "esp_board";
            string password = "AhsanTipu-264B";
            string connString;
            connString = "SERVER=" + "65.108.97.18" + " ;" + "DATABASE=" + "jnzngjes_esp_data" + ";" + "UID=" + "esp_board" + ";" + "PASSWORD=" + "AhsanTipu-264B" + ";MultipleActiveResultSets=True";
            connString = "SERVER=65.108.97.18; DATABASE=jnzngjes_esp_data; UID=esp_board; PASSWORD=AhsanTipu-264B; MultipleActiveResultSets=True";

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
