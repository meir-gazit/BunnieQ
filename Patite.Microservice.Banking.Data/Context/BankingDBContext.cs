using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Patite.Microservice.Banking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Patite.Microservice.Banking.Data.Context
{
    public class BankingDBContext : DbContext
    {
        public BankingDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite(@"Data Source=../Data/DB/BankingDB.db", b => b.MigrationsAssembly("Patite.Microservice.Banking.Api"));
            optionsBuilder.UseSqlite(@"Data Source=C:\Projects\c#\MyRabbits\Patite·Bunnie\BunnieQ\Patite.Microservice.Banking.Data\DB\BankingDB.db;");
        }

        public DbSet<Account> Accounts { get; set; }
    }
}
