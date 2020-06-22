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
                var userDetail = db.Users.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefault();

                if (userDetail == null)
                {
                    //Error message for incorrect email or password
                    user.LoginErrorMessage = "The Email or Password field is invalid.";
                    return View("Login", user);
                } 
                else
                {
                    //if no action within a time frame, session will redirect to /home/login(condition in view) not users/index
                    Session["Email"] = user.Email;
                    //ModelState.AddModelError("", "Invalid Email & Password");

                    return RedirectToAction("Index", "Users");
                }
                //if (ModelState.IsValid)
                //{
                //    if (user.Email == user.Email && user.Password == user.Password)
                //    {
                //        FormsAuthentication.SetAuthCookie(user.Email, false);
                //        return RedirectToAction("Index", "Users");
                //    }
                //    else if (user.Email != user.Email)
                //    {
                //        ModelState.AddModelError("", "Invalid Email");
                //    }
                //    else if (user.Password != user.Password)
                //    {
                //        ModelState.AddModelError("", "Invalid Password");
                //    }
                //    else
                //    {
                //        Session["Email"] = user.Email;
                //        ModelState.AddModelError("", "Invalid Email & Password");
                //        return RedirectToAction("Index", "Users");
                //    }
                //}
                //return View();
            }
        }
        //[HttpPost]
        //public ActionResult Auth(User user)
        //{
        //    using (TimesheetEntities db = new TimesheetEntities())
        //    {
        //        var userDet = db.Users.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefault();
        //        if (userDet == null)
        //        {
        //            user.LoginErrorMessage = "Invalid Email or Password";
        //            return View("Login", user);
        //        } else
        //        {
        //            Session["UserID"] = user.UserID;
        //            return RedirectToAction("Index", "Users");
        //        }
        //    }
        //    //return RedirectToAction("Index", "Users");
        //}
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }
    }
}