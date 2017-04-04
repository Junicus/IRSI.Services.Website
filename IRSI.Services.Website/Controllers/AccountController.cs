using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IRSI.Services.Website.Controllers
{
    public class AccountController : Controller
    {
        public async Task Logout()
        {
            await HttpContext.Authentication.SignOutAsync("cookies");
            await HttpContext.Authentication.SignOutAsync("oidc");
        }

        public async Task<IActionResult> Login(string returnUrl = "/")
        {
            await HttpContext.Authentication.ChallengeAsync("oidc");

            if (User.Identity.IsAuthenticated)
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View("Error");
        }

        public IActionResult AccessDenied(string returnUrl = "/")
        {
            return View("Error");
        }
    }
}
