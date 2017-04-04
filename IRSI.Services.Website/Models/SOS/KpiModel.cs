using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Website.Models.SOS
{
    public class KpiModel
    {
        public Guid Id { get; set; }
        public Guid StoreId { get; set; }
        public Guid RegionId { get; set; }
        public Guid ConceptId { get; set; }
        public DateTime BusinessDate { get; set; }

        public string Title { get; set; }
        public decimal Average { get; set; }
        public string TimePeriod { get; set; }
        public decimal Goal { get; set; }
        public decimal Variance { get; set; }
    }
}
