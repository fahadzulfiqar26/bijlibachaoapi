using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using NPOI.SS.Formula.Functions;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DeleteUserController : ControllerBase
    {
        int count = 0;
        [HttpPost]
        public IActionResult Post([FromBody] UserDo value)
        {
            resetfun(value);
            if (count > 0)
            {
                return Ok("Deleted Successfully");
            }
            else
            {
                return NotFound("No Record Found");
            }
        }
            private void resetfun(UserDo value)
        {
            string connectionString = "server=164.92.148.47;database=meter_data;uid=node_user;pwd=RNK@ehsan123;";
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                string query = "DELETE FROM  billing_data.users_app  WHERE user_name=@user_name2";


                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = query;

                cmd.Parameters.AddWithValue("@user_name2", value.Username);
                cmd.Connection = con;
                int dr = cmd.ExecuteNonQuery();
                count += dr;

            }
        }
    }
}
