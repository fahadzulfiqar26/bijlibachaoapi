using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static WebApplication6.Controllers.PdfLineController;
using System.Collections.Generic;
using System;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PdfGenController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] MsnByDates2 value)
        {
            try
            {
                List<List<TodayLiveGenerator>> megaList = new List<List<TodayLiveGenerator>>();
                GeneratorDetailsByDateT2Controller generatorDetailsByDateT2Controller = new GeneratorDetailsByDateT2Controller();
                var date = value.start_date;
                while (date.Date <= value.end_date.Date)
                {
                    var obj = new MsnDate() { date = date.Date, msn = value.msn };
                    var list = generatorDetailsByDateT2Controller.selectpart2(obj, 2);
                    if (list.Count > 0)
                    {
                        megaList.Add(list);
                    }
                    date = date.AddDays(1);
                }
                if (megaList.Count > 0)
                {


                    Genrating_PDF2 genrating_PDF = new Genrating_PDF2();
                    string url = genrating_PDF.Genrating_PDF_Function(megaList, value);
                    return Ok(new returnclass() { publicUrl = url });
                }

                return NotFound("No Record Found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
