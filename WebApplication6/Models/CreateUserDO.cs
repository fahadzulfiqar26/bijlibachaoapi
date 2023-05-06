namespace WebApplication6.Models
{
    public class CreateUserDO
    {
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Msn { get; set; }
        public string Description { get; set; }
        public int Billing_Month_End { get; set; }
        public int Meter_Phases{ get; set; }

        public int isActive { get; set; }
        public string Device_Type { get; set; }
    
    }
}
