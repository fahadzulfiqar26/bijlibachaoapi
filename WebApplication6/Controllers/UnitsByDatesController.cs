using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication6.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UnitsByDatesController : ControllerBase
    {
        // GET: api/<DailyLiveController>
       
        // POST api/<DailyLiveController>
        [HttpPost]
        public IActionResult Post([FromBody] MsnByDates value)
        {
            try
            {

                List<DailyLiveDO> list = new List<DailyLiveDO>();
                list.Clear();
                TimeZoneInfo timeZoneInfo;
                timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");

                DateTime enddate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);



                DateTime dateTime10 = value.start_date;
                String connectionString = "server=164.92.148.47;database=meter_data;uid=node_user;pwd=RNK@ehsan123;";
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    while (dateTime10 <= value.end_date)
                    {
                        string query = "SELECT Max(energy_reg)-min(energy_reg) as 'units' from energy_relay_status where date_time between '" + dateTime10.ToString("yyyy-MM-dd") + "' and '" + dateTime10.AddDays(1).ToString("yyyy-MM-dd") + "' and msn='" + value.msn + "'";
                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            MySqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                {
                                    Double d = ((Double)reader[0]);
                                    list.Add(new DailyLiveDO() { Enerygy = d.ToString("0.##"),Date= dateTime10.ToString("yyyy-MM-dd") });
                                }
                            }
                            reader.Close();
                        } dateTime10 = dateTime10.AddDays(1);
                    }
                   
                }
                ///////////

                Log_Load_Records(value.msn);
                //   string Query = "SELECT Max(energy_reg)-min(energy_reg) as 'units' from energy_relay_status where msn='21000925' and date_time>'2022-09-13 00:00:08'";
                if (list.Count > 0)
                {
                    return Ok(list);
                }
                else { return NotFound(); }
            }
            catch (Exception d)
            {
                return BadRequest();
            }
        }


        string type = "UnitByDates";
        private void Log_Load_Records(String msn)
        {
            String connectionString = "server=164.92.148.47;database=meter_data;uid=node_user;pwd=RNK@ehsan123;";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                int count = 0;
                var month = DateTime.Now.Month;
                string query = "Select count from  Load_Records where month='" + month + "' and msn='" + msn + "' and api_name='" + type + "'";


                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        try
                        {
                            count = reader.GetInt16(0);

                        }
                        catch (Exception d) { }
                    }
                    reader.Close();
                }
                count++;
                add_record(type, month, count, msn, con);
            }
        }

        private void add_record(string v, int month, int count, string msn, MySqlConnection con)
        {
            if (count == 1)
            {
                String query = "insert into Load_Records (msn,month,api_name,count) values ('" + msn + "','" + month + "','" + v + "','" + count + "')";
                using (MySqlCommand command = new MySqlCommand(query, con))
                {

                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {

                    }
                    else
                    {

                    }
                }
            }
            else
            {


                string query = "UPDATE Load_Records SET count=@count WHERE msn=@msn and api_name=@apiname and month=@month";

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@count", count);
                cmd.Parameters.AddWithValue("@msn", msn);
                cmd.Parameters.AddWithValue("@apiname", type);
                cmd.Parameters.AddWithValue("@month", month);

                cmd.Connection = con;
                int dr = cmd.ExecuteNonQuery();
                if (dr > 0)
                {

                }
                else
                {

                }

            }
        }


        // PUT api/<DailyLiveController>/5

    }
}
