using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VCheckListenerWorker.Lib.Models;

namespace VCheckListenerWorker.Lib.DBContext
{
    public class TestResultDBContext : DbContext
    {
        private readonly IConfiguration iconfig;

        public DbSet<tbltestanalyze_results> tbltestanalyze_results { get; set; }
        public DbSet<tbltestanalyze_results_messageheader> tbltestanalyze_results_messageheader { get; set; }
        public DbSet<tbltestanalyze_results_notes> tbltestanalyze_results_notes { get; set; }
        public DbSet<tbltestanalyze_results_observationrequest> tbltestanalyze_results_observationrequest { get; set; }
        public DbSet<tbltestanalyze_results_observationresult> tbltestanalyze_results_observationresult { get; set; }
        public DbSet<tbltestanalyze_results_patientidentification> tbltestanalyze_results_patientidentification { get; set; }

        public TestResultDBContext(IConfiguration config)
        {
            iconfig = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(iconfig.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(iconfig.GetConnectionString("DefaultConnection")));
        }
    }
}
