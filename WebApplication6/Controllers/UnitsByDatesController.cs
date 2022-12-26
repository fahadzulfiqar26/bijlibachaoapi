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
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<DailyLiveController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

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

        // PUT api/<DailyLiveController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DailyLiveController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
