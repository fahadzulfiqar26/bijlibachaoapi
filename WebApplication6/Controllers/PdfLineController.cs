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
      
        [HttpPost]
        public IActionResult Post([FromBody] MsnByDates value)
        {
            UnitsbyDatesT2Controller unitsbyDatesT2Controller = new UnitsbyDatesT2Controller();
            var response = unitsbyDatesT2Controller.Post(value) as ObjectResult;
            if (response != null)
            {
                var result = response.Value as List<DailyLiveDoT2>;
              
                Genrating_PDF genrating_PDF = new Genrating_PDF();
              string url=  genrating_PDF.start(result);
            }

                return Ok("");
        }
    }
}
