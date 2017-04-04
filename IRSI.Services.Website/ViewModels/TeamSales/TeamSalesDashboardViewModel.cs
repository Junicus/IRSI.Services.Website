using IRSI.Services.Website.Models.Common;
using IRSI.Services.Website.Models.TeamSales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Website.ViewModels.TeamSales
{
    public class TeamSalesDashboardViewModel
    {
        public Store Store { get; set; }
        public DateTime CurrentStartDate { get; set; }
        public DateTime CurrentEndDate { get; set; }
        public double CurrentWeekSalesPerHour { get; set; }
        public double LastWeekSalesPerHour { get; set; }
        public List<EmployeeSalesPerHour> Bottom10EmployeeSales { get; set; }
        public List<EmployeeSalesPerHour> Top10EmployeeSales { get; set; }
        public List<EmployeeSalesPerHourIncrement> EmployeeSalesIncrementWeek { get; set; }
        public List<EmployeeSalesPerHourIncrement> EmployeeSalesIncrementPeriod { get; set; }
    }
}
