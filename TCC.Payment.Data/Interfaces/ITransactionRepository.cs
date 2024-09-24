using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Payment.Data.Entities;

namespace TCC.Payment.Data.Interfaces
{
    public interface ITransactionRepository
    {
        Transaction GetByID(Guid id);
        IQueryable<Transaction> GetAllAsQuerable();
        Transaction Add(Transaction obj);
        Transaction Update(Transaction obj);
        Transaction Delete(Guid id);
        List<Transaction> GetAll();
        Task<List<Transaction>> GetAllAsync();
        void SaveChangesAsync();
        void SaveChanges();

        Task<EntityEntry<Transaction>> AddAsync(Transaction obj);

        Task<List<Transaction>> GetAllByCustomerID(Guid customerID);

    }
}
