using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheckViewer.Lib.Function
{
    public class ConfigSettings
    {
        public static Microsoft.Extensions.Configuration.IConfiguration GetConnectionConfiguration()
        {
            //IConfiguration iConfig = new ConfigurationBuilder()
            //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //.Build();
            IConfiguration iConfig = null;
            return iConfig;
        }

    }
}
