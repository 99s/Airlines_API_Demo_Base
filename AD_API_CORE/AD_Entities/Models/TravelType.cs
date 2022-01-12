using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AD_Entities.Models
{
    public partial class TravelType
    {
        public long Id { get; set; }
        public string TypeCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
