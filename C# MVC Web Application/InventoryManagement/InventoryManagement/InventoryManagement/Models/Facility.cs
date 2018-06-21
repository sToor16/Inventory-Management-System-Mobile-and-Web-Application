using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventoryManagement.Models
{
    public class Facility
    {

        /*update-Database -TargetMigration $InitialDatabase*/

        [Key]
        public int FacilityId { get; set; }
        [Required]
        public string FacilityName { get; set; }
        [Required]
        public string Description { get; set; }

        public string Landmark { get; set; }
        
        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(6)]
        [RegularExpression("([1-9][0-9]*)")]
        public string ZipCode { get; set; }

        public virtual ICollection<Resource> Resources { get; set; }
        public virtual ICollection<EmployeeToFacility> EmployeeToFacility { get; set; }

    }


}
 