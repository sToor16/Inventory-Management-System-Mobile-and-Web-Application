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
    public class ResourcesController : Controller
    {
        private InventoryManagementContext db = new InventoryManagementContext();

        // GET: Resources
        public ActionResult Index()
        {
            var EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
            var employee = db.Employees.SingleOrDefault(e => e.Id == EmployeeId);

            if (employee != null && (string)Session["EmployeeIsAdmin"] == "True")
            {
                var resources = db.Resources.Include(r => r.Facility);
                return View(resources.ToList());
            }


            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: Resources/Details/5
        public ActionResult Details(int? id)
        {

            var EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
            var employee = db.Employees.SingleOrDefault(e => e.Id == EmployeeId);

            if (employee != null && (string)Session["EmployeeIsAdmin"] == "True")
            {
                Resource resource = db.Resources.Find(id);
                return View(resource);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        // GET: Resources/Create
        public ActionResult Create()
        {

            var EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
            var employee = db.Employees.SingleOrDefault(e => e.Id == EmployeeId);

            if (employee != null && (string)Session["EmployeeIsAdmin"] == "True")
            {
                ViewBag.FacilityId = new SelectList(db.Facilities.Where(f => f.IsActive == true), "FacilityId", "FacilityName");
                return View();
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: Resources/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ResourceId,ResourceName,Quantity,Description,Size,Color,FacilityId")] Resource resource)
        {
            resource.PreviousValue = resource.Quantity;
            if (ModelState.IsValid)
            {
                db.Resources.Add(resource);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FacilityId = new SelectList(db.Facilities.Where(f => f.IsActive == true), "FacilityId", "FacilityName", resource.FacilityId);
            return View(resource);
        }

        // GET: Resources/Edit/5
        public ActionResult Edit(int? id)
        {

            var EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
            var employee = db.Employees.SingleOrDefault(e => e.Id == EmployeeId);

            if (employee != null && (string)Session["EmployeeIsAdmin"] == "True")
            {
                Resource resource = db.Resources.Find(id);
                ViewBag.FacilityId = new SelectList(db.Facilities.Where(f => f.IsActive == true), "FacilityId", "FacilityName", resource.FacilityId);
                return View(resource);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: Resources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ResourceId,ResourceName,Quantity,Description,Size,Color,Changed,PreviousValue,FacilityId")] Resource resource)
        {
            if (ModelState.IsValid)
            {
                resource.PreviousValue = resource.Quantity;
                var checkValue = resource.Changed;
                System.Diagnostics.Debug.WriteLine(checkValue);


                db.Entry(resource).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            ViewBag.FacilityId = new SelectList(db.Facilities, "FacilityId", "FacilityName", resource.FacilityId);
            return View(resource);
        }

        // GET: Resources/Delete/5
        public ActionResult Delete(int? id)
        {

            var EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
            var employee = db.Employees.SingleOrDefault(e => e.Id == EmployeeId);

            if (employee != null && (string)Session["EmployeeIsAdmin"] == "True")
            {
                Resource resource = db.Resources.Find(id);
                return View(resource);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
        }

        // POST: Resources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Resource resource = db.Resources.Find(id);
            db.Resources.Remove(resource);
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

        public ActionResult redirectToLogin()
        {
            return RedirectToAction("Login", "Account");
        }
    }
}