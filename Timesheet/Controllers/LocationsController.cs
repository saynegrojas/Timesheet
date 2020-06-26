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
    public class LocationsController : Controller
    {
        private TimesheetEntities db = new TimesheetEntities();

        // GET: Locations
        public ActionResult Index()
        {
            var locations = db.Locations.Include(l => l.Sector);
            return View(locations.ToList());
        }

        // GET: Locations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // GET: Locations/Create
        public ActionResult Create()
        {
            ViewBag.SectionID = new SelectList(db.Sectors, "SectionID", "SectorName");
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LocationID,LocationName,SectionID")] Location location)
        {
            try
            {
                ViewBag.strSlectedSector = location.SectionID;
                if (ModelState.IsValid)
                {
                    if (!String.IsNullOrEmpty(location.LocationName))
                    {
                        if (!(String.IsNullOrEmpty(location.LocationName) || db.chkLocationNameExist(location.LocationName)))
                        {
                            db.Locations.Add(location);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        else
                            ViewBag.ValLocationName = String.IsNullOrEmpty(location.LocationName) ? "You have not entered a value for Location" : $"There already exist a Location named \"{location.LocationName}\"";
                    }
                    else
                        ViewBag.ValSectorName = "Select a value for Sector Name";
                }
                ViewBag.SectionID = new SelectList(db.Sectors, "SectionID", "SectorName", location.SectionID);
            }
            catch (Exception ex)
            {
                ViewBag.SectionID = new SelectList(db.Sectors, "SectionID", "SectorName", location.SectionID);
                ViewBag.ValSummary = $"An error occured while creating this Location \n{ex.Message}";
            }


            return View(location);
        }

        // GET: Locations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            ViewBag.SectionID = new SelectList(db.Sectors, "SectionID", "SectorName", location.SectionID);
            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LocationID,LocationName,SectionID")] Location location)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var context = new TimesheetEntities())
                    {
                        Location loc = context.Locations.Find(location.LocationID);
                        if (loc != null)
                        {
                            if (!(String.IsNullOrEmpty(location.LocationName) || (context.chkLocationNameExist(location.LocationName) && loc.LocationName != location.LocationName)))
                            {
                                db.Entry(location).State = EntityState.Modified;
                                db.SaveChanges();
                                return RedirectToAction("Index");
                            }
                            else
                                ViewBag.ValLocationName = String.IsNullOrEmpty(location.LocationName) ? "You have not entered a value for Location" : $"There already exist a Location named \"{location.LocationName}\"";
                        }
                        else
                            ViewBag.LocationName = $"This location could not be found in the database";
                    }
                }
                ViewBag.SectionID = new SelectList(db.Sectors, "SectorName", "SectorName", location.SectionID);
            }
            catch (Exception ex)
            {
                ViewBag.SectionID = new SelectList(db.Sectors, "SectorName", "SectorName", location.SectionID);
                ViewBag.ValLocationName = $"An error occured while updating this Location \n{ex.Message}";
            }
            return View(location);
        }

        // GET: Locations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Location location = db.Locations.Find(id);
            db.Locations.Remove(location);
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
