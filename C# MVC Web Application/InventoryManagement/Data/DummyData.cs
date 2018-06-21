using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using InventoryManagement.Models;

namespace InventoryManagement.Data
{
    public class DummyData
    {
        public static List<Employee> getEmployees(InventoryManagementContext context)
        {
            try
            {
                List<Employee> employees = new List<Employee>()
            {
                new Employee()
                {
                    FirstName = "Shubhpreet",
                    LastName = "Toor",
                    IsAdmin = true,
                    Email = "toor@gmail.com",
                    Password = "password",
                    UserName = "TOOR",
                    IsActive = true,
                    Facilities = new List<Facility>()
                    
                }
            };
                return employees;
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var errors in ex.EntityValidationErrors)
                {
                    foreach (var validationerror in errors.ValidationErrors)
                    {
                        string errorMessage = validationerror.ErrorMessage;
                    }
                }
            }
            return null;
        }
    }
}
