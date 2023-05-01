using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication6.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DailyAverageCurrentController : ControllerBase
    {
        // GET: api/<DailyAverageCurrentController>
    

        // POST api/<DailyAverageCurrentController>
        List<AverageCalculation> listcal;
        // POST api/<DailyAverageVoltageController>
        [HttpPost]
        public IActionResult Post([FromBody] DailyAverageDO value)
        {
            try { 
            CultureInfo provider = CultureInfo.InvariantCulture;
            listcal = new List<AverageCalculation>();
            listcal.Clear();
            List<DailyAverageReturnCurrent> list = new List<DailyAverageReturnCurrent>();
            TimeZoneInfo timeZoneInfo;

            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            String TableName = "";
            if (value.Type.Equals("L1"))
            {
                TableName = "inst_data_L1";
            }
            else if (value.Type.Equals("L2"))
            {
                TableName = "inst_data_L2";
            }
            else if (value.Type.Equals("L3"))
            {
                TableName = "inst_data_L3";
            }
            else
            {
                return NotFound();
            }
            DateTime dateTime10 = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            DateTime dateTime100 = DateTime.ParseExact(dateTime10.ToString("yyyy-MM-dd ") + checkdate(0), ("yyyy-MM-dd HH"), provider);

            int count = 0;
            String connectionString = "server=164.92.148.47;database=meter_data;uid=node_user;pwd=RNK@ehsan123;";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT * FROM " + TableName + " where date_time >= '" + dateTime100.ToString("yyyy-MM-dd HH:mm:ss") + "'  and msn='" + value.Id + "' ORDER BY date_time";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                   // while (reader.Read())
                    {
                        while (reader.Read())
                        {
                            AverageCalculation obj = new AverageCalculation();
                            obj.value = ((Double)reader[3]);
                            obj.Date = ((DateTime)reader[1]);
                            listcal.Add(obj);
                        }
                    }
                }
            }

            list = magicfunction(listcal);
            if (list.Count == 0)
            {
                   
                return NotFound();
            }
            else
            {
                return Ok(list);
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
        private List<DailyAverageReturnCurrent> magicfunction(List<AverageCalculation> listcal)
        {
            List<DailyAverageReturnCurrent> list = new List<DailyAverageReturnCurrent>();
            if (listcal.Count == 0)
            {
                return list;
            }
            var d= listcal[listcal.Count - 1];
           var dd= d.Date.ToString("HH");
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
                    list.Add(new DailyAverageReturnCurrent()
                    {
                        AverageCurrent = 0,
                        Time = dateTime100.ToString("hh tt")
                    });
                  
                }
                else
                {
                    double avg = sum / count;
                    string c = avg.ToString("0.##");

                    list.Add(new DailyAverageReturnCurrent()
                    {
                        AverageCurrent = double.Parse(c),
                        Time = dateTime100.ToString("hh tt")

                    });
                }



            }
            // end
            return list;
        }

        // PUT api/<DailyAverageCurrentController>/5
      
    }
}
