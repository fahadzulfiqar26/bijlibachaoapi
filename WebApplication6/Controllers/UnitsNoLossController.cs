using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using NPOI.Util;
using System;
using System.Collections.Generic;
using WebApplication6.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication6.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UnitsNoLossController : ControllerBase
    {
        // GET: api/<UnitsNoLossController>
       

        // POST api/<UnitsNoLossController>
        [HttpPost]
        public IActionResult Post([FromBody] UnitsNoLossModel value)
        {
            try
            {

                ReturnUnit unit = new ReturnUnit();
                   String connectionString = "server=164.92.148.47;database=meter_data;uid=node_user;pwd=RNK@ehsan123;";
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    //select max(energy_reg)-min(energy_reg) as ssss from energy_relay_status where msn='22491086' and (date_time BETWEEN '2023-03-17' and '2023-03-19')
                 //   string query2 = "SELECT Max(energy_reg)-min(energy_reg) as 'units' from (select * from energy_relay_status where date_time between '" + value.StartDate.ToString("yyyy-MM-dd") + "' and '" + value.EndDate.ToString("yyyy-MM-dd") + "' and msn='" + value.msn + "')";
                    string query = "select max(energy_reg)-min(energy_reg) as 'units' from energy_relay_status where msn='"+value.msn+"' and (date_time BETWEEN '"+ value.StartDate.ToString("yyyy-MM-dd") + "' and '"+ value.EndDate.AddDays(1).ToString("yyyy-MM-dd") + "')";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            MySqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                              //  if (!reader.IsDBNull(0))
                                {
                                    Double d = ((Double)reader[0]);
                                unit.Units = d;
                                }
                            }
                            reader.Close();
                        }
                    

                }
                ///////////
                //   string Query = "SELECT Max(energy_reg)-min(energy_reg) as 'units' from energy_relay_status where msn='21000925' and date_time>'2022-09-13 00:00:08'";
                if (unit!=null)
                {
                    return Ok(unit);
                }
                else { return NotFound(""); }
            }
            catch (Exception d)
            {
                return BadRequest();
            }
        }

     
    }
}
