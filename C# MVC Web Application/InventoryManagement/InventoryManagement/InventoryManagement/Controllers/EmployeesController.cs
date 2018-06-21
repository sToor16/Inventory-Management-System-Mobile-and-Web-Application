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
    public class EmployeesController : Controller
    {
        private InventoryManagementContext db = new InventoryManagementContext();

        // GET: Employees
        public ActionResult Index()
        {

            var EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
            var employee = db.Employees.SingleOrDefault(e => e.Id == EmployeeId);

            if (employee == null)
            {
                return redirectToLogin();
            }

            return View(db.Employees.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return redirectToLogin();
            }

            var Result = from f in db.Facilities
                         select new
                         {
                             f.FacilityId,
                             f.FacilityName,
                             Selected = ((from ab in db.EmplyoeesToFacilities
                                          where (ab.EmployeeID == id) & (ab.FacilityId == f.FacilityId)
                                          select ab).Count() > 0)

                         };
            var myViewModel = new EmployeeViewModel()
            {
                EmployeeId = id.Value,
                UserName = employee.UserName,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                IsAdmin = employee.IsAdmin,
                Password = employee.Password
            };

            var myDropDownList = new List<DropDownViewModel>();
            foreach (var item in Result)
            {
                myDropDownList.Add(new DropDownViewModel { Id = item.FacilityId, Name = item.FacilityName, Selected = item.Selected });
            }

            myViewModel.Facilities = myDropDownList;

            return View(myViewModel);
        }

        // GET: Employees/Create
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

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName,FirstName,LastName,Email,IsAdmin,Password")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {

            var EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
            var employee1 = db.Employees.SingleOrDefault(e => e.Id == EmployeeId);

            if (employee1 == null)
            {
                return redirectToLogin();
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

            var Result = from f in db.Facilities
                         select new
                         {
                             f.FacilityId,
                             f.FacilityName,
                             Selected = ((from ab in db.EmplyoeesToFacilities
                                          where (ab.EmployeeID == id) & (ab.FacilityId == f.FacilityId)
                                          select ab).Count() > 0)

                         };
            var myViewModel = new EmployeeViewModel()
            {
                EmployeeId = id.Value,
                UserName = employee.UserName,
                FirstName = employee.FirstName,
               LastName = employee.LastName,
               Email = employee.Email,
               IsAdmin = employee.IsAdmin,
               Password = employee.Password
            };

            var myDropDownList = new List<DropDownViewModel>();
            foreach (var item in Result)
            {
                myDropDownList.Add(new DropDownViewModel { Id = item.FacilityId, Name = item.FacilityName, Selected = item.Selected });
            }

            myViewModel.Facilities = myDropDownList;
            return View(myViewModel);

        }
            
    

    // POST: Employees/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(EmployeeViewModel employee)
    {
            if (ModelState.IsValid)
            {
                var myEmployee = db.Employees.Find(employee.EmployeeId);
                myEmployee.UserName = employee.UserName;
                myEmployee.FirstName = employee.FirstName;
                myEmployee.LastName = employee.LastName;
                myEmployee.Email = employee.Email;
                myEmployee.IsAdmin = employee.IsAdmin;
                myEmployee.Password = employee.Password;

            foreach (var item in db.EmplyoeesToFacilities)
            {
                if(item.EmployeeID == employee.EmployeeId)
                {
                    db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
            }
            foreach (var item in employee.Facilities)
            {
                if (item.Selected)
                {
                    db.EmplyoeesToFacilities.Add(new EmployeeToFacility() { EmployeeID = employee.EmployeeId, FacilityId = item.Id });
                }
            }
            
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(employee);
    }

    // GET: Employees/Delete/5
    public ActionResult Delete(int? id)
    {

            var EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
            var employee1 = db.Employees.SingleOrDefault(e => e.Id == EmployeeId);

            if (employee1 == null)
            {
                return redirectToLogin();
            }

            if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        Employee employee = db.Employees.Find(id);
        if (employee == null)
        {
            return HttpNotFound();
        }
        return View(employee);
    }

    // POST: Employees/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        Employee employee = db.Employees.Find(id);
        db.Employees.Remove(employee);
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
