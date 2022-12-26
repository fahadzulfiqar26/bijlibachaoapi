using System.Collections.Generic;

namespace WebApplication6
{
    public class choticlassi
    {
      public  string Title { get; set; }
      public  List<DailyAverageReturnCurrent> list { get; set; }

      public choticlassi()
        {
            list = new List<DailyAverageReturnCurrent>();
        }
    }
}
