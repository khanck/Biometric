using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Payment.Data.Entities;

namespace TCC.Payment.Data.Interfaces
{
    public interface ICustomerRepository
    {
        Customer GetByID(Guid id);
        Task<Customer> GetByEmail(string? email);
        Task<Customer> GetByMobile(string? mobile);
        Task<Customer> GetByCustomerID(string customerID);
        Task<Customer> Login(string email, string password);
        IQueryable<Customer> GetAllAsQuerable();
        Customer Add(Customer obj);
        Customer Update(Customer obj);
        Customer Delete(Guid id);
        List<Customer> GetAll();
        Task<List<Customer>> GetAllAsync();
        void SaveChangesAsync();
        void SaveChanges();


        Task<EntityEntry<Customer>> AddAsync(Customer obj);

    }
}
