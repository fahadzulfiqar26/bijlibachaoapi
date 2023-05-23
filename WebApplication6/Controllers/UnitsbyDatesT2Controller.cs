using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UnitsbyDatesT2Controller : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] MsnByDates value)
        {
            try
            {

                List<DailyLiveDoT2> list = new List<DailyLiveDoT2>();
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
                        DailyLiveDoT2 t2 = new DailyLiveDoT2();
                        t2.Generator_Enerygy = "0";
                        t2.Line_Enerygy = "0";
                        string query = "SELECT Max(energy_reg_t1)-min(energy_reg_t1) as 'units' from energy_relay_status where date_time between '" + dateTime10.ToString("yyyy-MM-dd") + "' and '" + dateTime10.AddDays(1).ToString("yyyy-MM-dd") + "' and msn='" + value.msn + "' and TNO='1'";
                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            t2.Date = dateTime10.ToString("yyyy-MM-dd");
                            MySqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                {
                                    Double d = ((Double)reader[0]);
                                    t2.Line_Enerygy = d.ToString("0.##");
                                  }
                            }
                            reader.Close();
                        }
                        // generator

                         query = "SELECT Max(energy_reg_t2)-min(energy_reg_t2) as 'units' from energy_relay_status where date_time between '" + dateTime10.ToString("yyyy-MM-dd") + "' and '" + dateTime10.AddDays(1).ToString("yyyy-MM-dd") + "' and msn='" + value.msn + "' and TNO='2'";
                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            t2.Date = dateTime10.ToString("yyyy-MM-dd");
                            MySqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                {
                                    Double d = ((Double)reader[0]);
                                    t2.Generator_Enerygy = d.ToString("0.##");
                                }
                            }
                            reader.Close();
                        }
                        list.Add(t2);
                        dateTime10 = dateTime10.AddDays(1);
                    }

                }
                ///////////

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

    }
}
