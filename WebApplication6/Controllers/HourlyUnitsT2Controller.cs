using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Globalization;
using System;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HourlyUnitsT2Controller : ControllerBase
    {

        // POST api/<HourlyUnitsController>
        [HttpPost]
        public IActionResult Post([FromBody] HourlyUnitsRequest value)
        {
            try
            {
                HourlyUnitsT2DO hourlyUnitsT2DO = new HourlyUnitsT2DO();
               hourlyUnitsT2DO.Line= selectpart(value,1);
                hourlyUnitsT2DO.Generator = selectpart(value, 2);
                return Ok(hourlyUnitsT2DO);
            }
            catch (Exception d)
            {
                return BadRequest();
            }
        }

        List<HourlyUnitsQuerydata> datetime = new List<HourlyUnitsQuerydata>();

        private List<HourlyUnitResponse> selectpart(HourlyUnitsRequest msn, int tno)
        {
            datetime.Clear();

            List<HourlyUnitResponse> hourlyUnitResponses = new List<HourlyUnitResponse>();




            DateTime dateTime10 = msn.date;
            String connectionString = "server=164.92.148.47;database=meter_data;uid=node_user;pwd=RNK@ehsan123;";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                string query = "SELECT energy_reg, date_time from energy_relay_status where date_time between '" + dateTime10.ToString("yyyy-MM-dd") + "' and '" + dateTime10.AddDays(1).ToString("yyyy-MM-dd") + "' and msn='" + msn.Msn + "' and TNO='"+tno+"'";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        HourlyUnitsQuerydata obj = new HourlyUnitsQuerydata();
                        if (!reader.IsDBNull(0))
                        {
                            Double dd = ((Double)reader[0]);
                            obj.energy = dd;
                        }
                        var date = ((DateTime)reader[1]);
                        obj.dates = date;
                        datetime.Add(obj);
                    }
                    reader.Close();
                }


            }


            var d = datetime.Count;
            double units = 0;
            int startindex = 0, endindex = 0;
            bool start = false;
            CultureInfo provider = CultureInfo.InvariantCulture;
            // It throws Argument null exception  
            hourlyUnitResponses.Clear();
            if (datetime.Count < 1)
            {
                return hourlyUnitResponses;
            }
            //  result = getminutes(datetime);
            for (int i = 0; i < 24; i++)
            {
                startindex = 0;
                endindex = 0;
                units = 0;
                start = true;
                // list.Clear();
                DateTime dateTime100 = DateTime.ParseExact(dateTime10.ToString("yyyy-MM-dd ") + checkdate(i), ("yyyy-MM-dd HH"), provider);
                for (int j = 0; j < datetime.Count; j++)
                {
                    if (datetime[j].dates >= dateTime100 && datetime[j].dates < dateTime100.AddHours(1))
                    {
                        if (start)
                        {
                            start = false;
                            startindex = j;
                        }

                        endindex = j;

                    }


                }
                units = datetime[endindex].energy - datetime[startindex].energy;

                //   double min = getminutes(list);
                HourlyUnitResponse obj = new HourlyUnitResponse();
                obj.time = dateTime100.ToString("hh tt");
                obj.units = units;
                hourlyUnitResponses.Add(obj);

                
            }
            return hourlyUnitResponses;
            var ff = 0;
        }
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


    }
}
