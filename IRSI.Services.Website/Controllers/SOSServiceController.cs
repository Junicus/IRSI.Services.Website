using IRSI.Services.Website.ApiClients;
using IRSI.Services.Website.Models.Common;
using IRSI.Services.Website.Models.SOS;
using IRSI.Services.Website.ViewModels.SOS;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Website.Controllers
{
    [Authorize(Policy = "UseSOSService")]
    public class SOSServiceController : Controller
    {
        private SOSApiClient _client;

        public SOSServiceController(SOSApiClient client)
        {
            _client = client;
        }
        public async Task<IActionResult> Index()
        {
            var storeKpis = new Dictionary<Store, StoreKpiViewModel>();
            var access_token = await HttpContext.Authentication.GetTokenAsync("access_token");
            _client.SetAccessToken(access_token);
            var result = await _client.GetStoresAsync();

            var startDate = DateTime.Now.AddDays(-1 * (((int)DateTime.Now.DayOfWeek) - 1));
            var endDate = DateTime.Now;

            foreach (var store in result)
            {
                storeKpis.Add(store, new StoreKpiViewModel
                {
                    Kpis = new List<KpiModel>()
                });
                var kpisYesterday = await _client.GetKpisAsync(store, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-1));
                var kpisWeekToDate = await _client.GetKpisAsync(store, startDate, endDate);

                if (kpisYesterday.Any())
                {
                    storeKpis[store].Kpis.Add(new KpiModel()
                    {
                        Title = "Average Ticket Time",
                        Average = kpisYesterday.First().Average,
                        TimePeriod = "yesterday",
                        Goal = 9.5m,
                        Variance = Math.Round((kpisYesterday.First().Average - 9.5m) / kpisYesterday.First().Average * 100, 2)
                    });
                }

                if (kpisWeekToDate.Any())
                {
                    storeKpis[store].Kpis.Add(new KpiModel()
                    {
                        Title = "Average Ticket Time",
                        Average = kpisWeekToDate.Average(k => k.Average),
                        TimePeriod = "week to date",
                        Goal = 9.5m,
                        Variance = Math.Round((kpisWeekToDate.Average(k => k.Average) - 9.5m) / kpisWeekToDate.Average(k => k.Average) * 100, 2)
                    });
                }
            }

            return View(storeKpis);
        }

        [HttpGet]
        public async Task<IActionResult> TicketTime()
        {
            var access_token = await HttpContext.Authentication.GetTokenAsync("access_token");
            _client.SetAccessToken(access_token);
            var stores = await _client.GetStoresAsync();
            ViewBag.Stores = stores;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TicketTime([FromForm]TicketTimeViewModel viewModel)
        {
            var reportViewModel = new TicketTimeReportViewModel()
            {
                Summaries = new List<SummaryDayPartGroupModel>(),
                SummaryGroupTitles = new List<string>(),
                Averages = new List<KpiModel>()
            };

            var access_token = await HttpContext.Authentication.GetTokenAsync("access_token");
            _client.SetAccessToken(access_token);
            var store = await _client.GetStoreAsync(viewModel.StoreId);

            reportViewModel.Store = store;
            reportViewModel.StartDate = viewModel.StartDate;
            reportViewModel.EndDate = viewModel.EndDate;

            var dayPartGroups = await _client.GetSummariesAsync(store, viewModel.StartDate, viewModel.EndDate);
            foreach (var group in dayPartGroups)
            {
                var newGroup = new SummaryDayPartGroupModel()
                {
                    DayPart = group.DayPart,
                    DayPartGroup = new List<SummaryDateGroupModel>()
                };

                foreach (var dateGroup in group.DayPartGroup)
                {
                    if (dateGroup.DateGroup.Any(d => d.Count > 0))
                    {
                        newGroup.DayPartGroup.Add(dateGroup);
                    }
                }

                if (newGroup.DayPartGroup.Any())
                {
                    reportViewModel.Summaries.Add(newGroup);
                }
            }

            if (reportViewModel.Summaries.Any())
            {
                foreach (var summaryTitle in reportViewModel.Summaries.First().DayPartGroup.First().DateGroup)
                {
                    reportViewModel.SummaryGroupTitles.Add(summaryTitle.Summary);
                }
            }

            var averages = await _client.GetKpisAsync(store, viewModel.StartDate, viewModel.EndDate);
            foreach (var avg in averages)
            {
                reportViewModel.Averages.Add(avg);
            }

            return View("TicketTimeReport", reportViewModel);
        }
    }
}
