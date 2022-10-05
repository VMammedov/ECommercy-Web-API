using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommercialAPI.Persistance
{
    static class Configuration
    {
        public static string ConnectionString
        {
            get
            {
                ConfigurationManager configurationManager = new(); // Microsoft.Extensions.Configuration package
                configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../E-CommercialAPI/E-CommercialAPI.API")); // Microsoft.Extensions.Configuration.Json package
                configurationManager.AddJsonFile("appsettings.json"); // Microsoft.Extensions.Configuration.Json package

                return configurationManager.GetConnectionString("PostgreSQL");
            }
        }
    }
}
