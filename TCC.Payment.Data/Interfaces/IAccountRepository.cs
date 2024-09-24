using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Payment.Data.Entities;

namespace TCC.Payment.Data.Interfaces
{
    public interface IAccountRepository
    {
        Account GetByID(Guid id);
        IQueryable<Account> GetAllAsQuerable();
        Account Add(Account obj);
        Account Update(Account obj);
        Account Delete(Guid id);
        List<Account> GetAll();
        Task<List<Account>> GetAllAsync();
        void SaveChangesAsync();
        void SaveChanges();

        Task<EntityEntry<Account>> AddAsync(Account obj);

    }
}
