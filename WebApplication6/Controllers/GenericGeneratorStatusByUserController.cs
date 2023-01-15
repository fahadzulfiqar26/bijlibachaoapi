using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using WebApplication6.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericGeneratorStatusByUserController : ControllerBase
    {
        // GET: api/<GenericGeneratorStatusByUserController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<GenericGeneratorStatusByUserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
   // POST api/<GenericGeneratorStatusByUserController>
        [HttpPost]
        public IActionResult Post([FromBody] GenericRequest value)
        {
            try
            {
                var msnlist = loginresponse(value.Username);
           var genericlist=     selectpart(value, msnlist);
                if (genericlist.Count > 0)
                {

                    return Ok(genericlist);
                }
                else
                {
                    return NotFound();
                }
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private List<GenericStatusDO> selectpart(GenericRequest cls, List<GenericDO> msnlist)
        {
            List<GenericStatusDO> genericlist= new List<GenericStatusDO>();

           
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");

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
                for (int i = 0; i < msnlist.Count; i++)
                {

                    String status = "Device not found!";
                    String msn = msnlist[i].Msn;
                    String Description = msnlist[i].Description;
                    string query = "SELECT * FROM inst_data_L123 where msn='" + msn + "'  order by date_time desc limit 1";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        MySqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            //and ((current_L1>'2' and kW_l1>'0' ) || (current_L2>'2' and kW_l2>'0' ) || (current_L3>'2' and kW_l3>'0' ))
                            count++;
                            //You can get values from column names
                            var date = reader.GetDateTime(1);
                            var cur1 = reader.GetDouble(3);
                            var kw1 = reader.GetDouble(4);

                            var cur2 = reader.GetDouble(8);
                            var kw2 = reader.GetDouble(9);

                            var cur3 = reader.GetDouble(12);
                            var kw3 = reader.GetDouble(13);

                            var s = enddate - date;
                            if (s.TotalMinutes > 15)
                            {
                                status = "Disconnected";
                            }
                            else
                            {
                                if ((cur1 > 2 && kw1 > 0) || (cur2 > 2 && kw2 > 0) || (cur3 > 2 && kw3 > 0))
                                {
                                    status = "ON";
                                }
                                else
                                {
                                    status = "OFF";
                                }
                            }
                            //Or return the value from the columnID, in this case, 0
                            //  Console.WriteLine(reader.GetInt32(0));
                        }
                        else
                        {
                            status = "Device not found!";
                        }

                        genericlist.Add(new GenericStatusDO() { Msn = msn, Description = Description, Status = status });
                        reader.Close();
                    }

                }

            }
           





            return genericlist;
         


            //      double x = getminutes(datetime);
        }


        private List<GenericDO> loginresponse(String str)
        {
            List<GenericDO> result = new List<GenericDO>();
            var connectionString = "server=164.92.148.47;database=meter_data;uid=node_user;pwd=RNK@ehsan123;";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = "Select msn,Description from billing_data.users_app where user_name='" + str + "'";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {

                        String s = reader.GetString(0);
                        String s1 = reader.GetString(1);
                        var list1 = s.Split(',');
                        var list2 = s1.Split(',');

                        for (int i = 0; i < list1.Length; i++)
                        {
                            GenericDO obj = new GenericDO();
                            obj.Msn = list1[i].Split('-')[0];
                            obj.Description = list2[i];
                            result.Add(obj);
                        }


                        reader.Close();

                    }
                }

            }
            return result;
        }


        // PUT api/<GenericGeneratorStatusByUserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GenericGeneratorStatusByUserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
