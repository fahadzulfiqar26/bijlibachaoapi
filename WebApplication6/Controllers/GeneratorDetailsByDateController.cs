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
    public class GeneratorDetailsByDateController : ControllerBase
    {
        private string connectionString;

        // GET: api/<TodayController>
      
        List<TodayLiveGenerator> MegaList;
        // POST api/<TodayController>
        [HttpPost]
        public IActionResult Post([FromBody] MsnDate value)
        {
            MegaList = new List<TodayLiveGenerator>();
            selectpart2(value);
            if (MegaList.Count > 0)
                return Ok(MegaList);
            else
                return NotFound();
        }

        private void selectpart2(MsnDate value)
        {
            String msn=value.msn;
            datetime.Clear();
            TimeZoneInfo timeZoneInfo;
            // Set the time zone information from US Mountain Standard Time to Pakistan Standard Time
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");

            // Get date and time in US Mountain Standard Time 
            DateTime dateTime10 = value.date;
            //   string dateString = "2022-05-09";
            //  CultureInfo provider = CultureInfo.InvariantCulture;
            // It throws Argument null exception  
            //   DateTime dateTime10 = DateTime.ParseExact(dateString, ("yyyy-MM-dd"), provider);
            int count = 0;
            connectionString = "server=164.92.148.47;database=meter_data;uid=node_user;pwd=RNK@ehsan123;";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT* FROM inst_data_L123 where date_time between '" + dateTime10.ToString("yyyy-MM-dd") + "' and '" + dateTime10.AddDays(1).ToString("yyyy-MM-dd") + "' and msn='" + msn + "' and ((current_L1>'2' and kW_l1>'0' ) || (current_L2>'2' and kW_l2>'0' ) || (current_L3>'2' and kW_l3>'0' )) order by date_time";
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
            int x = 0;
            if (datetime.Count == 0)
            {
                return;
            }
            TodayLiveGenerator obj = new TodayLiveGenerator();
            obj.Minutes = 0;
            obj.start_time = datetime[0];
            for (int i = 0; i < datetime.Count - 1; i++)
            {
                if (i == 144)
                {
                    int ddd = 0;
                }

                if (differenceminutes(datetime[i], datetime[i + 1]) < 5)
                {
                    obj.Minutes = obj.Minutes + differenceminutes(datetime[i], datetime[i + 1]);

                    TimeSpan spWorkMin = TimeSpan.FromMinutes(obj.Minutes);
                    string workHours = spWorkMin.ToString(@"hh\:mm");
                    obj.Time = workHours;
                }
                else
                {
                    obj.end_time = datetime[i];
                    PopulateMegaList(obj);

                    obj.Minutes = 0;
                    obj.start_time = datetime[i + 1];
                }
            }

            obj.end_time = datetime[datetime.Count - 1];
            PopulateMegaList(obj);




            int sss = 0;
        }

        private void PopulateMegaList(TodayLiveGenerator obj)
        {
            TodayLiveGenerator todayLiveGenerator = new TodayLiveGenerator()
            {
                start_time = obj.start_time,
                end_time = obj.end_time,
                Minutes = obj.Minutes,
                Time = obj.Time
            };
            MegaList.Add(todayLiveGenerator);
        }

        List<DateTime> datetime = new List<DateTime>();

        private string checkdate(int month)
        {
            if (month < 10)
                return "0" + month;
            else
            {
                return month.ToString();
            }
        }
        String timestring = "";
        private double differenceminutes(DateTime start, DateTime dateTime)
        {
            TimeSpan ts = dateTime - start;
            timestring = ts.TotalHours + ":" + ts.Minutes;
            return ts.TotalMinutes;
        }

    }
}
