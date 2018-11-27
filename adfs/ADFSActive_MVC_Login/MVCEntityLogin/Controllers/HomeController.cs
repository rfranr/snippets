using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using MVCEntityLogin.Models;

namespace MVCEntityLogin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    IEnumerable<Claim> claims = MVCEntityLogin.Authentication.Adfs.Login.Validate(login.UserName, login.Password);
                    foreach (Claim claim in claims)
                    {
                        ViewBag.Claims += "\n----------------------------------------------" + "\n";
                        ViewBag.Claims += "Type =" + claim.Type + "\n";
                        ViewBag.Claims += "Value =" + claim.Value + "\n";
                        ViewBag.Claims += "ValueType =" + claim.ValueType + "\n";
                        ViewBag.Claims += "Issuer =" + claim.Issuer + "\n";
                        ViewBag.Claims += "OriginalIssuer =" + claim.OriginalIssuer + "\n";
                        ViewBag.Claims += "Properties =" + claim.Properties + "\n";
                        ViewBag.Claims += "Subject =" + claim.Subject + "\n";
                    }
                }
                catch ( Exception ex)
                {
                    //ModelState.AddModelError("", "Invalid login credentials.");
                    ViewBag.Claims = ex.Message;
                }
            }
            return View(login);
        }

        public ActionResult WelcomePage()
        {
            return View();
        }
    }
}
