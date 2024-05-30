using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace VCheck.Lib.Data.DBContext
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        private string connectionString = "";

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            connectionString = options.FindExtension<MySqlOptionsExtension>().ConnectionString;
        }

        public ApplicationDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseMySql("Server=localhost;Database=vcheckdb;User=root;Password=Retes@123;", new MySqlServerVersion(new Version(8, 0, 21)));
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
