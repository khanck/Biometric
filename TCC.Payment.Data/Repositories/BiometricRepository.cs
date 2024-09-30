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
    public class BiometricRepository : CoreRepository<Biometrics>, IBiometricRepository
    {
        public BiometricRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<Biometrics> GetByCustomerID(Guid customerID)
        {
            return await DbSet.Where(o => o.customer.Id == customerID).FirstOrDefaultAsync();
        }

    }
}
