using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using WebApplication6.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication6.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HourlyUnitsController : ControllerBase
    {
        // GET: api/<HourlyUnitsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<HourlyUnitsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<HourlyUnitsController>
        [HttpPost]
        public IActionResult Post([FromBody] HourlyUnitsRequest value)
        {
            try
            {
                selectpart(value);
                if (finallist.Count > 0)
                {
                    return Ok(finallist);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception d)
            {
                return BadRequest();
            }
        }

        List<HourlyUnitsQuerydata> datetime = new List<HourlyUnitsQuerydata>();

        private void selectpart(HourlyUnitsRequest msn)
        {
            datetime.Clear();



            


            DateTime dateTime10 = msn.date;
            String connectionString = "server=164.92.148.47;database=meter_data;uid=node_user;pwd=RNK@ehsan123;";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
               
                    string query = "SELECT energy_reg, date_time from energy_relay_status where date_time between '" + dateTime10.ToString("yyyy-MM-dd") + "' and '" + dateTime10.AddDays(1).ToString("yyyy-MM-dd") + "' and msn='" + msn.Msn + "'";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            HourlyUnitsQuerydata obj = new HourlyUnitsQuerydata();
                            if (!reader.IsDBNull(0))
                            {
                                Double dd = ((Double)reader[0]);
                                obj.energy= dd;
                            }
                            var date = ((DateTime)reader[1]);
                            obj.dates= date;
                        datetime.Add(obj);
                    }
                        reader.Close();
                    }
                 

            }


            var d = datetime.Count;
            double units =0;
            int startindex = 0, endindex = 0;
            bool start=false;
            CultureInfo provider = CultureInfo.InvariantCulture;
            // It throws Argument null exception  
            finallist.Clear();
            if (datetime.Count < 1)
            {
                return;
            }
          //  result = getminutes(datetime);
            for (int i = 0; i < 24; i++)
            {
                startindex = 0;
                endindex = 0;
                units = 0;
                start= true;
                // list.Clear();
                DateTime dateTime100 = DateTime.ParseExact(dateTime10.ToString("yyyy-MM-dd ") + checkdate(i), ("yyyy-MM-dd HH"), provider);
                for (int j = 0; j < datetime.Count; j++)
                {
                    if (datetime[j].dates >= dateTime100 && datetime[j].dates < dateTime100.AddHours(1))
                    {
                        if (start)
                        {
                            start=false;
                            startindex= j;
                        }
                       
                            endindex= j;
                        
                     }


                }
                units= datetime[endindex].energy - datetime[startindex].energy;

             //   double min = getminutes(list);
                HourlyUnitResponse obj = new HourlyUnitResponse();
                obj.time = dateTime100.ToString("hh tt");
                obj.units = units;
                finallist.Add(obj);


            }
            var ff = 0;
        }
        List<HourlyUnitResponse> finallist = new List<HourlyUnitResponse>();
        double result = 0;
   //     List<DateTime> list = new List<DateTime>();
        private string checkdate(int month)
        {
            if (month < 10)
                return "0" + month;
            else
            {
                return month.ToString();
            }
        }
        private double getminutes(List<DateTime> datetime2)
        {
            if (datetime2.Count == 0)
                return 0;
            DateTime start, end, running;
            start = running = datetime2[0];
            double count = 0;
            for (int i = 0; i < datetime2.Count; i++)
            {
                double x = differenceminutes(running, datetime2[i]);
                if (x > 10)
                {
                    count += differenceminutes(start, running);
                    start = running = datetime2[i];
                }
                else
                {
                    if (i == datetime2.Count - 1)
                    {

                        if (start.Minute <= 3)
                            start = start.AddMinutes(-1 * (start.Minute));
                        var f = datetime2[i];
                        if (f.Minute >= 57)
                            datetime2[i] = f.AddMinutes((60 - (f.Minute)));

                        count += differenceminutes(start, datetime2[i]);
                    }
                }
                running = datetime2[i];
            }

            return count;
        }

        private double differenceminutes(DateTime start, DateTime dateTime)
        {
            TimeSpan ts = dateTime - start;
            return ts.TotalMinutes;
        }


        // PUT api/<HourlyUnitsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<HourlyUnitsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
