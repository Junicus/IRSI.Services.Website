using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Website.Models.TeamSales
{
    public class EmployeeSalesPerHourIncrement
    {
        public int EmployeeId { get; set; }
        public int StoreId { get; set; }
        public string EmployeeName { get; set; }
        public decimal CurrentSalesPerHour { get; set; }
        public decimal ComparativeSalesPerHour { get; set; }
        public decimal Difference { get; set; }
    }
}
