using System.Collections.Generic;

namespace WebApplication6
{
    public class choticlassv
    {
        public string Title { get; set; }
        public List<DailyAverageReturnVoltage> list { get; set; }

        public choticlassv()
        {
            list = new List<DailyAverageReturnVoltage>();
        }
    }
}
