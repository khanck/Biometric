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
    public class BusinessRepository : CoreRepository<Business>, IBusinessRepository
    {
        public BusinessRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<Business> Login(string email, string password)
        {
            return await DbSet.Where(o => o.email.ToLower() == email.ToLower() & o.password == password.Trim()).FirstOrDefaultAsync();
        }

    }
}
