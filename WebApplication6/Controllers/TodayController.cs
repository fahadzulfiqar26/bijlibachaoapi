using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication6.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TodayController : ControllerBase
    {
        private string connectionString;

        // GET: api/<TodayController>
     
        // POST api/<TodayController>
        [HttpPost]
        public IActionResult Post([FromBody] msnclass value)
        {
            try
            {
                selectpart(value.id);
                if (finallist.Count > 0)
                {
                    return Ok(finallist);
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception d)
            {
                return BadRequest();
            }
        }
        List<DateTime> datetime = new List<DateTime>();

        private void selectpart(string msn)
        {
            datetime.Clear();
            TimeZoneInfo timeZoneInfo;
            // Set the time zone information from US Mountain Standard Time to Pakistan Standard Time
          timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");

            // Get date and time in US Mountain Standard Time 
           DateTime dateTime10 = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
         //   string dateString = "2022-05-09";
         //  CultureInfo provider = CultureInfo.InvariantCulture;
            // It throws Argument null exception  
         //   DateTime dateTime10 = DateTime.ParseExact(dateString, ("yyyy-MM-dd"), provider);
            int count = 0;
            connectionString = "server=164.92.148.47;database=meter_data;uid=node_user;pwd=RNK@ehsan123;";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT* FROM inst_data_L1 where date_time between '" + dateTime10.ToString("yyyy-MM-dd") + "' and '" + dateTime10.AddDays(1).ToString("yyyy-MM-dd") + "' and msn='" + msn + "' and current_L1>'0' and kW_l1>'0' and PF_L1>'0'";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        count++;
                        //You can get values from column names
                       datetime.Add((DateTime)reader[1]);
                        //Or return the value from the columnID, in this case, 0
                      //  Console.WriteLine(reader.GetInt32(0));
                    }
                }
            }
            var d = datetime.Count;
            
             CultureInfo provider = CultureInfo.InvariantCulture;
            // It throws Argument null exception  
            finallist.Clear();
            result =  getminutes(datetime);
            for (int i = 0; i < 24; i++)
            {
                list.Clear();
                DateTime dateTime100 = DateTime.ParseExact(dateTime10.ToString("yyyy-MM-dd ")+ checkdate(i), ("yyyy-MM-dd HH"), provider);
                for (int j = 0; j < datetime.Count; j++)
                {
                    if(datetime[j]>=dateTime100 && datetime[j] < dateTime100.AddHours(1))
                    {
                        list.Add(datetime[j]);
                    }

                    
                }
             double min=   getminutes(list);
                timeformatclass obj = new timeformatclass();
                obj.time = dateTime100.ToString("hh tt");
                obj.minutes = (int)min;
                finallist.Add(obj);


            }
            var ff = 0;
        }
        List<timeformatclass> finallist = new List<timeformatclass>();
        double result = 0;
        List<DateTime> list = new List<DateTime>();
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
                            start = start.AddMinutes(-1*(start.Minute));
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

        // PUT api/<TodayController>/5
  
    }
}
