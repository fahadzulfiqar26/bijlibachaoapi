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
    public class MonthlyController : ControllerBase
    {
        // GET: api/<MonthlyController>
        [HttpGet]
        public string Get()
        {
            // selectpart();
            return "value";
        }

        // GET api/<MonthlyController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {

            return "value";
        }

        // POST api/<MonthlyController>
        [HttpPost]
        public IActionResult Post([FromBody] msnclass value)
        {
            try { 
            selectpart(value.id);
            if (classobj.Count > 0)
            {
                return Ok(classobj);
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

        // PUT api/<MonthlyController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MonthlyController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        List<TimeMinutes> classobj = new List<TimeMinutes>();
        private void selectpart(string msn)
        {
            
            List<List<DateTime>> listmaster = new List<List<DateTime>>();
            listmaster.Clear();
           
                TimeZoneInfo timeZoneInfo;
                timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");

                 DateTime enddate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            string dateString = "2022-"+checkdate(enddate.Month) +"-01";
            CultureInfo provider = CultureInfo.InvariantCulture;
            // It throws Argument null exception  
            DateTime startdate = DateTime.ParseExact(dateString, ("yyyy-MM-dd"), provider);
            int count = 0;
            string connectionString = "server=164.92.148.47;database=meter_data;uid=node_user;pwd=RNK@ehsan123;";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                bool check = true;
                DateTime date=DateTime.Now;
                List<DateTime> datetime = new List<DateTime>();
             //testing   string query = "SELECT* FROM inst_data_L1 where date_time between '" + startdate.ToString("yyyy-MM-dd") + "' and '" + enddate.AddDays(1).ToString("yyyy-MM-dd") + "' and msn='"+msn+"' order by date_time ";
                string query = "SELECT* FROM inst_data_L1 where date_time between '" + startdate.ToString("yyyy-MM-dd") + "' and '" + enddate.AddDays(1).ToString("yyyy-MM-dd") + "' and msn='" + msn + "' and current_L1>'0' and kW_l1>'0' and PF_L1>'0' order by date_time ";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                       
                        
                        count++;
                        
                        DateTime cc=   (DateTime)reader[1];
                       

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
                    if(datetime.Count!=0)
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
