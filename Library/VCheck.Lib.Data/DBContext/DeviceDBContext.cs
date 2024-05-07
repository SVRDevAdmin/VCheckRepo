using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;
using VCheck.Lib.Data.Models;

namespace VCheck.Lib.Data.DBContext
{
    public class DeviceDBContext : DbContext
    {
        private readonly IConfiguration config;

        public DbSet<DeviceModel> mst_deviceslist { get; set; }

        public DeviceDBContext(IConfiguration config)
        {
            this.config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(config.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(config.GetConnectionString("DefaultConnection")));
        }
    }
}
