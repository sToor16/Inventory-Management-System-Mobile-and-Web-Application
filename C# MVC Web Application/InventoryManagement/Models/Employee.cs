using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventoryManagement.Models
{
    public class Employee
    {
        [Key]
       
        public int Id { get; set; }
        [Required(ErrorMessage = "You must enter the user name.")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "You must enter the user email.")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; } = true;
        [DataType(DataType.Password)]
        public string Password { get; set; }

        

        public virtual ICollection<Facility> Facilities { get; set; }

    }
}