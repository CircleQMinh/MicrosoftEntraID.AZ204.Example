using CircleCat.MicrosoftEntraID.AZ204.Helpers;
using CircleCat.MicrosoftEntraID.AZ204.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CircleCat.MicrosoftEntraID.AZ204.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(AppPolicy.Admin)]
        public IActionResult AdminOnly()
        {
            return View();
        }
        [Authorize(AppPolicy.HighLevelUserOnly)]
        public IActionResult HigherLevelOnly()
        {
            return View();
        }
        public IActionResult UserInfo()
        {
            ViewBag.Username = UserName;
            ViewBag.Roles = RoleBasePolicyHepler.GetUserRolesFromClaim(User);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}