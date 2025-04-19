using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuDokuhebi.Data
{
    class ConfigurationHelper
    {
        public static IConfigurationRoot Configuration { get; }

        static ConfigurationHelper()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public static string GetConnectionString(string name)
        {
            Console.WriteLine("Ruta base de configuración: " + Directory.GetCurrentDirectory());
            return Configuration.GetConnectionString(name);
        }
    }
}
