using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using WebApplication6.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Login2Controller : ControllerBase
    {
        // GET: api/<Login2Controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<Login2Controller>/5
      

        // POST api/<Login2Controller>
        [HttpPost]
        public IActionResult Post([FromBody] loginrequestDO value)
        {
            LoginresponseDOt2 obj = new LoginresponseDOt2();
            string id = "";
            var connectionString = "server=164.92.148.47;database=meter_data;uid=node_user;pwd=RNK@ehsan123;";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = "Select msn,full_name,last_login,num_logins,Description,IsActive,Device_Type from billing_data.users_app_2t where user_name='" + value.user_name + "' and pass_word=md5('" + value.pass_word + "')";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {

                        id = reader.GetString(0);
                        obj.full_name = reader.GetString(1);
                        obj.last_login = reader.GetDateTime(2);
                        obj.num_logins = reader.GetInt16(3);
                        var d = reader.GetString(4);
                        obj.IsActive = reader.GetInt16(5);
                        obj.Device_Type = reader.GetString(6);
                        reader.Close();




                      //  return Ok(obj);
                    }
                    else
                    {
                        return NotFound("Invalid username or password");
                    }
                }
                List<MeterDO> meters = new List<MeterDO>();
                if (id.Equals("")) { }
                else
                {
                    query = "Select * from MSN_Records  where id='" + id + "'";
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            MeterDO t = new MeterDO();
                            t.Msn = reader.GetString(1);
                            t.Description = reader.GetString(2);
                            t.Type = reader.GetString(3);
                            //  obj.Description = reader.GetString(4);

                            meters.Add(t);



                            // return Ok(obj);
                        }
                        reader.Close();

                    }
                }
                obj.MsnDetails = meters;
                return Ok(obj);
            }
            return NotFound();
        }


    
    }
}
