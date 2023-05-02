using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using WebApplication6.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication6.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GeneratorStatusT2Controller : ControllerBase
    {
        // GET: api/<GeneratorStatusT2Controller>
        // POST api/<GeneratorStatus>
        [HttpPost]
        public IActionResult Post([FromBody] msnclass value)
        {

            return Ok(selectpart2(value));
        }

        String status = "";
        private GeneratorstatusResponseT2 selectpart2(msnclass value)
        {
            GeneratorstatusResponseT2 obj = new GeneratorstatusResponseT2();
            String msn = value.id;
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            obj.Type = "";
            DateTime enddate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);

            //   string dateString = "2022-05-09";
            //  CultureInfo provider = CultureInfo.InvariantCulture;
            // It throws Argument null exception  
            //   DateTime dateTime10 = DateTime.ParseExact(dateString, ("yyyy-MM-dd"), provider);
            int count = 0;
            var connectionString = "server=164.92.148.47;database=meter_data;uid=node_user;pwd=RNK@ehsan123;";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT date_time, TNO FROM energy_relay_status where msn='" + msn + "'  order by date_time desc limit 1";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        //and ((current_L1>'2' and kW_l1>'0' ) || (current_L2>'2' and kW_l2>'0' ) || (current_L3>'2' and kW_l3>'0' ))
                        count++;
                        //You can get values from column names
                        var date = reader.GetDateTime(0);
                        var tno = reader.GetInt16(1);
                        if(tno==1)
                        {
                            obj.Type = "Line";
                        }
                        else if (tno == 2)
                        {
                            obj.Type = "Generator";
                        }
                        var s = enddate - date;
                        if (s.TotalMinutes > 15)
                        {
                            status = "Disconnected";
                        }
                        else
                        {
                            status = "Connected";
                        }
                        //Or return the value from the columnID, in this case, 0
                        //  Console.WriteLine(reader.GetInt32(0));
                    }
                    else
                    {
                        status = "Device not found!";
                    }
                }
            }
            obj.Status = status;
            return obj;

        }


    }
}
