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
    public class GetAllUsersController : ControllerBase
    {
        List<CreateUserDO> list;
        // GET: api/<GetAllUsersController>
        [HttpGet]
        public IActionResult Get()
        {
            selectpart2();
            return Ok(list);
        }
        private void selectpart2()
        {
           list = new List<CreateUserDO>();
           var connectionString = "server=164.92.148.47;database=billing_data;uid=node_user;pwd=RNK@ehsan123;";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT * FROM users_app";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        CreateUserDO obj = new CreateUserDO();
                        obj.Fullname = reader.GetString(1);
                        obj.Username = reader.GetString(2);
                        obj.Password = reader.GetString(3);
                        obj.Msn = reader.GetString(4);
                        obj.Billing_Month_End = reader.GetInt32(8);
                        obj.Meter_Phases = reader.GetInt32(9);
                        obj.Description = reader.GetString(10);
                        obj.isActive = reader.GetInt16(13);
                        obj.Device_Type = reader.GetString(14);
                        list.Add(obj);
                     //   datetime.Add((DateTime)reader[1]);
                       
                     
                    }
                }
            }
           
        }
        private void selectpart2(string user)
        {
            list = new List<CreateUserDO>();
            var connectionString = "server=164.92.148.47;database=billing_data;uid=node_user;pwd=RNK@ehsan123;";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT * FROM users_app where user_name='"+user+"'";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        CreateUserDO obj = new CreateUserDO();
                        obj.Fullname = reader.GetString(1);
                        obj.Username = reader.GetString(2);
                        obj.Password = reader.GetString(3);
                        obj.Msn = reader.GetString(4);
                        obj.Billing_Month_End = reader.GetInt32(8);
                        obj.Meter_Phases = reader.GetInt32(9);
                        obj.Description = reader.GetString(10);
                        obj.isActive = reader.GetInt16(13);
                        obj.Device_Type = reader.GetString(14);
                        list.Add(obj);
                        //   datetime.Add((DateTime)reader[1]);


                    }
                }
            }

        }

        // GET api/<GetAllUsersController>/5
      

        // POST api/<GetAllUsersController>
        [HttpPost]
        public IActionResult Post([FromBody] String user)
        {
            selectpart2(user);
            return Ok(list);
        }

        // PUT api/<GetAllUsersController>/5
    
    }
}
