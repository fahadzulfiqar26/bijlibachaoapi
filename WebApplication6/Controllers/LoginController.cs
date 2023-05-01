using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using WebApplication6.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication6.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        // GET: api/<LoginController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST api/<LoginController>
        [HttpPost]
        public IActionResult Post([FromBody] loginrequestDO value)
        {
            var connectionString = "server=164.92.148.47;database=meter_data;uid=node_user;pwd=RNK@ehsan123;";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = "Select msn,full_name,last_login,num_logins,Description from billing_data.users_app where user_name='" + value.user_name+"' and pass_word=md5('"+value.pass_word+"')";              
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        loginresponseDO obj = new loginresponseDO();
                        obj.msn = reader.GetString(0);
                        obj.full_name = reader.GetString(1);
                        obj.last_login = reader.GetDateTime(2);
                        obj.num_logins = reader.GetInt16(3);
                        obj.Description = reader.GetString(4);
                        reader.Close();

                        return Ok(obj);
                    }
                }
            }
                        return NotFound();
        }

        // PUT api/<LoginController>/5
    
    }
}
