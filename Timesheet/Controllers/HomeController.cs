using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Timesheet.Models;


namespace Timesheet.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                if (user.Email == user.Email && user.UserID == user.UserID)
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    return RedirectToAction("Index", "Users");
                }
                else if (user.Email != user.Email)
                {
                    ModelState.AddModelError("", "Invalid Email");
                }
                else if (user.UserID != user.UserID)
                {
                    ModelState.AddModelError("", "Invalid Password");
                }
                else
                {
                    Session["Email"] = user.Email;
                    ModelState.AddModelError("", "Invalid Email & Password");
                    return RedirectToAction("Index", "Users");
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }
    }
}