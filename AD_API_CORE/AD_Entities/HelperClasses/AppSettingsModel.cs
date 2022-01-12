using System;
using System.Collections.Generic;
using System.Text;

namespace AD_Entities.HelperClasses
{


    public class LogLevel
    {
        public string Default { get; set; }
        public string Microsoft { get; set; }
        //public string Microsoft.Hosting.Lifetime { get; set; }

    }
    public class Logging
    {
        public LogLevel LogLevel { get; set; }

    }
    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }

    }
    public class Config
    {
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public string UseSwaggerUi { get; set; }
        public string AzureStorageConnectionString { get; set; }
        public string DefaultConnection { get; set; }
      

    }
    public class AppSettingsModel
    {
        public Logging Logging { get; set; }
        public string AllowedHosts { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public Config Config { get; set; }

    }
}
