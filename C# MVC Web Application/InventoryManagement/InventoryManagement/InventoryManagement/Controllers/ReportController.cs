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

            /*TempData["valueChanged"] = "Resource quantity updated from last time, New value is " + resource.Quantity;*/
            ViewBag.id = id;
            if (id != null)
            {
                var Facility = db.Facilities.Find(id);
                ViewBag.FacilityName = new SelectList(db.Facilities.Where(f => f.IsActive), "FacilityName", "FacilityName", Facility.FacilityName);
                ViewBag.facilityResources = db.Facilities.Include("Resources").Single(f => f.FacilityId == Facility.FacilityId);
            }
            else
            {
                ViewBag.FacilityName = new SelectList(db.Facilities.Where(f => f.IsActive), "FacilityName", "FacilityName");
            }
            
            return View();
        }
        [HttpPost]
        public ActionResult Index(ReportViewModel report)
        {
            var facilityName = report.FacilityName;
            var Facility = db.Facilities.SingleOrDefault(f => f.FacilityName == facilityName);
            if(Facility == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return RedirectToAction("Index", new { @id =  Facility.FacilityId});
        }
    }
}