using System;
using System.Collections.Generic;
using System.Text;

namespace AD_Entities.HelperClasses
{
    public class SessionManager
    {
        // public static IHttpContextAccessor _httpContextAccessor;
        // public static ISession _session;



        public static string LoggedInUserEmail;

        public static long LoggedInUserType;

        public static bool LoggedInUser_IsAdmin;

        public static string SessionConnectionString { get; set; }
        public static bool SessionIsLoggedIn { get; set; }

        //public SessionManager(IHttpContextAccessor httpContextAccessor)
        //{

        //    _httpContextAccessor = httpContextAccessor;

        //    _session = _httpContextAccessor.HttpContext.Session;
        //}

        public static bool ClearSessionManagerData()
        {
            try
            {
                LoggedInUserEmail = string.Empty;
                LoggedInUserType = 0;

                SessionIsLoggedIn = false;
                return true;
            }
            catch (Exception _e) { return false; }

        }
    }
}
