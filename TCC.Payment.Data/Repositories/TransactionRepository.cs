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
      

    }
}
