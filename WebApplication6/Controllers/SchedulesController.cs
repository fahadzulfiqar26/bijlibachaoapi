using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication6.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        // GET: api/<SchedulesController>
        [HttpGet]
        public IActionResult Get()
        {
            String connectionString = "server=164.92.148.47;database=meter_data;uid=node_user;pwd=RNK@ehsan123;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
              
                String query = "insert into meter_msn_testing (msn) values ('API')";
                using (MySqlCommand command = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        return Ok("Record inserted");
                    }
                    else
                    {
                        return NotFound();
                    }
                }
              }
               
        }

        // GET api/<SchedulesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SchedulesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SchedulesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SchedulesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
