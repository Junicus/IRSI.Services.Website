using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Website.Models.SOS
{
    public class SummaryModel
    {
        public DateTime BusinessDate { get; set; }
        public string DayPart { get; set; }
        public string Summary { get; set; }
        public int Count { get; set; }
    }
}
