using System;
using System.Collections.Generic;

namespace WebApplication6.Models
{
    public class LoginresponseDOt2
    {
         public string full_name { get; set; }

        public DateTime last_login { get; set; }


        public int num_logins { get; set; }

        public int IsActive { get; set; }
        public string Device_Type { get; set; }
        public List<MeterDO> MsnDetails { get; set; }
    }
}
