using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryManagement.Data;
using InventoryManagement.Models;

namespace InventoryManagement.Controllers
{
    public class EmployeesController : Controller
    {
        private InventoryManagementContext db = new InventoryManagementContext();

        // GET: Employees
        public ActionResult Index()
        {

            var EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
            var employee = db.Employees.SingleOrDefault(e => e.Id == EmployeeId && e.IsActive == true);

            //employee.Facilities( f => f.)

            if (employee != null && (string)Session["EmployeeIsAdmin"] == "True")
            {
                return View(db.Employees.Where(e => e.IsActive == true).ToList());
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            //employee - id!=null
            //employee -- is Active
            //User is Admin
            if ((string)Session["EmployeeIsAdmin"] == "True")
            {
                Employee employee = db.Employees.Include(e => e.Facilities).Where(i => i.Id == id && i.IsActive==true).SingleOrDefault();
                if (employee != null)
                {
                    return View(employee);
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {

            var EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
            var employee = db.Employees.SingleOrDefault(e => e.Id == EmployeeId);

            if (employee != null && (string)Session["EmployeeIsAdmin"] == "True")
            {
                var newEmployee = new Employee();
                newEmployee.IsActive = true;
                newEmployee.Facilities = new List<Facility>();
                PopulateAssignedFacilityData(newEmployee);
                return View();
            }

            return View();
            //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName,FirstName,LastName,Email,IsAdmin,Password")] Employee employee, string[] selectedFacilities)
        {
            List<String> alreadyCreated = db.Employees.Where(u => u.IsActive).Select(u => u.Email).ToList();

            if (alreadyCreated.Contains(employee.Email.ToLower()))
            {
                TempData["Already Registered"] = "disable";
                return RedirectToAction("Create");
               
            }
            if (selectedFacilities != null)
            {
                employee.Facilities = new List<Facility>();
                foreach (var bat in selectedFacilities)
                {
                    var batToAdd = db.Facilities.Find(int.Parse(bat));
                    employee.Facilities.Add(batToAdd);
                }
            }
            if (ModelState.IsValid)
            {
                employee.Email = employee.Email.ToLower();
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateAssignedFacilityData(employee);
            return View(employee);
        }



        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {

            var EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
            var employee1 = db.Employees.SingleOrDefault(e => e.Id == EmployeeId);

            if (employee1 != null && (string)Session["EmployeeIsAdmin"] == "True")
            {
                Employee employee = db.Employees.Include(e => e.Facilities).Where(i => i.Id == id && i.IsActive == true).SingleOrDefault();
                PopulateAssignedFacilityData(employee);
                return View(employee);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
            
    

    // POST: Employees/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int? id, string[] selectedFacilities)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        var employeeToUpdate = db.Employees.Include(e => e.Facilities) .Where(i => i.Id == id && i.IsActive == true).SingleOrDefault();
        if (TryUpdateModel(employeeToUpdate, "",
            new string[] { "Id", "UserName", "FirstName", "LastName", "Email", "IsAdmin", "Password" }))
        {
            try
            {
                UpdateEmployeeFacilities(selectedFacilities, employeeToUpdate);

                db.Entry(employeeToUpdate).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
        }
        
        PopulateAssignedFacilityData(employeeToUpdate);
        return View(employeeToUpdate);
        }

    // GET: Employees/Delete/5
    public ActionResult Delete(int? id)
    {

            var EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
            var employee1 = db.Employees.SingleOrDefault(e => e.Id == EmployeeId && e.IsActive == true);

            if (employee1 != null && (string)Session["EmployeeIsAdmin"] == "True")
            {
                Employee employee = db.Employees.Include(e => e.Facilities).Where(i => i.Id == id && i.IsActive == true).SingleOrDefault();
                if(employee != null)
                {
                    return View(employee);
                }
                
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

    // POST: Employees/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        Employee employee = db.Employees.Find(id);
            employee.IsActive = false;
        db.SaveChanges();
        return RedirectToAction("Index");
    }

        private void PopulateAssignedFacilityData(Employee employee)
        {
            var allFacilities = db.Facilities.Where(f=>f.IsActive == true);
            var employeeToFacility = new HashSet<int>(employee.Facilities.Where(f=>f.IsActive == true).Select(f => f.FacilityId));
            var viewModel = new List<DropDownViewModel>();
            

            foreach (var facility in allFacilities)
            {
                viewModel.Add(new DropDownViewModel
                {
                    Id = facility.FacilityId,
                    Name = facility.FacilityName,
                    Selected = employeeToFacility.Contains(facility.FacilityId)
                });
            }
            ViewBag.Facilities = viewModel;
        }

            protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Employees/Edit/5
        public ActionResult EditLoggedIn()
        {

            var EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
            var employee1 = db.Employees.SingleOrDefault(e => e.Id == EmployeeId);
            var employeeId = (string)Session["EmployeeID"];
            var id = Convert.ToInt32(employeeId);
            if (employee1 != null && ((string)Session["EmployeeIsAdmin"] == "True" || (string)Session["EmployeeIsAdmin"] != "True"))
            {
                Employee employee = db.Employees.Include(e => e.Facilities).Where(i => i.Id == id && i.IsActive == true).SingleOrDefault();
                PopulateAssignedFacilityData(employee);
                return View(employee);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }



        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditLoggedIn(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var employeeToUpdate = db.Employees.Include(e => e.Facilities).Where(i => i.Id == id && i.IsActive == true).SingleOrDefault();
            if (TryUpdateModel(employeeToUpdate, "",
                new string[] { "Id", "UserName", "FirstName", "LastName", "Email", "IsAdmin", "Password" }))
            {
                try
                {
                    db.Entry(employeeToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index", "Facilities");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            PopulateAssignedFacilityData(employeeToUpdate);
            return View(employeeToUpdate);
        }

        public ActionResult redirectToLogin()
        {
            return RedirectToAction("Login", "Account");
        }

        private void UpdateEmployeeFacilities(string[] selectedFacilities, Employee employeeToUpdate)
        {
            if (selectedFacilities == null)
            {
                employeeToUpdate.Facilities = new List<Facility>();
                return;
            }

            var selectedFacilitiesHashSet = new HashSet<string>(selectedFacilities);
            var employeeFacilities = new HashSet<int>
                (employeeToUpdate.Facilities.Select(f => f.FacilityId));
            foreach (var facility in db.Facilities.Where(f=>f.IsActive==true))
            {
                if (selectedFacilitiesHashSet.Contains(facility.FacilityId.ToString()))
                {
                    if (!employeeFacilities.Contains(facility.FacilityId))
                    {
                        employeeToUpdate.Facilities.Add(facility);
                    }
                }
                else
                {
                    if (employeeFacilities.Contains(facility.FacilityId))
                    {
                        employeeToUpdate.Facilities.Remove(facility);
                    }
                }
            }
        }

    }
}
