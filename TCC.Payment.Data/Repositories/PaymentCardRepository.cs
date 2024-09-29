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
    public class PaymentCardRepository : CoreRepository<PaymentCard>, IPaymentCardRepository
    {
        public PaymentCardRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<PaymentCard> GetByCustomerID(string customerID)
        {
            return await DbSet.Where(o => o.customer.mobile == customerID.ToString()).FirstOrDefaultAsync();
        }

    }
}
