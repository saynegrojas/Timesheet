using PagedList;
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
using PagedList.Mvc;

namespace Timesheet.Controllers
{
    public class SchedulesController : Controller
    {
        private TimesheetEntities db = new TimesheetEntities();

        // GET: Schedules
        public ActionResult Index(string searchBy, string search, int? page, string sortBy)
        {
            ViewBag.SortNameParameter = string.IsNullOrEmpty(sortBy) ? "Name asc" : "";
            ViewBag.SortFileNumberParameter = string.IsNullOrEmpty(sortBy) ? "FileNumber" : "";

            var schedules = db.Schedules.Include(s => s.Doctor).Include(s => s.HourCode).AsQueryable();

            if (searchBy == "FileNumber")
            {
                int id = Convert.ToInt32(search);
                schedules = schedules.Where(x => x.Doctor.FileNumber == id || search == null);
            }
            else 
            {
                schedules = schedules.Where(x => x.Doctor.FirstName.StartsWith(search) || search == null);
            } 

            switch (sortBy)
            {
                case "Name asc":
                    schedules = schedules.OrderBy(x => x.Doctor.FirstName);
                    break;
                case "FileNumber":
                    schedules = schedules.OrderBy(x => x.Doctor.FileNumber);
                    break;
                //default:
                //    schedules = schedules.OrderBy(x => x.Doctor.FirstName);
                //    break;
            }
            return View(schedules.ToList().ToPagedList(page ?? 1, 5));
        }

        public ActionResult Calendar()
        {
            //ViewBag.SortNameParameter = string.IsNullOrEmpty(sortBy) ? "Name asc" : "";
            //ViewBag.SortFileNumberParameter = string.IsNullOrEmpty(sortBy) ? "FileNumber" : "";

            //var schedules = db.Schedules.Include(s => s.Doctor).Include(s => s.HourCode).AsQueryable();

            //if (searchBy == "FileNumber")
            //{
            //    int id = Convert.ToInt32(search);
            //    schedules = schedules.Where(x => x.Doctor.FileNumber == id || search == null);
            //}
            //else
            //{
            //    schedules = schedules.Where(x => x.Doctor.FirstName.StartsWith(search) || search == null);
            //}

            //switch (sortBy)
            //{
            //    case "Name asc":
            //        schedules = schedules.OrderBy(x => x.Doctor.FirstName);
            //        break;
            //    case "FileNumber":
            //        schedules = schedules.OrderBy(x => x.Doctor.FileNumber);
            //        break;
            //}
            return View();
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
            Doctor dr = db.Doctors.Single(emp => emp.DoctorId == id);
            TempData["DoctorID"] = dr.DoctorId;
            ViewBag.dr = dr;
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName");
            ViewBag.HourCodeId = new SelectList(db.HourCodes, "CodeID", "CodeDescription");
            ViewBag.Amount = new SelectList(db.HourCodes, "CodeID", "CodeValue");
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LocationID,HourCodeId")] Schedule schedule, FormCollection data)
        {
            int hi = Convert.ToInt32(data["HourCodeId"]);
            HourCode hr = db.HourCodes.Single(emp => emp.CodeID == hi);
            string timein = data["Date"] + " " + data["Time_In"];
            string timeout = data["Date"] + " " + data["Time_Out"];
            schedule.Time_In = DateTime.ParseExact(timein, "yyyy-MM-dd HH:mm", null);
            schedule.Time_Out = DateTime.ParseExact(timeout, "yyyy-MM-dd HH:mm", null);
            schedule.DoctorID = Convert.ToInt32(TempData["DoctorID"]);
            schedule.Amount = hr.CodeValue;
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