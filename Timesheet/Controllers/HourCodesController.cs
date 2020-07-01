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
    public class HourCodesController : Controller
    {
        private TimesheetEntities db = new TimesheetEntities();

        // GET: HourCodes
        public ActionResult Index(string searchString)
        {
            return View(db.HourCodes.Where(x => x.CodeDescription.Contains(searchString) || searchString == null).ToList());
        }

        // GET: HourCodes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HourCode hourCode = db.HourCodes.Find(id);
            if (hourCode == null)
            {
                return HttpNotFound();
            }
            return View(hourCode);
        }

        // GET: HourCodes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HourCodes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodeID,CodeDescription,CodeValue")] HourCode hourCode)
        {
            bool errorFlag = false;
            if(String.IsNullOrEmpty(hourCode.CodeDescription) || db.HourCodes.ToList().Any(s => s.CodeDescription == hourCode.CodeDescription))
            {
                ViewBag.ValidateHourCode = String.IsNullOrEmpty(hourCode.CodeDescription) ? "Code description field is required" : $"\"There already exists code description: {hourCode.CodeDescription}\"";
                errorFlag = true;
            }
            if(String.IsNullOrEmpty(hourCode.CodeValue.ToString()))
            {
                ViewBag.ValidateCodeValue = "Code value field is required";
                errorFlag = true;
            }
            if (ModelState.IsValid)
            {
                if(!errorFlag)
                {
                db.HourCodes.Add(hourCode);
                db.SaveChanges();
                return RedirectToAction("Index");
                }
            }

            return View(hourCode);
        }

        // GET: HourCodes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HourCode hourCode = db.HourCodes.Find(id);
            if (hourCode == null)
            {
                return HttpNotFound();
            }
            return View(hourCode);
        }

        // POST: HourCodes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodeID,CodeDescription,CodeValue")] HourCode hourCode)
        {
            bool errorFlag = false;
            if (String.IsNullOrEmpty(hourCode.CodeDescription))
            {
                ViewBag.ValidateHourCode = "Code description field is required";
                errorFlag = true;
            }
            if (String.IsNullOrEmpty(hourCode.CodeValue.ToString()))
            {
                ViewBag.ValidateCodeValue = "Code value field is required";
                errorFlag = true;
            }
            if (ModelState.IsValid)
            {
                if(!errorFlag)
                {
                    db.Entry(hourCode).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(hourCode);
        }

        // GET: HourCodes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HourCode hourCode = db.HourCodes.Find(id);
            if (hourCode == null)
            {
                return HttpNotFound();
            }
            return View(hourCode);
        }

        // POST: HourCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HourCode hourCode = db.HourCodes.Find(id);
            db.HourCodes.Remove(hourCode);
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
