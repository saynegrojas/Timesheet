using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Timesheet.Models
{
    public class DoctorsController : Controller
    {
        private TimesheetEntities db = new TimesheetEntities();
        //Gender selection
        List<Doctor> genderList = new List<Doctor>
        {
            new Doctor { GenderName= "Male", GenderID = 1},
            new Doctor { GenderName= "Female", GenderID = 2}
        };

        // GET: Doctors
        public ActionResult Index()
        {
            var doctors = db.Doctors.Include(d => d.Location);
            return View(doctors.ToList());
        }

        // GET: Doctors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // GET: Doctors/Create
        public ActionResult Create()
        {
            ViewBag.Sex = new SelectList(genderList, "GenderName", "GenderName");
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName");
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,Phone,Email,LocationID,Sex, Address")] Doctor doctor)
        {
            bool errorFlag = false;
            if(String.IsNullOrEmpty(doctor.Email) || db.Doctors.ToList().Any(s => s.Email == doctor.Email))
            {
                ViewBag.ValidateEmail = String.IsNullOrEmpty(doctor.Email) ? "Email field is required" : $"There already exist an email: \"{doctor.Email}\"";
                errorFlag = true;
            }
            if(String.IsNullOrEmpty(doctor.FirstName))
            {
                ViewBag.ValidateFirstName = "First name field is required";
                errorFlag = true;
            }
            if(String.IsNullOrEmpty(doctor.LastName))
            {
                ViewBag.ValidateLastName = "Last name field is required";
                errorFlag = true;
            }

            if (ModelState.IsValid)
            {
                if(!errorFlag)
                {
                    //db.Doctors.Add(doctor);
                    db.Doctors.Add(doctor);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            ViewBag.Sex = new SelectList(genderList, "GenderName", "GenderName", doctor.Sex);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", doctor.LocationID);
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", doctor.LocationID);
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FirstName,LastName,Sex,Phone,Email,DoctorId,LocationID,Address")] Doctor doctor)
        {
            bool errorFlag = false;
            if(String.IsNullOrEmpty(doctor.Email))
            {
                ViewBag.ValidateEmail = "Email address field is required";
            }
            if(String.IsNullOrEmpty(doctor.FirstName))
            {
                ViewBag.ValidateFirstName = "First name field is required";
                errorFlag = true;
            }
            if(String.IsNullOrEmpty(doctor.LastName))
            {
                ViewBag.ValidateLastName = "Last name field is required";
                errorFlag = true;
            }

            if (ModelState.IsValid)
            {
                if(!errorFlag)
                {
                    db.Entry(doctor).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "LocationName", doctor.LocationID);
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            ViewBag.Sex = new SelectList(genderList, "GenderName", "GenderName", doctor.Sex);
            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            db.Doctors.Remove(doctor);
            db.SaveChanges();
            ViewBag.Sex = new SelectList(genderList, "GenderName", "GenderName", doctor.Sex);
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
