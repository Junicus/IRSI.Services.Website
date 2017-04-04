using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IRSI.Services.Website.ApiClients;
using IRSI.Services.Website.Models.Common;
using Microsoft.AspNetCore.Authentication;
using IRSI.Services.Website.ViewModels.TeamSales;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IRSI.Services.Website.Controllers
{
    [Authorize(policy: "UseTeamSalesService")]
    public class TeamSalesServiceController : Controller
    {
        private StoresApiClient _storeApiClient { get; set; }
        private TeamSalesApiClient _teamSalesApiClient { get; set; }

        public TeamSalesServiceController(TeamSalesApiClient teamSalesApiClient)
        {
            _teamSalesApiClient = teamSalesApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var conceptClaim = User.FindFirst("teamApiConcept").Value;
            if (string.IsNullOrEmpty(conceptClaim))
            {
                return new UnauthorizedResult();
            }

            var storeClaim = User.FindFirst("teamApiStore").Value;
            if (string.IsNullOrEmpty(storeClaim))
            {
                return new UnauthorizedResult();
            }
            var access_token = await HttpContext.Authentication.GetTokenAsync("access_token");
            _teamSalesApiClient.SetAccessToken(access_token);

            Guid storeId = Guid.Empty;
            Store store = null;
            if (Guid.TryParse(storeClaim, out storeId))
            {
                store = await _teamSalesApiClient.GetStoreAsync(storeId);
            }
            else
            {
                return new BadRequestResult();
            }
            var storeNum = int.Parse(store.Number.Substring(1, 2));
            var startDate = DateTime.Today.AddDays(-1 * ((int)DateTime.Today.DayOfWeek - 1));
            var endDate = startDate.AddDays(6);
            var lastStartDate = startDate.AddDays(-7);
            var lastEndDate = startDate.AddDays(-1);

            var currentStoreSales = await _teamSalesApiClient.GetSalesPerHour(conceptClaim, storeNum, startDate, endDate);
            var lastStoreSales = await _teamSalesApiClient.GetSalesPerHour(conceptClaim, storeNum, lastStartDate, lastEndDate);

            var top10Sales = await _teamSalesApiClient.GetTopEmployeeSalesPerHourAsync(conceptClaim, storeNum, startDate, endDate);
            var bottom10Sales = await _teamSalesApiClient.GetBottomEmployeeSalesPerHourAsync(conceptClaim, storeNum, startDate, endDate);
            var topImproveWeek = await _teamSalesApiClient.GetEmployeeSalesPerHourImproveWeekAsync(conceptClaim, storeNum, startDate, endDate);
            var topImprovePeriod = await _teamSalesApiClient.GetEmployeeSalesPerHourImprovePeriodAsync(conceptClaim, storeNum, startDate, endDate);

            var model = new TeamSalesDashboardViewModel()
            {
                Store = store,
                CurrentStartDate = startDate,
                CurrentEndDate = endDate,

                CurrentWeekSalesPerHour = currentStoreSales.SalesPerHour,
                LastWeekSalesPerHour = lastStoreSales.SalesPerHour,

                Top10EmployeeSales = top10Sales,
                Bottom10EmployeeSales = bottom10Sales,
                EmployeeSalesIncrementWeek = topImproveWeek,
                EmployeeSalesIncrementPeriod = topImprovePeriod
            };

            return View(model);
        }
    }
}
