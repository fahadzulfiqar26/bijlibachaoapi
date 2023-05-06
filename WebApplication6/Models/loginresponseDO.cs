using System;

namespace WebApplication6.Models
{
    public class loginresponseDO
    {
        public string msn { get; set; }
        public string Description { get; set; }
        public string full_name { get; set; }

        public DateTime last_login { get; set; }


        public int num_logins { get; set; }

        public int IsActive { get; set; }
        public string Device_Type { get; set; }
       
    }
}
