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
            if (Request.Cookies["CookiePopup"] != null) //checks the existance of the cookie
            {
                ViewData["HasAcceptedCookies"] = "yes";
                ViewData["RequiredCookies"] = HttpContext.User.Claims.First(c => c.Type == "required_cookies").Value;
                ViewData["PersonalizationCookies"] = HttpContext.User.Claims.First(c => c.Type == "personalization_cookies").Value;
                ViewData["ThirdPartyCookies"] = HttpContext.User.Claims.First(c => c.Type == "thirdparty_cookies").Value;
            }

            return View();
        }

        public async Task<IActionResult> CookieAcceptButton(IFormCollection form)
        {

            string requiredCookiesSwitch = "off";
            string personalizationCookiesSwitch = "off";
            string thirdpartyCookiesSwitch = "off";

            if (form["RequiredCookiesSwitch"] == "on")
            {
                requiredCookiesSwitch = "on";
            }
            if (form["PersonalizationCookiesSwitch"] == "on")
            {
                personalizationCookiesSwitch = "on";
            }
            if (form["ThirdPartyCookiesSwitch"] == "on")
            {
                thirdpartyCookiesSwitch = "on";
            }

            //creating security context using System.Security.Claims and ClaimsPrincipal 
            var claims = new List<Claim>
                {
                    new Claim("required_cookies", requiredCookiesSwitch), //if = "on" then true, if "off" then user did not accept. 
                    new Claim("personalization_cookies", personalizationCookiesSwitch),
                    new Claim("thirdparty_cookies", thirdpartyCookiesSwitch),
                };
            var identity = new ClaimsIdentity(claims, "CookiePopup");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity); //security context is created incide principal

            await HttpContext.SignInAsync("CookiePopup", claimsPrincipal); //serealize the claimsprinciple and encrypt it and then save it in cookie as HttpContext

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> RemoveCookies()
        {
            await HttpContext.SignOutAsync("CookiePopup"); //removes cookie from browser
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