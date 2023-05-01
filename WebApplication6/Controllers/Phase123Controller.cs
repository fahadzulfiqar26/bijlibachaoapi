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
                        try
                        {
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
                        }
                        catch(Exception d) { }
                        reader.Close();
                        //  obj.Fullname = reader.GetString(1);
                        Log_Load_Records(value.msn,con);
                        return Ok(obj);
                    }
                }
            }
            return NotFound();
        }
        string type = "Phase123";
        private void Log_Load_Records(String msn, MySqlConnection con)
        {
            int count = 0;
            var month = DateTime.Now.Month;
            string query = "Select count from  Load_Records where month='" + month + "' and msn='" + msn + "' and api_name='"+type+"'";


            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    try
                    {
                        count = reader.GetInt16(0);
                      
                    }
                    catch(Exception d) { }  
                    }
                reader.Close();
            }
            count++;
            add_record(type, month, count, msn, con);
                    }

        private void add_record(string v, int month, int count, string msn, MySqlConnection con)
        {
            if (count == 1)
            {
                String query = "insert into Load_Records (msn,month,api_name,count) values ('" + msn + "','" + month + "','" + v + "','" + count + "')";
                using (MySqlCommand command = new MySqlCommand(query, con))
                {

                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {

                    }
                    else
                    {

                    }
                }
            }
            else
            {
               

                    string query = "UPDATE Load_Records SET count=@count WHERE msn=@msn and api_name=@apiname and month=@month";

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@count", count);
                    cmd.Parameters.AddWithValue("@msn", msn);
                    cmd.Parameters.AddWithValue("@apiname", type);
                    cmd.Parameters.AddWithValue("@month", month);
                 
                    cmd.Connection = con;
                    int dr = cmd.ExecuteNonQuery();
                    if (dr > 0)
                    {
                      
                    }
                    else
                    {
                       
                    }
                
            }
        }


        // PUT api/<Phase123Controller>/5

    }
}
