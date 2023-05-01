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
    public class CombinedController : ControllerBase
    {
        // GET: api/<CombinedController>
  

        // POST api/<CombinedController>
        [HttpPost]
        public IActionResult Post([FromBody] UnitsNoLossModel value)
        {
            try
            {
                string str = value.msn;
                List<ReturnUnitDate> returnUnitDates = new List<ReturnUnitDate>();
                while (value.StartDate <= value.EndDate)
                {
                    ReturnUnitDate unit = new ReturnUnitDate();
                    unit.Units = 0;
                    String connectionString = "server=164.92.148.47;database=meter_data;uid=node_user;pwd=RNK@ehsan123;";
                    using (MySqlConnection con = new MySqlConnection(connectionString))
                    {
                        con.Open();
                        string query = "select * from api_table where title='" + str + "' ";
                        List<double> values = new List<double>();
                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            MySqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                //  if (!reader.IsDBNull(0))

                                string d = ((string)reader[1]);
                                string d2 = ((string)reader[2]);
                                var list = d2.Split(',');
                                reader.Close();
                                value.msn = d.Trim();
                                unit.Units = getunits(con, value);

                                reader.Close();
                                foreach (var item in list)
                                {
                                    value.msn = item.Trim();
                                    double units = getunits(con, value);
                                    values.Add(units);

                                }
                                foreach (var item in values)
                                {
                                    unit.Units -= item;
                                }
                                reader.Close();
                            }
                            
                        }
                        unit.date = value.StartDate;
                        returnUnitDates.Add(unit);
                        //select max(energy_reg)-min(energy_reg) as ssss from energy_relay_status where msn='22491086' and (date_time BETWEEN '2023-03-17' and '2023-03-19')
                        //   string query2 = "SELECT Max(energy_reg)-min(energy_reg) as 'units' from (select * from energy_relay_status where date_time between '" + value.StartDate.ToString("yyyy-MM-dd") + "' and '" + value.EndDate.ToString("yyyy-MM-dd") + "' and msn='" + value.msn + "')";

                        value.StartDate = value.StartDate.AddDays(1);
con.Close();
                    }
                }
                ///////////
                //   string Query = "SELECT Max(energy_reg)-min(energy_reg) as 'units' from energy_relay_status where msn='21000925' and date_time>'2022-09-13 00:00:08'";
                if (returnUnitDates.Count>0)
                {
                    return Ok(returnUnitDates);
                }
                else { return NotFound(""); }
            }
            catch (Exception d)
            {
                return BadRequest();
            }
        }

        private double getunits(MySqlConnection con, UnitsNoLossModel value)
        {
            double units = 0;
            string query = "select max(energy_reg)-min(energy_reg) as 'units' from energy_relay_status where msn='" + value.msn + "' and (date_time BETWEEN '" + value.StartDate.ToString("yyyy-MM-dd") + "' and '" + value.StartDate.AddDays(1).ToString("yyyy-MM-dd") + "')";

            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                     if (!reader.IsDBNull(0))
                    {
                        Double d = ((Double)reader[0]);
                        units = d;
                    }
                }
                reader.Close();
            }
            return units;

        }


  
    }
}
