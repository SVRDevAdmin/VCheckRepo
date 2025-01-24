using log4net;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using VCheck.Lib.Data.Models;

namespace VCheck.Lib.Data.DBContext
{
    public class SystemProcessingLogDBContext : DbContext
    {
        private readonly IConfiguration config;

        public DbSet<ProcessingLogModel> System_ProcessingLog { get; set; }

        public SystemProcessingLogDBContext(IConfiguration config) 
        {
            this.config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(config.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(config.GetConnectionString("DefaultConnection")));
        }
    }
}
