using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Helpers
{
    public enum RoleEnum
    {
        [Display(Name = "Admin")]
        Admin = 1,
        [Display(Name = "Employee")]
        Employee = 2,
        [Display(Name = "Manager")]
        Manager = 3,
    }
}
