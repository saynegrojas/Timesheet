using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Timesheet.Models;

namespace Timesheet.Controllers
{
    public class Job_RoleController : Controller
    {
        private TimesheetEntities db = new TimesheetEntities();

        // GET: Job_Role
        public ActionResult Index(string searchString)
        {
            return View(db.Job_Role.Where(x => x.JobDescription.Contains(searchString) || searchString == null).ToList());
        }

        // GET: Job_Role/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job_Role job_Role = db.Job_Role.Find(id);
            if (job_Role == null)
            {
                return HttpNotFound();
            }
            return View(job_Role);
        }

        // GET: Job_Role/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Job_Role/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobRoleID,JobDescription")] Job_Role job_Role)
        {
            bool errorFlag = false;
            if(String.IsNullOrEmpty(job_Role.JobDescription) || db.Job_Role.ToList().Any(s => s.JobDescription == job_Role.JobDescription))
            {
                ViewBag.ValidateJobRole = String.IsNullOrEmpty(job_Role.JobDescription) ? "Job role field is required" : $"Job role \"{job_Role.JobDescription}\" already exists";
                errorFlag = true;
            }
            
            if (ModelState.IsValid)
            {
                if(!errorFlag)
                {
                    db.Job_Role.Add(job_Role);
                    db.SaveChanges();
                    return RedirectToAction("Index");   
                }

            } 
                return View(job_Role);

        }

        // GET: Job_Role/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job_Role job_Role = db.Job_Role.Find(id);
            if (job_Role == null)
            {
                return HttpNotFound();
            }
            return View(job_Role);
        }

        // POST: Job_Role/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobRoleID,JobDescription")] Job_Role job_Role)
        {
            bool errorFlag = false;
            if(String.IsNullOrEmpty(job_Role.JobDescription))
            {
                ViewBag.ValidateJobRole = "Job description field is required";
                errorFlag = true;
            }
            if (ModelState.IsValid)
            {
                if(!errorFlag)
                {
                db.Entry(job_Role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
                }
            }
            return View(job_Role);
        }

        // GET: Job_Role/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job_Role job_Role = db.Job_Role.Find(id);
            if (job_Role == null)
            {
                return HttpNotFound();
            }
            return View(job_Role);
        }

        // POST: Job_Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Job_Role job_Role = db.Job_Role.Find(id);
            db.Job_Role.Remove(job_Role);
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
