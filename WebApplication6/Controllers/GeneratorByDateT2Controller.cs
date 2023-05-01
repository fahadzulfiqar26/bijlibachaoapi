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
    public class GeneratorByDateT2Controller : ControllerBase
    {
        // GET: api/<GeneratorByDateT2Controller>
        [HttpPost]
        public IActionResult Post([FromBody] MsnByDates value)
        {
            try
            {
               var classobj= selectpart(value,1);
               var classobj2 = selectpart(value, 2);

                List<TimeMinutesT2> obj = new List<TimeMinutesT2>();
                obj.Add(new TimeMinutesT2() { Title="Line", times= classobj });
                obj.Add(new TimeMinutesT2() { Title = "Generator", times = classobj2 });

                if (obj.Count > 0)
                {

                    return Ok(obj);
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

       

         private List<TimeMinutes> selectpart(MsnByDates cls, int v)
        {
            List<TimeMinutes> classobj = new List<TimeMinutes>();

            string msn = cls.msn;
            List<List<DateTime>> listmaster = new List<List<DateTime>>();
            listmaster.Clear();

            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");

            DateTime enddate = cls.end_date;
            string dateString = "2022-" + checkdate(enddate.Month) + "-01";
            CultureInfo provider = CultureInfo.InvariantCulture;
            // It throws Argument null exception  
            DateTime startdate = cls.start_date;
            int count = 0;
            string connectionString = "server=164.92.148.47;database=meter_data;uid=node_user;pwd=RNK@ehsan123;";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                bool check = true;
                DateTime date = DateTime.Now;
                List<DateTime> datetime = new List<DateTime>();
                //testing   string query = "SELECT* FROM inst_data_L1 where date_time between '" + startdate.ToString("yyyy-MM-dd") + "' and '" + enddate.AddDays(1).ToString("yyyy-MM-dd") + "' and msn='"+msn+"' order by date_time ";   ///and current_L1>'0' and kW_l1>'0' and PF_L1>'0' order by date_time ";
                string query = "SELECT* FROM energy_relay_status where date_time between '" + startdate.ToString("yyyy-MM-dd") + "' and '" + enddate.AddDays(1).ToString("yyyy-MM-dd") + "' and msn='" + msn + "' and TNO='"+v+"' order by date_time ";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {


                        count++;

                        DateTime cc = (DateTime)reader[1];


                        if (check) //singleton
                        {
                            check = false;
                            date = cc;
                        }
                        if (date.Day != cc.Day)
                        {
                            listmaster.Add(datetime);
                            //   datetime.Clear();
                            datetime = new List<DateTime>();
                            date = cc;
                        }
                        datetime.Add(cc);

                    }
                    if (datetime.Count != 0)
                        listmaster.Add(datetime);

                }
            }

            for (int i = 0; i < listmaster.Count; i++)
            {
                double x = getminutes(listmaster[i]);
                TimeMinutes obj = new TimeMinutes();
                int min = (int)x;
                TimeSpan spWorkMin = TimeSpan.FromMinutes(min);
                string workHours = spWorkMin.ToString(@"hh\:mm");
                obj.time = workHours;
                obj.minutes = (int)x;
                obj.date = listmaster[i][0].ToShortDateString();
                classobj.Add(obj);

            }
            return classobj;
            //      double x = getminutes(datetime);
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
                        count += differenceminutes(start, datetime2[i]);
                    }
                }
                running = datetime2[i];
            }

            return count;
        }
        String gg = "";
        private double differenceminutes(DateTime start, DateTime dateTime)
        {
            TimeSpan ts = dateTime - start;


            //here backslash is must to tell that colon is
            //not the part of format, it just a character that we want in output
            gg = ts.ToString(@"hh\:mm");
            return ts.TotalMinutes;
        }
    }
}
