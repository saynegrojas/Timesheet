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
    public class SectorsController : Controller
    {
        private TimesheetEntities db = new TimesheetEntities();

        // GET: Sectors
        public ActionResult Index(string searchString)
        {
            return View(db.Sectors.Where(x => x.SectorName.Contains(searchString) || searchString == null).ToList());
        }

        // GET: Sectors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sector sector = db.Sectors.Find(id);
            if (sector == null)
            {
                return HttpNotFound();
            }
            return View(sector);
        }

        // GET: Sectors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sectors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SectorName,SectionID")] Sector sector)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!(String.IsNullOrEmpty(sector.SectorName) || db.chkSectorNameExist(sector.SectorName)))
                    {
                        db.Sectors.Add(sector);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                        ViewBag.ErrorValidation = String.IsNullOrEmpty(sector.SectorName) ? "You have not entered a value for Sector" : $"There already exist a Sector named \"{sector.SectorName}\"";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorValidation = $"An error occured while creating this Sector \n{ex.Message}";
            }

            return View(sector);
        }

        // GET: Sectors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sector sector = db.Sectors.Find(id);
            if (sector == null)
            {
                return HttpNotFound();
            }
            return View(sector);
        }

        // POST: Sectors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SectorName,SectionID")] Sector sector, string oldSectorName)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var context = new TimesheetEntities())
                    {
                        Sector sec = context.Sectors.Find(sector.SectionID);
                        if (sec != null)
                        {
                            if (!(String.IsNullOrEmpty(sector.SectorName) || (context.chkSectorNameExist(sector.SectorName) && sec.SectorName != sector.SectorName)))
                            {
                                db.Entry(sector).State = EntityState.Modified;
                                db.SaveChanges();
                                //db.updateSector(oldSectorName, sector.SectorName);
                                return RedirectToAction("Index");
                            }
                            else
                                ViewBag.ErrorValidation = String.IsNullOrEmpty(sector.SectorName) ? "You have not entered a value for Sector" : $"There already exist a Sector named \"{sector.SectorName}\"";
                        }
                        else
                            ViewBag.ErrorValidation = $"This sector ID {sector.SectionID} could not be found in the database";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorValidation = $"An error occured during update {ViewBag.oldSectorName}. ({ex.Message})";
            }
            return View(sector);
        }

        // GET: Sectors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sector sector = db.Sectors.Find(id);
            if (sector == null)
            {
                return HttpNotFound();
            }
            return View(sector);
        }

        // POST: Sectors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sector sector = db.Sectors.Find(id);
            db.Sectors.Remove(sector);
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
