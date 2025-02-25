using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;
using VCheck.Lib.Data.Models;

namespace VCheck.Lib.Data.DBContext
{
    public class TestResultDBContext : DbContext
    {
        private readonly IConfiguration config;

        public DbSet<TestResultModel> txn_testResults { get; set; }
        public DbSet<TestResultDetailsModel> txn_testresults_details { get; set; }
        public DbSet<TestListModel> mst_testlist { get; set; }

        public TestResultDBContext(IConfiguration config)
        {
            this.config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(config.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(config.GetConnectionString("DefaultConnection")));
        }
    }
}
