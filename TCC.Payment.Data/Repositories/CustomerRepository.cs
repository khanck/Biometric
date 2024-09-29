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
    public class CustomerRepository: CoreRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<Customer> GetByEmail(string? email)
        {
            return await DbSet.Where(o => o.email == email).FirstOrDefaultAsync();
        }
        public async Task<Customer> GetByMobile(string? mobile)
        {
            return await DbSet.Where(o => o.mobile == mobile).FirstOrDefaultAsync();
        }
        public async Task<Customer> Login(string email, string password)
        {
            return await DbSet.Where(o => o.email == email & o.password == password.Trim()).FirstOrDefaultAsync();
        }
        public async Task<Customer> GetByCustomerID(string customerID)
        {
            return await DbSet.Where(o => o.mobile == customerID).FirstOrDefaultAsync();
        }


    }
}
