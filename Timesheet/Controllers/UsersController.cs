using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using Timesheet.Models;

namespace Timesheet.Controllers
{
    public class UsersController : Controller
    {
        private TimesheetEntities db = new TimesheetEntities();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Job_Role);
            return View(users.ToList());
        }
        //[HttpPost]
        //public ActionResult Index(User user)
        //{
        //    using (TimesheetEntities db = new TimesheetEntities());
        //    var userDetail = db.Users.Where(x => x.FirstName == user.FirstName).FirstOrDefault();

        //    if (userDetail == null)
        //    {
        //        return View();
        //    }
        //    else
        //    {
        //        user.DisplayName = user.FirstName;
        //        return View("Index", user);
        //    }
        //}

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.JobDescription = new SelectList(db.Job_Role, "JobTitle", "JobDescription");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,FirstName,LastName,Email,Phone,JobDescription,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                //Hashing password 
                user.Password = Crypto.Hash(user.Password);
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JobDescription = new SelectList(db.Job_Role, "JobTitle", "JobDescription", user.JobDescription);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobDescription = new SelectList(db.Job_Role, "JobTitle", "JobDescription", user.JobDescription);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,FirstName,LastName,Email,Phone,JobDescription,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JobDescription = new SelectList(db.Job_Role, "JobTitle", "JobDescription", user.JobDescription);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Login 
        //[ValidateAntiForgeryToken]
        //public ActionResult Login()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Login(User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //       if(user.Email == user.Email && user.UserID == user.UserID)
        //        {
        //            FormsAuthentication.SetAuthCookie(user.Email, false);
        //            return RedirectToAction("Index", "Users");
        //        } else if (user.Email != user.Email)
        //        {
        //            ModelState.AddModelError("", "Invalid Email");
        //        } else if(user.UserID != user.UserID)
        //        {
        //            ModelState.AddModelError("", "Invalid Password");
        //        } else
        //        {
        //            ModelState.AddModelError("", "Invalid Email & Password");
        //        }
        //    }
        //    return View();
        //}
    }
}
