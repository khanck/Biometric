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
    public class TransactionRepository : CoreRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<List<Transaction>> GetAllByCustomerID(Guid customerID)
        {
            return await DbSet.Where(o => o.paymentCard.customer_ID == customerID).OrderByDescending(o=>o.createdDate).Take(20).ToListAsync();
        }
        public async Task<List<Transaction>> GetAllByBusinessID(Guid businessID)
        {
            return await DbSet.Where(o => o.account.business_ID == businessID).OrderByDescending(o => o.createdDate).Take(100).ToListAsync();
        }

    }
}
