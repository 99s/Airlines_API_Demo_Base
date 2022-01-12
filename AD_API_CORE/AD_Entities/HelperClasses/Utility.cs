using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using AD_Entities.Models;

namespace AD_Entities.HelperClasses
{
    public class Utility
    {
    }

    public class DefaultResponse
    {
        public bool Status { get; set; }

        public dynamic Data { get; set; }
        public List<DefaultKVP> DataList {get ; set;}

        public string Message { get; set; }

    }

    public class DefaultKVP
    {
        public dynamic Key { get; set; }
        public dynamic Value { get; set; }

    }


    public class LoginAPIModel 
    {
        [Required]
        public string UserEMAIL { get; set; }
        [Required]
        public string PassWord { get; set; }

        public string Token { get; set; }

        public bool IsAdmin { get; set; }

        public string id_afterlogin { get; set; }

    }

    public class RegisterAPIModel 
    {
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEMAIL { get; set; }
        public string PassWord { get; set; }

        public bool IsAdmin { get; set; }

        //forupdate
        public int Id_forupdate { get; set; }

        public string Password_forupdate { get; set; }

    }

    public class TravelsRegisterAPIModel
    {
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
