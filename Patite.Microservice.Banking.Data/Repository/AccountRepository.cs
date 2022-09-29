using Patite.Microservice.Banking.Data.Context;
using Patite.Microservice.Banking.Domain.Interfaces;
using Patite.Microservice.Banking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patite.Microservice.Banking.Data.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BankingDBContext _ctx;

        public AccountRepository(BankingDBContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<Account> GetAccounts()
        {
            return _ctx.Accounts;   
        }
    }
}
