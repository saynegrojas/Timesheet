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
    public class SchedulesController : Controller
    {
        private TimesheetEntities db = new TimesheetEntities();

        // GET: Schedules
        public ActionResult Index()
        {
            var schedules = db.Schedules.Include(s => s.Doctor).Include(s => s.HourCode).Include(s => s.Location).Include(s => s.User);
            return View(schedules.ToList());
        }

        // GET: Schedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // GET: Schedules/Create
        public ActionResult Create()
        {
            ViewBag.FileNumber = new SelectList(db.Doctors, "FileNumber", "FirstName");
            ViewBag.HourCodeId = new SelectList(db.HourCodes, "CodeID", "CodeDescription");
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName");
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ScheduleID,FileNumber,LocationID,UserID,Time_In,Time_Out,HourCodeId,Amount")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Schedules.Add(schedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FileNumber = new SelectList(db.Doctors, "FileNumber", "FirstName", schedule.FileNumber);
            ViewBag.HourCodeId = new SelectList(db.HourCodes, "CodeID", "CodeDescription", schedule.HourCodeId);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", schedule.LocationID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", schedule.UserID);
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.FileNumber = new SelectList(db.Doctors, "FileNumber", "FirstName", schedule.FileNumber);
            ViewBag.HourCodeId = new SelectList(db.HourCodes, "CodeID", "CodeDescription", schedule.HourCodeId);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", schedule.LocationID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", schedule.UserID);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ScheduleID,FileNumber,LocationID,UserID,Time_In,Time_Out,HourCodeId,Amount")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FileNumber = new SelectList(db.Doctors, "FileNumber", "FirstName", schedule.FileNumber);
            ViewBag.HourCodeId = new SelectList(db.HourCodes, "CodeID", "CodeDescription", schedule.HourCodeId);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", schedule.LocationID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", schedule.UserID);
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Schedule schedule = db.Schedules.Find(id);
            db.Schedules.Remove(schedule);
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
