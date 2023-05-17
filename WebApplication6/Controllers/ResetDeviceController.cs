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
    public class ResetDeviceController : ControllerBase
    {
        // GET: api/<ResetDeviceController>



        // POST api/<ResetDeviceController>
        [HttpPost]
        public IActionResult Post([FromBody] msnclass value)
        {
            try
           {

               
                {
                   
                    resetfun("inst_data_L123", value);
                    resetfun("energy_relay_status", value);
                    resetfun("daily_energy_data", value);
                    resetfun("reactive_energy_relay_status", value);


                    ////////////////////////////////////
                 
                    if (count > 0)
                    {
                        return Ok("Device: "+value.id+" is reset successfully");
                    }
                    else
                    {
                        return Ok("No Record Found");
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }

        }
        int count = 0;
        private void resetfun(string table, msnclass value)
        {
            string connectionString = "server=164.92.148.47;database=meter_data;uid=node_user;pwd=RNK@ehsan123;";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                string query = "DELETE FROM "+table+"  WHERE msn=@user_name2";


                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = query;

                cmd.Parameters.AddWithValue("@user_name2", value.id);
                cmd.Connection = con;
                int dr = cmd.ExecuteNonQuery();
                count += dr;
               
            }
            }

        // PUT api/<ResetDeviceController>/5
     
    }
}
