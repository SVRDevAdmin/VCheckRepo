using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Lib.Data.DBContext
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        private string connectionString;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            connectionString = this.Database.GetDbConnection().ConnectionString;
        }

        public ApplicationDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21)));
            optionsBuilder.UseMySql("Server=localhost;Database=vcheckdb;User=root;Password=Retes@123;", new MySqlServerVersion(new Version(8, 0, 21)));
        }
    }
}
