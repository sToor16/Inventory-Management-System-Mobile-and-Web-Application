      using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryManagement.Data;
using InventoryManagement.Models;

namespace InventoryManagement.Controllers
{
    public class FacilitiesController : Controller
    {
        private InventoryManagementContext db = new InventoryManagementContext();

        // GET: Facilities
        public ActionResult Index()
        {

            var EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
            var employee = db.Employees.SingleOrDefault(e => e.Id == EmployeeId);

            if (employee == null)
            {
                return redirectToLogin();
            }

            if (employee != null)
            {
                if (employee.IsAdmin != true) 
                {
                    return View(db.EmplyoeesToFacilities.Where(f => f.EmployeeID == EmployeeId).Select(f => f.Facility)
                        .Where(f => f.IsActive == true).ToList());
                }
                else
                {
                    return View(db.Facilities.Where(f => f.IsActive == true).ToList());
                }
            }

            return View(db.Facilities.ToList());
        }

        // GET: Facilities/Details/5
        public ActionResult Details(int? id)
        {

            var EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
            var employee = db.Employees.SingleOrDefault(e => e.Id == EmployeeId);

            if (employee == null)
            {
                return redirectToLogin();
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facility facility = db.Facilities.Find(id);
            if (facility == null)
            {
                return HttpNotFound();
            }
            return View(facility);
        }

        // GET: Facilities/Create
        public ActionResult Create()
        {

            var EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
            var employee = db.Employees.SingleOrDefault(e => e.Id == EmployeeId);

            if (employee == null)
            {
                return redirectToLogin();
            }

            return View();
        }

        // POST: Facilities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FacilityId,FacilityName,Description,Landmark,Address,City,State,ZipCode")] Facility facility)
        {
            if (ModelState.IsValid)
            {
                db.Facilities.Add(facility);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(facility);
        }

        // GET: Facilities/Edit/5
        public ActionResult Edit(int? id)
        {

            var EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
            var employee = db.Employees.SingleOrDefault(e => e.Id == EmployeeId);

            if (employee == null)
            {
                return redirectToLogin();
            }             

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facility facility = db.Facilities.Find(id);
            if (facility == null)
            {
                return HttpNotFound();
            }
            return View(facility);
        }

        // POST: Facilities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FacilityId,FacilityName,Description,Landmark,Address,City,State,ZipCode")] Facility facility)
        {
            if (ModelState.IsValid)
            {
                db.Entry(facility).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(facility);
        }

        // GET: Facilities/Delete/5
        public ActionResult Delete(int? id)
        {
            var EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
            var employee = db.Employees.SingleOrDefault(e => e.Id == EmployeeId);

            if (employee == null)
            {
                return redirectToLogin();
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facility facility = db.Facilities.Find(id);
            if (facility == null)
            {
                return HttpNotFound();
            }
            return View(facility);
        }

        // POST: Facilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Facility facility = db.Facilities.Find(id);
            if (facility == null)
            {
                return RedirectToAction("Index");
            }

            facility.IsActive = false;
            db.SaveChanges();
            return RedirectToAction("index");

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Browse (int id)
        {

            var EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
            var employee = db.Employees.SingleOrDefault(e => e.Id == EmployeeId);

            if (employee == null)
            {
                return redirectToLogin();
            }

            var facilityResources = db.Facilities.Include("Resources").Single(f => f.FacilityId == id);
            return View(facilityResources);
        }

        [HttpGet]
        public ActionResult EditFacilityResource(int? id)
        {
            if (id == null)
            {
                return redirectToLogin();
            }
            Resource resource = db.Resources.Find(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            ViewBag.FacilityId = new SelectList(db.Facilities, "FacilityId", "FacilityName", resource.FacilityId);
            return View(resource);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFacilityResource([Bind(Include = "ResourceId,ResourceName,Quantity,Description,Size,Color,Changed,PreviousValue,FacilityId")] Resource resource)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resource).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Browse", "Facilities", new { id = resource.FacilityId});
            }
            
            ViewBag.FacilityId = new SelectList(db.Facilities, "FacilityId", "FacilityName", resource.FacilityId);
            return View(resource);

        }
        public ActionResult redirectToLogin()
        {
            return RedirectToAction("Login", "Account");
        }
    }
}
