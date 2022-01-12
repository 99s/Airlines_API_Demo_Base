using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace AD_Entities.HelperClasses
{
    public class ConfigWrapper
    {
        private readonly IConfiguration _config;

        public ConfigWrapper(IConfiguration config)
        {
            _config = config;
        }

        public string JwtKey
        {
            get { return _config["Config:JwtKey"]; }
        }

        public string JwtIssuer
        {
            get { return _config["Config:JwtIssuer"]; }
        }

        public string DefaultConnection
        {
            get { return _config["Config:DefaultConnection"]; }
        }

       

    }
}
