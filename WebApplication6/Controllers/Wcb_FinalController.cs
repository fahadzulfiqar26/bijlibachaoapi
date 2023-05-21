using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System;

namespace WebApplication6.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Wcb_FinalController : ControllerBase
    {
        ConnectionClass conn;
        DateTime datetimenow;
        string api_key_value = "tPmAT5Ab3j7F9";
        [HttpPost]
        public IActionResult Post([FromForm] dataDO formData)
        {
            try
            {
                // Process the form data
                conn = new ConnectionClass();
                if (api_key_value == formData.api_key)
                {
                  
                    conn.Open();
                    TimeZoneInfo timeZoneInfo;
                    timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");

                    datetimenow = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
                    datetimenow = datetimenow.AddMinutes(9);


                                        var value3 = double.Parse(formData.value3);
                    var value4 = double.Parse(formData.value4);
                    var value5 = double.Parse(formData.value5);

                    var value6 = double.Parse(formData.value6);
                    var value7 = double.Parse(formData.value7);
                    var value8 = double.Parse(formData.value8);
                    var value9 = double.Parse(formData.value9);

                    /////////////////////////////// NOSIE CANCELATION ///////////////////////////////
                    if (value7 > 500 || value7 < 0 || value6 > 500 || value6 < 0 || value8 > 500 || value8 < 0 || value3 > 700 || value4 > 700 || value5 > 700 || value6 > 200 || value7 > 200 || value8 > 200)
                    {
                        CloseConnections();
                        return Ok("Noise filter restrictions (Current<200 and Volatge<500 and non_negative) ");
                    }
                    /////////////////////////////// NOSIE CANCELATION ///////////////////////////////
                    if (value3 == 0 && value4 == 0 && value5 == 0 && value6 == 0 && value7 == 0 && value8 == 0 && value9 == 0)
                    {
                        CloseConnections();
                        return Ok("Noise filter restrictions (Current<200 and Volatge<500 and non_negative) ");
                    }






                    truncateTable(formData);


                    //normal seconds entry
                    addRecords(formData, false);

                    // 5 min table entry
                    if (FiveMintutes(formData))
                    {
                        addRecords(formData, true);
                    }



                    // relay status
                    var relay = readRelays(formData);
                    if (relay != null)
                    {
                        int d = updateRelayTable(formData);
                        CloseConnections();
                        return Ok("R" + relay.R1 + relay.R2 + ";");
                    }
                    //scheduling
                    var stringDO = readStrings(formData);
                    if (stringDO != null)
                    {
                        int t = updateStringTable(formData);
                        CloseConnections();
                        return Ok(stringDO.String_);
                    }
                    //time correction
                    try
                    {

                        var value1 = double.Parse(formData.value1);
                        DateTime dtcontroller = gettime(value1);
                        var dtnow = datetimenow;
                        TimeSpan ts = dtcontroller - dtnow;
                        if (ts.TotalMinutes > 5 || ts.TotalMinutes < -5)
                        {
                            var stringtime = dtnow.ToString("HHmm");
                            // if(double.Parse(stringtime)<)
                            CloseConnections();
                            return Ok("T1" + stringtime + ";");
                        }

                    }
                    catch { }


                    //truncate table


                    // normal return

                    return Ok("R33;");








                }
                else
                {
                    CloseConnections();
                    return Ok("Wrong Api Key");
                }

                // Funca();



            }
            catch (Exception ex)
            {

                CloseConnections();
                return Ok("Exception:" + ex.Message);
            }


           
            // Do something with the form data (e.g., save to a database)

            // Return a response

        }

        private bool FiveMintutes(dataDO formData)
        {
            bool check = false;
             try
            {

                if (!(conn.IsOpen()))
                {
                    conn.Open();
                }
                //$sql = "SELECT * FROM ATS_PM_Relays where Device_Id= '$sensor'";
                String q1 = " SELECT TOP 1 Date,Time FROM ATS_PM_D_" + formData.device_id + " order by Id desc";
                SqlCommand cmd1 = new SqlCommand(q1, conn.Sqlconn);
                SqlDataReader dr = cmd1.ExecuteReader();
                //  List<AlarmBO> list1 = new List<AlarmBO>();
                int count = 0;
                if (dr.Read())
                {

                    var date = (DateTime)dr[0];
                    var time = (string)dr[1];
                    var dt = DateTime.Parse(date.ToShortDateString() + " " + time);
                    var ts = datetimenow - dt;
                    if (ts.TotalMinutes > 5 || ts.TotalMinutes < -5)
                    {
                        check = true;
                    }
                }
                dr.Close();

            }
            catch { }
            return check;
        }

        private void truncateTable(dataDO formData)
        {
            try
            {

                if (!(conn.IsOpen()))
                {
                    conn.Open();
                }
                //$sql = "SELECT * FROM ATS_PM_Relays where Device_Id= '$sensor'";
                String q1 = " SELECT count(*) FROM ATS_PM_" + formData.device_id + " ";
                SqlCommand cmd1 = new SqlCommand(q1, conn.Sqlconn);
                SqlDataReader dr = cmd1.ExecuteReader();
                //  List<AlarmBO> list1 = new List<AlarmBO>();
                int count = 0;
                while (dr.Read())
                {

                    count = (int)dr[0];

                }
                dr.Close();
                if (count > 5000)
                {
                    String qu = "TRUNCATE  TABLE ATS_PM_" + formData.device_id;
                    var cmd = new SqlCommand(qu, conn.Sqlconn);
                    var recdr = cmd.ExecuteNonQuery();

                }
            }
            catch { }
        }

        private void addRecords(dataDO formData, bool check)
        {
            try
            {
                String tabletpye = "insert into ATS_PM_";
                if (check)
                    tabletpye = "insert into ATS_PM_D_";
                if (!conn.IsOpen())
                    conn.Open();
                var time = datetimenow.ToString("HH:mm:ss");
                var date = datetimenow.ToString("yyyy-MM-dd");
                string query = tabletpye + formData.device_id + " (sensor,Time,Date,value1,value2,value3,value4,value5,value6,value7,value8,value9,value10,value11,value12,value13,value14,value15) values ('" + formData.device_id + "','" + time + "','" + date + "','" + formData.value1 + "','" + formData.value2 + "','" + formData.value3 + "','" + formData.value4 + "','" + formData.value5 + "','" + formData.value6 + "','" + formData.value7 + "','" + formData.value8 + "','" + formData.value9 + "','" + formData.value10 + "','" + formData.value11 + "','" + formData.value12 + "','" + formData.value13 + "','" + formData.value14 + "','" + formData.value15 + "')";
                var cmd = new SqlCommand(query, conn.Sqlconn);
                int dr = cmd.ExecuteNonQuery();
            }
            catch { }
        }

        private void CloseConnections()
        {
            if (conn.IsOpen())
                conn.Close();
        }

        private DateTime gettime(double value1)
        {
            DateTime dt = datetimenow;
            var date = datetimenow.ToShortDateString();

            if (value1 < 10)
            {
                dt = DateTime.Parse(date + " 00:0" + ((int)value1));
            }
            else if (value1 < 60)
            {
                dt = DateTime.Parse(date + " 00:" + ((int)value1));
            }
            else if (value1 < 1000)
            {
                var str = ((int)value1) + "";
                dt = DateTime.Parse(date + " 0" + str.Substring(0, 1) + ":" + str.Substring(1, 2));

            }
            else
            {
                var str = ((int)value1) + "";
                dt = DateTime.Parse(date + " " + str.Substring(0, 2) + ":" + str.Substring(2, 2));

            }
            return dt;
        }

        private int updateStringTable(dataDO formData)
        {
            int dr = 0;
            try
            {
                if (!conn.IsOpen())
                    conn.Open();
                String qu = "Update ATS_PM_String set Bit = '0'  where Device_Id='" + formData.device_id + "'";
                var cmd = new SqlCommand(qu, conn.Sqlconn);
                dr = cmd.ExecuteNonQuery();

            }
            catch (Exception d) { }
            return dr;
            //Update ATS_PM_String set Bit = '0'  where Device_Id='$sensor'

        }

        private int updateRelayTable(dataDO formData)
        {
            int dr = 0;
            try
            {
                if (!conn.IsOpen())
                    conn.Open();
                String qu = "Update ATS_PM_Relays set R1 = '3' , R2='3'   where Device_Id='" + formData.device_id + "'";
                var cmd = new SqlCommand(qu, conn.Sqlconn);
                dr = cmd.ExecuteNonQuery();

            }
            catch (Exception d) { }
            return dr;
        }

        private StringDO readStrings(dataDO formData)
        {
            /*
             public int String_ { get; set; }
        public int Bit { get; set; }
        public int Time_Correction { get; set; }
        */
            StringDO relay = null;
            if (!(conn.IsOpen()))
            {
                conn.Open();
            }
            //$sql = "SELECT * FROM ATS_PM_Relays where Device_Id= '$sensor'";
            String q1 = " SELECT String_,Bit,Time_Correction FROM ATS_PM_String where Device_Id = '" + formData.device_id + "' ";
            SqlCommand cmd1 = new SqlCommand(q1, conn.Sqlconn);
            SqlDataReader dr = cmd1.ExecuteReader();
            //  List<AlarmBO> list1 = new List<AlarmBO>();
            int count = 0;
            while (dr.Read())
            {
                relay = new StringDO();
                relay.String_ = (string)dr[0];
                relay.Bit = (int)dr[1];
                relay.Time_Correction = (int)dr[2];
                if (relay.Bit != 1)
                {
                    relay = null;
                }
            }
            dr.Close();
            return relay;

        }

        private RelayDO readRelays(dataDO formData)
        {
            RelayDO relay = null;
            if (!(conn.IsOpen()))
            {
                conn.Open();
            }
            //$sql = "SELECT * FROM ATS_PM_Relays where Device_Id= '$sensor'";
            String q1 = " SELECT * FROM ATS_PM_Relays where Device_Id = '" + formData.device_id + "' ";
            SqlCommand cmd1 = new SqlCommand(q1, conn.Sqlconn);
            SqlDataReader dr = cmd1.ExecuteReader();
            //  List<AlarmBO> list1 = new List<AlarmBO>();
            int count = 0;
            while (dr.Read())
            {
                relay = new RelayDO();
                relay.R1 = (int)dr[1];
                relay.R2 = (int)dr[2];
                if (relay.R1 == 3 && relay.R2 == 3)
                {
                    relay = null;
                }
            }
            dr.Close();
            return relay;

        }
    }

    public class FormData
    {
        public string Name { get; set; }
        public string Email { get; set; }
        // Add more fields as needed
    }

    public class RelayDO
    {
        public int R1 { get; set; }
        public int R2 { get; set; }
        // Add more fields as needed
    }
    public class StringDO
    {
        public string String_ { get; set; }
        public int Bit { get; set; }
        public int Time_Correction { get; set; }
        // Add more fields as needed
    }

    public class dataDO
    {
        public string api_key { get; set; }
        public string device_id { get; set; }
        public string value1 { get; set; }
        public string value2 { get; set; }
        public string value3 { get; set; }
        public string value4 { get; set; }
        public string value5 { get; set; }
        public string value6 { get; set; }
        public string value7 { get; set; }
        public string value8 { get; set; }
        public string value9 { get; set; }
        public string value10 { get; set; }
        public string value11 { get; set; }
        public string value12 { get; set; }
        public string value13 { get; set; }
        public string value14 { get; set; }
        public string value15 { get; set; }

        // Add more fields as needed
    }
}
