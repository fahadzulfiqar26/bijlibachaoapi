using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication6.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MultiChannel : ControllerBase
    {
        // GET: api/<MultiChannel>
     
        // GET api/<MultiChannel>/5
        [HttpGet]
        public IActionResult Get(string value)
        {
            MegaList = new List<List<double>>();
            int bytesC = Encoding.ASCII.GetByteCount(value);

            try
            {
                list = new List<double>();
                list1 = new List<double>();
                list2 = new List<double>();
                list3 = new List<double>();
                list4 = new List<double>();
                list5 = new List<double>();
                list6 = new List<double>();
                list7 = new List<double>();
                list8 = new List<double>();
                list9 = new List<double>();
                list10 = new List<double>();
                list11 = new List<double>();
                list12 = new List<double>();

                //   return ddd;

                /*
                            using (StreamReader r = new StreamReader("D:\\json2.json"))
                            {

                                 json = r.ReadToEnd();
                            }
                            */

                JObject json2 = JObject.Parse(value);

                var lists1 = json2["Load1"].Values().ToList();
                var lists2 = json2["Load2"].Values().ToList();
                var lists3 = json2["Load3"].Values().ToList();
                var lists4 = json2["Load4"].Values().ToList();
                var lists5 = json2["Load5"].Values().ToList();
                var lists6 = json2["Load6"].Values().ToList();
                var lists7 = json2["Load7"].Values().ToList();
                var lists8 = json2["Load8"].Values().ToList();
                var lists9 = json2["Load9"].Values().ToList();
                var lists10 = json2["Load10"].Values().ToList();
                var lists11 = json2["Load11"].Values().ToList();
                var lists12 = json2["Load12"].Values().ToList();

                for (int i = 0; i < lists1.Count; i++)
                {
                    list1.Add(double.Parse(lists1[i].First.ToString()));
                }

                for (int i = 0; i < lists2.Count; i++)
                {
                    list2.Add(double.Parse(lists2[i].First.ToString()));
                }

                for (int i = 0; i < lists3.Count; i++)
                {
                    list3.Add(double.Parse(lists3[i].First.ToString()));
                }

                for (int i = 0; i < lists4.Count; i++)
                {
                    list4.Add(double.Parse(lists4[i].First.ToString()));
                }

                for (int i = 0; i < lists5.Count; i++)
                {
                    list5.Add(double.Parse(lists5[i].First.ToString()));
                }

                for (int i = 0; i < lists6.Count; i++)
                {
                    list6.Add(double.Parse(lists6[i].First.ToString()));
                }


                for (int i = 0; i < lists7.Count; i++)
                {
                    list7.Add(double.Parse(lists7[i].First.ToString()));
                }


                for (int i = 0; i < lists8.Count; i++)
                {
                    list8.Add(double.Parse(lists8[i].First.ToString()));
                }


                for (int i = 0; i < lists9.Count; i++)
                {
                    list9.Add(double.Parse(lists9[i].First.ToString()));
                }


                for (int i = 0; i < lists10.Count; i++)
                {
                    list10.Add(double.Parse(lists10[i].First.ToString()));
                }


                for (int i = 0; i < lists11.Count; i++)
                {
                    list11.Add(double.Parse(lists11[i].First.ToString()));
                }


                for (int i = 0; i < lists12.Count; i++)
                {
                    list12.Add(double.Parse(lists12[i].First.ToString()));
                }

                list.Add(double.Parse(json2["Device"].ToString()));
                list.Add(double.Parse(json2["RatedVoltage"].ToString()));
                list.Add(double.Parse(json2["RatedCurrent"].ToString()));
                list.Add(double.Parse(json2["VoltageRatio"].ToString()));
                list.Add(double.Parse(json2["CurrentRatio1"].ToString()));
                list.Add(double.Parse(json2["CurrentRatio2"].ToString()));
                list.Add(double.Parse(json2["checkDigit"].ToString()));
                list.Add((bytesC * 1.0));

                MegaList.Add(list);
                MegaList.Add(list1);
                MegaList.Add(list2);
                MegaList.Add(list3);
                MegaList.Add(list4);
                MegaList.Add(list5);
                MegaList.Add(list6);
                MegaList.Add(list7);
                MegaList.Add(list8);
                MegaList.Add(list9);
                MegaList.Add(list10);
                MegaList.Add(list11);
                MegaList.Add(list12);
            }
            catch (Exception f)
            {

                MegaList.Add(new List<double> { bytesC });
            }
            //     ddd.ToString();
            return Ok(MegaList);
        }
        string json;
        List<double> list,list1,list2,list3,list4,list5,list6,list7,list8,list9,list10,list11,list12;
        List<List<double>> MegaList;
        // POST api/<MultiChannel>
        [HttpPost]
        public List<List<double>> Post( String value)
        {

            /*
                           using (StreamReader r = new StreamReader("D:\\json2.json"))
                           {

                                json = r.ReadToEnd();
                           
                          }*/
            MegaList = new List<List<double>>();
            int bytesC = Encoding.ASCII.GetByteCount(value);

            try
            {
                list = new List<double>();
                list1 = new List<double>();
                list2 = new List<double>();
                list3 = new List<double>();
                list4 = new List<double>();
                list5 = new List<double>();
                list6 = new List<double>();
                list7 = new List<double>();
                list8 = new List<double>();
                list9 = new List<double>();
                list10 = new List<double>();
                list11 = new List<double>();
                list12 = new List<double>();

                //   return ddd;

                /*
                            using (StreamReader r = new StreamReader("D:\\json2.json"))
                            {

                                 json = r.ReadToEnd();
                            }
                            */

                JObject json2 = JObject.Parse(value);

                var lists1 = json2["Load1"].Values().ToList();
                var lists2 = json2["Load2"].Values().ToList();
                var lists3 = json2["Load3"].Values().ToList();
                var lists4 = json2["Load4"].Values().ToList();
                var lists5 = json2["Load5"].Values().ToList();
                var lists6 = json2["Load6"].Values().ToList();
                var lists7 = json2["Load7"].Values().ToList();
                var lists8 = json2["Load8"].Values().ToList();
                var lists9 = json2["Load9"].Values().ToList();
                var lists10 = json2["Load10"].Values().ToList();
                var lists11 = json2["Load11"].Values().ToList();
                var lists12 = json2["Load12"].Values().ToList();

                for (int i = 0; i < lists1.Count; i++)
                {
                    list1.Add(double.Parse(lists1[i].First.ToString()));
                }

                for (int i = 0; i < lists2.Count; i++)
                {
                    list2.Add(double.Parse(lists2[i].First.ToString()));
                }

                for (int i = 0; i < lists3.Count; i++)
                {
                    list3.Add(double.Parse(lists3[i].First.ToString()));
                }

                for (int i = 0; i < lists4.Count; i++)
                {
                    list4.Add(double.Parse(lists4[i].First.ToString()));
                }

                for (int i = 0; i < lists5.Count; i++)
                {
                    list5.Add(double.Parse(lists5[i].First.ToString()));
                }

                for (int i = 0; i < lists6.Count; i++)
                {
                    list6.Add(double.Parse(lists6[i].First.ToString()));
                }


                for (int i = 0; i < lists7.Count; i++)
                {
                    list7.Add(double.Parse(lists7[i].First.ToString()));
                }


                for (int i = 0; i < lists8.Count; i++)
                {
                    list8.Add(double.Parse(lists8[i].First.ToString()));
                }


                for (int i = 0; i < lists9.Count; i++)
                {
                    list9.Add(double.Parse(lists9[i].First.ToString()));
                }


                for (int i = 0; i < lists10.Count; i++)
                {
                    list10.Add(double.Parse(lists10[i].First.ToString()));
                }


                for (int i = 0; i < lists11.Count; i++)
                {
                    list11.Add(double.Parse(lists11[i].First.ToString()));
                }


                for (int i = 0; i < lists12.Count; i++)
                {
                    list12.Add(double.Parse(lists12[i].First.ToString()));
                }

                list.Add(double.Parse(json2["Device"].ToString()));
                list.Add(double.Parse(json2["RatedVoltage"].ToString()));
                list.Add(double.Parse(json2["RatedCurrent"].ToString()));
                list.Add(double.Parse(json2["VoltageRatio"].ToString()));
                list.Add(double.Parse(json2["CurrentRatio1"].ToString()));
                list.Add(double.Parse(json2["CurrentRatio2"].ToString()));
                list.Add(double.Parse(json2["checkDigit"].ToString()));
                list.Add((bytesC * 1.0));

                MegaList.Add(list);
                MegaList.Add(list1);
                MegaList.Add(list2);
                MegaList.Add(list3);
                MegaList.Add(list4);
                MegaList.Add(list5);
                MegaList.Add(list6);
                MegaList.Add(list7);
                MegaList.Add(list8);
                MegaList.Add(list9);
                MegaList.Add(list10);
                MegaList.Add(list11);
                MegaList.Add(list12);
            }
            catch(Exception f)
            {

                MegaList.Add(new List<double> { bytesC });
            }
            //     ddd.ToString();
            return MegaList;
        }

        // PUT api/<MultiChannel>/5
  
    }
}
