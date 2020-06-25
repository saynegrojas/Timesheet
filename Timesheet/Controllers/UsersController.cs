using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Timesheet.Models;

namespace Timesheet.Controllers
{
    public class UsersController : Controller
    {
        private TimesheetEntities db = new TimesheetEntities();

        public ActionResult Index()
        {
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TimesheetEntities"].ConnectionString);
            //SqlDataReader reader = null;
            //con.Open();
            //if((con.State & System.Data.ConnectionState.Open) > 0)
            //{

      
            //SqlCommand cmd = new SqlCommand("Menu", con);
            //cmd.CommandType = CommandType.StoredProcedure;

            //cmd.Parameters.Add(new SqlParameter("@FilterId", "MenuLocation"));
            //cmd.Parameters.Add(new SqlParameter("@Location", "TopBar"));

            //reader = cmd.ExecuteReader();
            //ViewBag.result = reader.ToString() + "working";

            //}
            var users = db.Users.Include(u => u.Job_Role);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
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
            ViewBag.JobRoleID = new SelectList(db.Job_Role, "JobRoleID", "JobDescription");
            ViewBag.JobDescription = new SelectList(db.Job_Role, "JobRoleID", "JobDescription");

            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,FirstName,LastName,Email,Phone,JobDescription,Password,confirmPassword,JobRoleID")] User user)
        {
            if (ModelState.IsValid)
            {
                //Hashing password 
                user.Password = Crypto.Hash(user.Password);
                //Hash confirm password
                user.confirmPassword = Crypto.Hash(user.confirmPassword);
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JobRoleID = new SelectList(db.Job_Role, "JobRoleID", "JobDescription", user.JobRoleID);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.JobRoleID = new SelectList(db.Job_Role, "JobRoleID", "JobDescription", user.JobRoleID);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,FirstName,LastName,Email,Phone,JobDescription,Password,JobRoleID")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JobRoleID = new SelectList(db.Job_Role, "JobRoleID", "JobDescription", user.JobRoleID);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
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
        public ActionResult DeleteConfirmed(int id)
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

    }
}
