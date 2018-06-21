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
                return View(employee.Facilities.Where(f => f.IsActive == true).ToList());
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: Facilities/Details/5
        public ActionResult Details(int? id)
        {

            var EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
            var employee = db.Employees.SingleOrDefault(e => e.Id == EmployeeId);

            if (employee != null)
            {
                Facility facility = db.Facilities.Find(id);
                return View(facility);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: Facilities/Create
        public ActionResult Create()
        {

            var EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
            var employee = db.Employees.SingleOrDefault(e => e.Id == EmployeeId);

            if (employee != null && (string)Session["EmployeeIsAdmin"] == "True")
            {
                return View();
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
                var admins = db.Employees.Where(e => e.IsAdmin == true && e.IsActive == true).ToList();
                foreach(var admin in admins)
                {
                    admin.Facilities.Add(facility);
                }
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

            if (employee != null && (string)Session["EmployeeIsAdmin"] == "True")
            {
                Facility facility = db.Facilities.Find(id);
                return View(facility);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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

            if (employee != null && (string)Session["EmployeeIsAdmin"] == "True")
            {
                Facility facility = db.Facilities.Find(id);
                if (facility.Resources.Count == 0)
                {
                    TempData["Disable Delete"] = "disable";
                }
                return View(facility);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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

            if (employee != null)
            {
                var facilityResources = db.Facilities.Include("Resources").Single(f => f.FacilityId == id);
                return View(facilityResources);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        public ActionResult EditFacilityResource(int? id)
        {
            if (id != null)
            {
                Resource resource = db.Resources.Find(id);

                ViewBag.FacilityId = new SelectList(db.Facilities, "FacilityId", "FacilityName", resource.FacilityId);
                return View(resource);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFacilityResource([Bind(Include = "ResourceId,ResourceName,Quantity,Description,Size,Color,Changed,PreviousValue,FacilityId")] Resource resource)
        {
            if (ModelState.IsValid)
            {
                string forStringManipulatoin = resource.PreviousValue;
                string[] parts = forStringManipulatoin.Split(',');

                string valueToCheck = parts[parts.Length-1];
                if (valueToCheck != resource.Quantity)
                {
                    resource.PreviousValue = resource.PreviousValue + "," + resource.Quantity;
                    resource.Changed = true;
                }


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
