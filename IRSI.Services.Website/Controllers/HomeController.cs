using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IRSI.Services.Website.ViewModels.Services;

namespace IRSI.Services.Website.Controllers
{
    public class HomeController : Controller
    {
        private IAuthorizationService _authorizationService;

        public HomeController(IAuthorizationService authroizationService)
        {
            _authorizationService = authroizationService;
        }

        public async Task<IActionResult> Index()
        {
            var services = new List<ServiceViewModel>();
            if (await _authorizationService.AuthorizeAsync(User, "UseSOSService"))
            {
                services.Add(new ServiceViewModel()
                {
                    Name = "Speed Of Service",
                    IconClass = "restaurant-cooking",
                    ServiceUrl = Url.Action("Index", "SOSService")
                });
            }

            if (await _authorizationService.AuthorizeAsync(User, "UseAVTService"))
            {
                services.Add(new ServiceViewModel()
                {
                    Name = "Actual vs. Theorical",
                    IconClass = "office-magnifying-glass",
                    ServiceUrl = Url.Action("Index", "AVTService")
                });
            }

            if (await _authorizationService.AuthorizeAsync(User, "UseTeamSalesService"))
            {
                services.Add(new ServiceViewModel()
                {
                    Name = "TeamSales Service",
                    IconClass = "marketing-time",
                    ServiceUrl = Url.Action("Index", "TeamSalesService")
                });
            }
            return View(services);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
