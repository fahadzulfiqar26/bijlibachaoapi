using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using WebApplication6.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication6.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Phase123Controller : ControllerBase
    {
        // GET: api/<Phase123Controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<Phase123Controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<Phase123Controller>
        [HttpPost]
        public IActionResult Post([FromBody] Phase123DO value)
        {
           var connectionString = "server=164.92.148.47;database=meter_data;uid=node_user;pwd=RNK@ehsan123;";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = "Select msn,date_time  as DateTime,voltage_L1,current_l1,kW_l1,PF_L1,Freq_L1,voltage_L2,current_l2,kW_l2,PF_L2,voltage_L3,current_l3,kW_l3,PF_L3  from inst_data_L123 where msn='"+value.msn+"' order by date_time desc limit 1";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                
                    while (reader.Read())
                    {
                        phase123returnDO obj = new phase123returnDO();
                        obj.msn = reader.GetString(0);
                        obj.DateTime = reader.GetDateTime(1);
                        obj.voltage_L1 = reader.GetString(2);
                        obj.current_l1 = reader.GetString(3);
                        obj.kW_l1 = reader.GetString(4);
                        obj.PF_L1 = reader.GetString(5);
                        obj.Freq_L1 = reader.GetString(6);
                        obj.voltage_L2 = reader.GetString(7);
                        obj.current_l2 = reader.GetString(8);
                        obj.kW_l2 = reader.GetString(9);
                        obj.PF_L2 = reader.GetString(10);
                        obj.voltage_L3 = reader.GetString(11);
                        obj.current_l3 = reader.GetString(12);
                        obj.kW_l3 = reader.GetString(13);
                        obj.PF_L3 = reader.GetString(14);
                        //  obj.Fullname = reader.GetString(1);
                        return Ok(obj);
                    }
                }
            }
            return NotFound();
        }
                    // PUT api/<Phase123Controller>/5
                    [HttpPut("{id}")]
                    public void Put(int id, [FromBody] string value)
                    {
                    }

                    // DELETE api/<Phase123Controller>/5
        [HttpDelete("{id}")]
                    public void Delete(int id)
                    {
                    }
                }
}
