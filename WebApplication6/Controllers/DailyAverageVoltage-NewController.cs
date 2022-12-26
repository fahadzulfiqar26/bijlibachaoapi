﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

using System.Globalization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication6.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DailyAverageVoltage_NewController : ControllerBase
    {
        // GET: api/<DailyAverageCurrent_NewController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<DailyAverageCurrent_NewController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        List<AverageCalculation> listcal, listcal2, listcal3;
        // POST api/<DailyAverageCurrent_NewController>
        [HttpPost]
        public IActionResult Post([FromBody] DailyAverageDO value)
        {
            try
            {
                megalist = new List<choticlassv>();
                CultureInfo provider = CultureInfo.InvariantCulture;
                listcal = new List<AverageCalculation>();
                listcal2 = new List<AverageCalculation>();
                listcal3 = new List<AverageCalculation>();

                List<DailyAverageReturnCurrent> list = new List<DailyAverageReturnCurrent>();
                TimeZoneInfo timeZoneInfo;

                timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");

                DateTime dateTime10 = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
                DateTime dateTime100 = DateTime.ParseExact(dateTime10.ToString("yyyy-MM-dd ") + checkdate(0), ("yyyy-MM-dd HH"), provider);

                int count = 0;
                String connectionString = "server=164.92.148.47;database=meter_data;uid=node_user;pwd=RNK@ehsan123;";
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT * FROM inst_data_L123 where date_time >= '" + dateTime100.ToString("yyyy-MM-dd HH:mm:ss") + "'  and msn='" + value.Id + "' ORDER BY date_time";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                       //     while (reader.Read())
                            {
                                AverageCalculation obj = new AverageCalculation();
                                obj.value = ((Double)reader[2]);
                                obj.Date = ((DateTime)reader[1]);
                                listcal.Add(obj);


                                AverageCalculation obj2 = new AverageCalculation();
                                obj2.value = ((Double)reader[7]);
                                obj2.Date = ((DateTime)reader[1]);
                                listcal2.Add(obj2);

                                AverageCalculation obj3 = new AverageCalculation();
                                obj3.value = ((Double)reader[11]);
                                obj3.Date = ((DateTime)reader[1]);
                                listcal3.Add(obj3);
                            }
                        }
                    }
                }

                megalist.Add(magicfunction(listcal,1));
                megalist.Add(magicfunction(listcal2,2));

                megalist.Add(magicfunction(listcal3,3));

                if (megalist.Count == 0)
                {

                    return NotFound();
                }
                else
                {
                    return Ok(megalist);
                }
            }
            catch (Exception d)
            {
                return BadRequest();
            }

        }
        private string checkdate(int month)
        {
            if (month < 10)
                return "0" + month;
            else
            {
                return month.ToString();
            }
        }
        List<choticlassv> megalist;
        private choticlassv magicfunction(List<AverageCalculation> listcal,int id)
        {
            choticlassv list2 = new choticlassv();
            list2.Title = "Voltage L" + id;
            if (listcal.Count == 0)
            {
                return list2;
            }
            var d = listcal[listcal.Count - 1];
            var dd = d.Date.ToString("HH");
            CultureInfo provider = CultureInfo.InvariantCulture;

            TimeZoneInfo timeZoneInfo;
            // Set the time zone information from US Mountain Standard Time to Pakistan Standard Time
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");

            // Get date and time in US Mountain Standard Time 
            DateTime dateTime10 = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            // Magic Begins here

            for (int i = 0; i < int.Parse(dd); i++)
            {

                double count = 0; double sum = 0;
                DateTime dateTime100 = DateTime.ParseExact(dateTime10.ToString("yyyy-MM-dd ") + checkdate(i), ("yyyy-MM-dd HH"), provider);
                for (int j = 0; j < listcal.Count; j++)
                {
                    if (listcal[j].Date >= dateTime100 && listcal[j].Date < dateTime100.AddHours(1))
                    {
                        count++;
                        sum += listcal[j].value;
                        // list.Add(datetime[j]);
                    }


                }
                if (sum == 0) // to avoid exception div/0
                {
                    list2.list.Add(new DailyAverageReturnVoltage()
                    {
                        AverageVoltage = 0,
                        Time = dateTime100.ToString("hh tt")
                    });

                }
                else
                {
                    double avg = sum / count;
                    string c = avg.ToString("0.##");

                    list2.list.Add(new DailyAverageReturnVoltage()
                    {
                        AverageVoltage = double.Parse(c),
                        Time = dateTime100.ToString("hh tt")

                    });
                }



            }
            // end
            return list2;
        }

        // PUT api/<DailyAverageCurrent_NewController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DailyAverageCurrent_NewController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
