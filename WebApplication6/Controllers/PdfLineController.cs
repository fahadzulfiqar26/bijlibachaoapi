using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PdfLineController : ControllerBase
    {
        public class returnclass
        {
            public string publicUrl { get; set; }
        }
      
        [HttpPost]
        public IActionResult Post([FromBody] MsnByDates2 value)
        {
            try
            {
                var value2 = new MsnByDates() { start_date=value.start_date, end_date=value.end_date, msn=value.msn };
                UnitsbyDatesT2Controller unitsbyDatesT2Controller = new UnitsbyDatesT2Controller();
                var response = unitsbyDatesT2Controller.Post(value2) as ObjectResult;
                if (response != null)
                {
                    var result = response.Value as List<DailyLiveDoT2>;

                    Genrating_PDF genrating_PDF = new Genrating_PDF();
                    string url = genrating_PDF.Genrating_PDF_Function(result, value);
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
