using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using Timesheet.Models;

namespace Timesheet.Controllers
{
    public class SchedulesController : Controller
    {
        private TimeSheetEntities db = new TimeSheetEntities();

        // GET: Schedules
        public ActionResult Index()
        {
            var schedules = db.Schedules.Include(s => s.Doctor).Include(s => s.HourCode);
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
        public ActionResult Create(int id)
        {
            Doctor dr = db.Doctors.Single(emp => emp.FileNumber == id);
            TempData["DoctorID"] = id;
            ViewBag.dr = dr;
            ViewBag.DoctorID = new SelectList(db.Doctors, "DoctorId", "DoctorId");
            ViewBag.LocationID = new SelectList(db.Locations, "LocationId", "LocationId");
            ViewBag.HourCodeId = new SelectList(db.HourCodes, "CodeID", "CodeId");
            ViewBag.Amount = new SelectList(db.HourCodes, "CodeID", "CodeValue");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserID");
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ScheduleID,DoctorID,LocationID,UserID,Time_In,Time_Out,HourCodeId,Amount")] Schedule schedule)
        {
            schedule.DoctorID = (int?)TempData["DoctorID"];
            if (ModelState.IsValid)
            {
                db.Schedules.Add(schedule);
                    db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DoctorID = new SelectList(db.Doctors, "DoctorId", "FirstName", schedule.DoctorID);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationId", "LocationName", schedule.LocationID);
            ViewBag.HourCodeId = new SelectList(db.HourCodes, "CodeID", "CodeID", schedule.HourCodeId);
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
            ViewBag.DoctorID = new SelectList(db.Doctors, "DoctorId", "FirstName", schedule.DoctorID);
            ViewBag.HourCodeId = new SelectList(db.HourCodes, "CodeID", "CodeDescription", schedule.HourCodeId);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ScheduleID,DoctorID,LocationID,UserID,Time_In,Time_Out,HourCodeId,Amount")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorID = new SelectList(db.Doctors, "DoctorId", "FirstName", schedule.DoctorID);
            ViewBag.HourCodeId = new SelectList(db.HourCodes, "CodeID", "CodeDescription", schedule.HourCodeId);
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
