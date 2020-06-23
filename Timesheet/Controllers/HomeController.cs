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
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            using (TimesheetEntities db = new TimesheetEntities())
            {
                //FOR TESTING: INPUT IN USER TABLE - email: user@user.com pw: user jobDes: IT
                var userValues = db.Users.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefault();

                if (userValues == null)
                {
                    //Error message for incorrect email or password
                    user.LoginErrorMessage = "The Email or Password field is invalid.";
                    return View("Login", user);
                }
                else
                {
                    //if no action within a time frame, session will redirect to /home/login(condition in view) not users/index
                    Session["Email"] = user.Email;
                    Session["Name"] = userValues.FirstName;
                    return RedirectToAction("Index", "Users");
                }
            }
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }
    }
}