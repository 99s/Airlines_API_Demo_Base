using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AD_Entities.Models
{
    public partial class Travel
    {
        public long Id { get; set; }
        public string TravelStatusCode { get; set; }
        public string TravelOrigin { get; set; }
        public string TravelDestination { get; set; }
        public DateTime? TravelInititionDate { get; set; }
        public DateTime? TravelCompletetionDate { get; set; }
        public long? TravelTypeFk { get; set; }
        public string TravelerName { get; set; }
        public long? EmployeeCodeFk { get; set; }
    }
}
