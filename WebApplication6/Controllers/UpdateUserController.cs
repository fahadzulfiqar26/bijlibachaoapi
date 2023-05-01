using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using WebApplication6.Models;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication6.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UpdateUserController : ControllerBase
    {
        // GET: api/<UpdateUserController>
      

        // POST api/<UpdateUserController>
        [HttpPost]
        public IActionResult Post([FromBody] CreateUserDO obj)
        {
            try
            {

                var arr = obj.Msn.Split(',');
                var arr2 = obj.Description.Split(',');
                if (!(arr.Length == arr2.Length))
                {
                    return BadRequest("MSN and DESCRIPTION parameters are not same");
                }

                var connectionString = "server=164.92.148.47;database=billing_data;uid=node_user;pwd=RNK@ehsan123;";
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    string query = "UPDATE users_app SET full_name=@full_name,user_name=@user_name, pass_word=@pass_word ,msn =@msn,billing_month_end=@billing_month_end,meter_phases=@meter_phases,Description=@desc WHERE user_name=@user_name2";

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@full_name", obj.Fullname);
                    cmd.Parameters.AddWithValue("@user_name", obj.Username);
                    cmd.Parameters.AddWithValue("@pass_word", Custom_Encryption.encrypt2(obj.Password));
                    cmd.Parameters.AddWithValue("@msn", obj.Msn);
                    cmd.Parameters.AddWithValue("@billing_month_end", Convert.ToInt16(obj.Billing_Month_End));
                    cmd.Parameters.AddWithValue("@meter_phases", Convert.ToInt16(obj.Meter_Phases));
                    cmd.Parameters.AddWithValue("@desc", obj.Description);
                    cmd.Parameters.AddWithValue("@user_name2", obj.Username);
                    cmd.Connection = con;
                    int dr = cmd.ExecuteNonQuery();
                    if (dr > 0)
                    {
                        return Ok("Record Updated Successfully");
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);

            }
          
        }

        // PUT api/<UpdateUserController>/5
    
    }
}
