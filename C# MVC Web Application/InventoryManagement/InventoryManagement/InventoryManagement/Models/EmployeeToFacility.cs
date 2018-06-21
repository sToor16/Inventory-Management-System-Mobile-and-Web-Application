using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Models
{
    public class EmployeeToFacility
    {

        public int EmployeeToFacilityId { get; set; }
        public int EmployeeID { get; set; }
        public int FacilityId { get; set; }

        public virtual Facility Facility { get; set; }
        public virtual Employee Employee { get; set; }
    }
}