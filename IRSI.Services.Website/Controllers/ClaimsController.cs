using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IRSI.Services.Website.Controllers
{
    public class ClaimsController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Claims()
        {
            ViewBag.IdentityToken = await HttpContext.Authentication.GetTokenAsync("id_token");
            ViewBag.AccessToken = await HttpContext.Authentication.GetTokenAsync("access_token");
            return View();
        }

        [Authorize]
        public async Task<IActionResult> CallApi()
        {
            var accessToken = await HttpContext.Authentication.GetTokenAsync("access_token");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await client.GetStringAsync("http://localhost:51000/api/account");
                ViewBag.Json = JArray.Parse(response).ToString();
                return View();
            }
        }
    }
}
