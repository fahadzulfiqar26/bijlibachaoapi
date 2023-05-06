using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using WebApplication6.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication6.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CreateUserController : ControllerBase
    {
        // GET: api/<CreateUserController>
      

        // POST api/<CreateUserController>
        [HttpPost]
        public IActionResult Post([FromBody] CreateUserDO obj)
        {
            try
            {
                var arr= obj.Msn.Split(',');
                var arr2 = obj.Description.Split(',');
                if (!(arr.Length == arr2.Length))
                {
                    return BadRequest("MSN and DESCRIPTION parameters are not same");
                }

                obj.Password = Custom_Encryption.encrypt2(obj.Password);
                var date = DateTime.Now;
                String connectionString = "server=164.92.148.47;database=billing_data;uid=node_user;pwd=RNK@ehsan123;";
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {

                    String query = "insert into users_app (full_name,user_name,pass_word,msn,last_login,login_created,num_logins,billing_month_end,meter_phases,Description,IsActive,Device_Type) values ('" + obj.Fullname + "','" + obj.Username + "','" + obj.Password + "','" + obj.Msn + "','" + date.ToString("yyyy-MM-dd HH:mm:ss") + "','" + date.ToString("yyyy-MM-dd HH:mm:ss") + "','" + 0 + "','" + obj.Billing_Month_End + "','" + obj.Meter_Phases + "','" + obj.Description+ "','" + obj.isActive + "','" + obj.Device_Type + "')";
                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        conn.Open();
                        int result = command.ExecuteNonQuery();
                        if (result > 0)
                        {
                            return Ok("User Created Successfully");
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                }
            }
            catch(Exception d)
            {
                if (d.Message.Equals("Duplicate entry 'string' for key 'users_app.username_unique'"))
                {
                    return BadRequest("User \"" + obj.Username + "\" is already created ");
                }
                else
                {
                    return BadRequest(d.Message);
                }
            }
        }

        // PUT api/<CreateUserController>/5
    
    }
}
