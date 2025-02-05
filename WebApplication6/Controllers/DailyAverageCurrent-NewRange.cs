﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Globalization;
using System;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DailyAverageCurrent_NewRange : ControllerBase
    {
        // GET: api/<DailyAverageCurrent_NewController>

        List<AverageCalculation> listcal, listcal2, listcal3;
       
        // POST api/<DailyAverageCurrent_NewController>
        [HttpPost]
        public IActionResult Post([FromBody] DailyAverageRange value)
        {
            try
            {
                List<Rangereturn> returndata = new List<Rangereturn>();
               while(value.StartDate<=value.EndDate)  
                {


                    megalist = new List<choticlassi>();
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    listcal = new List<AverageCalculation>();
                    listcal2 = new List<AverageCalculation>();
                    listcal3 = new List<AverageCalculation>();

                    List<DailyAverageReturnCurrent> list = new List<DailyAverageReturnCurrent>();
                    TimeZoneInfo timeZoneInfo;

                    timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");

                    DateTime dateTime10 = value.StartDate;
                    DateTime dateTime100 = DateTime.ParseExact(dateTime10.ToString("yyyy-MM-dd ") + checkdate(0), ("yyyy-MM-dd HH"), provider);

                    int count = 0;
                    String connectionString = "server=164.92.148.47;database=meter_data;uid=node_user;pwd=RNK@ehsan123;";
                    using (MySqlConnection con = new MySqlConnection(connectionString))
                    {
                        con.Open();
                        string query = "SELECT * FROM inst_data_L123 where date_time >= '" + dateTime100.ToString("yyyy-MM-dd HH:mm:ss") + "' and date_time <= '"+ dateTime100.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") + "'   and msn='" + value.Id + "' ORDER BY date_time";
                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            MySqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                //     while (reader.Read())
                                {
                                    try
                                    {
                                        AverageCalculation obj = new AverageCalculation();
                                        obj.value = ((Double)reader[3]);
                                        obj.Date = ((DateTime)reader[1]);
                                        listcal.Add(obj);
                                    }
                                    catch (Exception d) { }

                                    try
                                    {
                                        AverageCalculation obj2 = new AverageCalculation();
                                        obj2.value = ((Double)reader[8]);
                                        obj2.Date = ((DateTime)reader[1]);
                                        listcal2.Add(obj2);
                                    }
                                    catch (Exception d) { }

                                    try
                                    {
                                        AverageCalculation obj3 = new AverageCalculation();
                                        obj3.value = ((Double)reader[12]);
                                        obj3.Date = ((DateTime)reader[1]);
                                        listcal3.Add(obj3);
                                    }
                                    catch (Exception d) { }
                                }
                            }
                        }
                    }

                    megalist.Add(magicfunction(listcal, 1, value.StartDate));
                    megalist.Add(magicfunction(listcal2, 2, value.StartDate));

                    megalist.Add(magicfunction(listcal3, 3, value.StartDate));

                  
                    returndata.Add(new Rangereturn() { Date = value.StartDate.ToString("yyyy-MM-dd HH:mm:ss"), Details = megalist });
                    value.StartDate= value.StartDate.AddDays(1);
                }
                if (returndata.Count == 0)
                {

                    return NotFound();
                }
                else
                {
                    return Ok(returndata);
                }
            }
            catch (Exception d)
            {
                return BadRequest();
            }

        }
        private string checkdate(int month)
        {
            if (month < 10)
                return "0" + month;
            else
            {
                return month.ToString();
            }
        }
        List<choticlassi> megalist;
        private choticlassi magicfunction(List<AverageCalculation> listcal, int id,DateTime date)
        {
            choticlassi list2 = new choticlassi();
            list2.Title = "Current L" + id;
            if (listcal.Count == 0)
            {
                return list2;
            }
            var dd = 24;
            CultureInfo provider = CultureInfo.InvariantCulture;

            TimeZoneInfo timeZoneInfo;
            // Set the time zone information from US Mountain Standard Time to Pakistan Standard Time
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");

            // Get date and time in US Mountain Standard Time 
            DateTime dateTime10 = date;
            // Magic Begins here

            for (int i = 0; i < dd; i++)
            {

                double count = 0; double sum = 0;
                DateTime dateTime100 = DateTime.ParseExact(dateTime10.ToString("yyyy-MM-dd ") + checkdate(i), ("yyyy-MM-dd HH"), provider);
                for (int j = 0; j < listcal.Count; j++)
                {
                    if (listcal[j].Date >= dateTime100 && listcal[j].Date < dateTime100.AddHours(1))
                    {
                        count++;
                        sum += listcal[j].value;
                        // list.Add(datetime[j]);
                    }


                }
                if (sum == 0) // to avoid exception div/0
                {
                    list2.list.Add(new DailyAverageReturnCurrent()
                    {
                        AverageCurrent = 0,
                        Time = dateTime100.AddHours(1).ToString("hh tt")
                    });

                }
                else
                {
                    double avg = sum / count;
                    string c = avg.ToString("0.##");

                    list2.list.Add(new DailyAverageReturnCurrent()
                    {
                        AverageCurrent = double.Parse(c),
                        Time = dateTime100.AddHours(1).ToString("hh tt")

                    });
                }



            }
            // end

            return list2;

        }

        // PUT api/<DailyAverageCurrent_NewController>/5
    }
}
