using IRSI.Services.Website.Models.SOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Website.ViewModels.SOS
{
    public class StoreKpiViewModel
    {
        public ICollection<KpiModel> Kpis { get; set; }
    }
}
