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
        public ActionResult Index()
        {
            return View(db.Job_Role.ToList());
        }

        // GET: Job_Role/Details/5
        public ActionResult Details(string id)
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
        public ActionResult Create([Bind(Include = "JobTitle,JobDescription")] Job_Role job_Role)
        {
            if (ModelState.IsValid)
            {
                db.Job_Role.Add(job_Role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(job_Role);
        }

        // GET: Job_Role/Edit/5
        public ActionResult Edit(string id)
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
        public ActionResult Edit([Bind(Include = "JobTitle,JobDescription")] Job_Role job_Role)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job_Role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(job_Role);
        }

        // GET: Job_Role/Delete/5
        public ActionResult Delete(string id)
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
        public ActionResult DeleteConfirmed(string id)
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
