using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CircleCat.MicrosoftEntraID.AZ204.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected string UserName
        {
            get
            {
                var email = HttpContext.User.Identity.Name;
                var userName = email.Split('@')[0];
                return userName;
            }
        }
    }
}
