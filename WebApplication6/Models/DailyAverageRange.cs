using System;

namespace WebApplication6.Models
{
    public class DailyAverageRange
    {
        public String Id { get; set; }
        public String Type { get; set; } // L1,L2,L3
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; } 

    }
}
