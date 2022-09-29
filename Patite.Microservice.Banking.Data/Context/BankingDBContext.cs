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
            optionsBuilder.UseSqlite(@"Data Source=../DB/BankingDB.db", b => b.MigrationsAssembly("Patite.Microservice.Banking.Api"));
        }

        public DbSet<Account> Accounts { get; set; }
    }
}
