using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Website.Models.SOS
{
    public class SummaryDateGroupModel
    {
        public DateTime BusinessDate { get; set; }
        public List<SummaryModel> DateGroup { get; set; }

        public bool HasSummaryData()
        {
            return DateGroup.Where(d => d.Count > 0).Any();
        }
    }
}
