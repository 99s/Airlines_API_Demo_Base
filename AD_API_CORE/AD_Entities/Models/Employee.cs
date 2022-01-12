using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AD_Entities.Models
{
    public partial class Employee
    {
        public long Id { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeLastName { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeePasswordHash { get; set; }
        public bool? IsAdmin { get; set; }
    }
}
