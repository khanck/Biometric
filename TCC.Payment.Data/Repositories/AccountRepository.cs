using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Payment.Data.Entities;
using TCC.Payment.Data.Interfaces;

namespace TCC.Payment.Data.Repositories
{
    public class AccountRepository : CoreRepository<Account>, IAccountRepository
    {
        public AccountRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<Account> GetByBusinessID(Guid businessID)
        {
            return await DbSet.Where(o => o.business_ID == businessID).FirstOrDefaultAsync();
        }

    }
}
