using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;
using WebApplication6.Models;
using System.Linq;

namespace WebApplication6.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Info2Controller : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] Info2RequestDO value)
        {
            try
            {
                string id = "",desc="";
                var connectionString = "server=164.92.148.47;database=meter_data;uid=node_user;pwd=RNK@ehsan123;";
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    string query = "Select msn,Description from billing_data.users_app where user_name='" + value.Username+ "'";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        MySqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {

                            id = reader.GetString(0);
                            desc = reader.GetString(1);
                            reader.Close();
                            //  return Ok(obj);
                        }
                        else
                        {
                            return NotFound("User not registered");
                        }
                    }
                    List<MeterDO2> meters = new List<MeterDO2>();
                    if (id.Equals("")) { }
                    else if (id.Contains(",")) {
                         var list = id.Split(',');
                         var list2 = desc.Split(',');
                        for (int i = 0; i < list.Length; i++)
                        {
                            MeterDO2 t = new MeterDO2();
                            t.Msn = list[i].Split('-')[0];
                            if (list.Length == list2.Length)
                            {
                                t.Description = list2[i];
                            }
                            else
                            {
                                t.Description = "Invalid Description";
                            }
                           
                            meters.Add(t);
                        }
                    }
                    else
                    {
                        query = "Select * from MSN_Records  where id='" + id + "'";
                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            MySqlDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                MeterDO2 t = new MeterDO2();
                                t.Msn = reader.GetString(1);
                                t.Description = reader.GetString(2);
                                //  obj.Description = reader.GetString(4);

                                meters.Add(t);



                                // return Ok(obj);
                            }
                            reader.Close();

                        }
                    }
                    if (meters.Count == 0)
                    {
                        return NotFound("No device registered against this user.");
                    }
                    else
                    {
                        for (int i = 0; i < meters.Count; i++)
                        {
                            meters[i].Status=identify_status(meters[i].Msn, con);
                        }
                        return Ok(meters);
                    }

                    
                }
               
            }
            catch (Exception ee)
            {
                return BadRequest("User not found: " + ee.Message);
            }
        }
        string status = "";
        private string identify_status(string msn, MySqlConnection con)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");

            DateTime enddate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);

            //   string dateString = "2022-05-09";
            //  CultureInfo provider = CultureInfo.InvariantCulture;
            // It throws Argument null exception  
            //   DateTime dateTime10 = DateTime.ParseExact(dateString, ("yyyy-MM-dd"), provider);
         
                string query = "SELECT * FROM inst_data_L123 where msn='" + msn + "'  order by date_time desc limit 1";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        //and ((current_L1>'2' and kW_l1>'0' ) || (current_L2>'2' and kW_l2>'0' ) || (current_L3>'2' and kW_l3>'0' ))
                        
                        //You can get values from column names
                        var date = reader.GetDateTime(1);
                    

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
                        status = "Disconnected";
                    }
                reader.Close();
            }
            return status;
        }
    }
}
