using IRSI.Services.Website.Models.Common;
using IRSI.Services.Website.Models.SOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Website.ViewModels.SOS
{
    public class TicketTimeReportViewModel
    {
        public Store Store { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> SummaryGroupTitles { get; set; }
        public List<SummaryDayPartGroupModel> Summaries { get; set; }
        public List<KpiModel> Averages { get; set; }
    }
}
