using CookiePopupUsingAspNet.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace CookiePopupUsingAspNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (Request.Cookies["RequiredCookies"] != null) //checks the existance of the cookie
            {
                ViewData["HasAcceptedCookies"] = "yes";
                ViewData["RequiredCookies"] = "Accepted";
            }
            if (Request.Cookies["PersonalizationCookies"] != null)
            {
                ViewData["HasAcceptedCookies"] = "yes";
                ViewData["PersonalizationCookies"] = "Accepted";
            }
            if (Request.Cookies["ThirdPartyCookies"] != null)
            {
                ViewData["HasAcceptedCookies"] = "yes";
                ViewData["ThirdPartyCookies"] = "Accepted";
            }
            if (Request.Cookies["UserRejectedAll"] != null)
            {
                ViewData["HasAcceptedCookies"] = "no";
            }

            return View();
        }

        public async Task<IActionResult> CookieAcceptButton(IFormCollection form)
        {
        
            if (form["RequiredCookiesSwitch"] == "on")
            {
                var _claim = new List<Claim>
                {
                    new Claim("required_cookies", "on")
                };
                var _claimIdentity = new ClaimsIdentity(_claim, "RequiredCookies");
                ClaimsPrincipal _claimsPrincipal = new ClaimsPrincipal(_claimIdentity); //security context is created incide principal
                await HttpContext.SignInAsync("RequiredCookies", _claimsPrincipal); //serealize the claimsprinciple and encrypt it and then save it in cookie as HttpContext
            }
            if (form["PersonalizationCookiesSwitch"] == "on")
            {
                var _claim = new List<Claim>
                {
                    new Claim("personalization_cookies", "on")
                };
                var _claimIdentity = new ClaimsIdentity(_claim, "PersonalizationCookies");
                ClaimsPrincipal _claimsPrincipal = new ClaimsPrincipal(_claimIdentity); 
                await HttpContext.SignInAsync("PersonalizationCookies", _claimsPrincipal); 
            }
            if (form["ThirdPartyCookiesSwitch"] == "on")
            {
                var _claim = new List<Claim>
                {
                    new Claim("thirdparty_cookies", "on")
                };
                var _claimIdentity = new ClaimsIdentity(_claim, "ThirdPartyCookies");
                ClaimsPrincipal _claimsPrincipal = new ClaimsPrincipal(_claimIdentity); 
                await HttpContext.SignInAsync("ThirdPartyCookies", _claimsPrincipal); 
            }
            if (form["RequiredCookiesSwitch"] != "on" && form["PersonalizationCookiesSwitch"] != "on" && form["ThirdPartyCookiesSwitch"] != "on")
            {
                var _claim = new List<Claim>
                {
                    new Claim("rejected_all_cookies", "on")
                };
                var _claimIdentity = new ClaimsIdentity(_claim, "UserRejectedAll");
                ClaimsPrincipal _claimsPrincipal = new ClaimsPrincipal(_claimIdentity);
                await HttpContext.SignInAsync("UserRejectedAll", _claimsPrincipal);

            }

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> RemoveCookies()
        {
            await HttpContext.SignOutAsync("RequiredCookies"); //removes cookie from browser
            await HttpContext.SignOutAsync("PersonalizationCookies");
            await HttpContext.SignOutAsync("ThirdPartyCookies");
            await HttpContext.SignOutAsync("UserRejectedAll");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}