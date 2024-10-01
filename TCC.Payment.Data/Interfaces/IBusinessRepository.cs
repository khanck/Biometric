using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Payment.Data.Entities;

namespace TCC.Payment.Data.Interfaces
{
    public interface IBusinessRepository
    {
        Task<Business> Login(string email, string password);
        Business GetByID(Guid id);
        IQueryable<Business> GetAllAsQuerable();
        Business Add(Business obj);
        Business Update(Business obj);
        Business Delete(Guid id);
        List<Business> GetAll();
        Task<List<Business>> GetAllAsync();
        void SaveChangesAsync();
        void SaveChanges();


        Task<EntityEntry<Business>> AddAsync(Business obj);

    }
}
