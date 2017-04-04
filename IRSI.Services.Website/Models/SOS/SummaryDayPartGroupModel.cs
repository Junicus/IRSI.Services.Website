using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Website.Models.SOS
{
    public class SummaryDayPartGroupModel
    {
        public string DayPart { get; set; }
        public List<SummaryDateGroupModel> DayPartGroup { get; set; }

        public bool HasDateData()
        {
            var result = false;
            foreach (var dateGroup in DayPartGroup)
            {
                if (dateGroup.HasSummaryData())
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
