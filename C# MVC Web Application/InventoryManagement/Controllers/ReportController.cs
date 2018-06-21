using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using InventoryManagement.Data;
using InventoryManagement.Models;

namespace InventoryManagement.Controllers
{
    public class ReportController : Controller
    {

        InventoryManagementContext db = new InventoryManagementContext();
        // GET: Report
        public ActionResult Index(int? id)
        {
            ViewBag.id = id;

            var EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
            var employee1 = db.Employees.SingleOrDefault(e => e.Id == EmployeeId && e.IsActive == true);
            var Facility = db.Facilities.Find(id);
            if (id != null)
            {
                if ((string)Session["EmployeeIsAdmin"] == "True")
                {
                    ViewBag.FacilityName = new SelectList(db.Facilities.Where(f => f.IsActive), "FacilityName", "FacilityName", Facility.FacilityName);
                    ViewBag.facilityResources = db.Facilities.Include("Resources").Single(f => f.FacilityId == Facility.FacilityId);
                    return View();
                } 
            }
            else if (id == null && employee1 != null && (string)Session["EmployeeIsAdmin"] == "True")
            {
                ViewBag.FacilityName = new SelectList(db.Facilities.Where(f => f.IsActive), "FacilityName", "FacilityName");
                return View();
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        [HttpPost]
        public ActionResult Index(ReportViewModel report)
        {
            var facilityName = report.FacilityName;
            var Facility = db.Facilities.Where(f => f.IsActive == true).SingleOrDefault(f => f.FacilityName == facilityName);
            
            if(Facility == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return RedirectToAction("Index", new { @id =  Facility.FacilityId});
        }

        [HttpGet]
        public ActionResult Acknowledge(int? resourceId)
        {
            var resource = db.Resources.Find(resourceId);
            var EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
            var employee = db.Employees.SingleOrDefault(e => e.Id == EmployeeId && e.IsActive == true);
            
            if (resource != null && employee != null && (string)Session["EmployeeIsAdmin"] == "True")
            {
                resource.Changed = false;
                resource.PreviousValue = resource.Quantity;
                db.SaveChanges();
                return RedirectToAction("Index", new {id = resource.FacilityId});
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
    }
}