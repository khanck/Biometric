using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Payment.Data.Entities;

namespace TCC.Payment.Data.Interfaces
{
    public interface IBiometricRepository
    {
        Task<Biometrics> GetByCustomerID(Guid customerID);
        Biometrics GetByID(Guid id);
        IQueryable<Biometrics> GetAllAsQuerable();
        Biometrics Add(Biometrics obj);
        Biometrics Update(Biometrics obj);
        Biometrics Delete(Guid id);
        List<Biometrics> GetAll();
        Task<List<Biometrics>> GetAllAsync();
        void SaveChangesAsync();
        void SaveChanges();

        Task<EntityEntry<Biometrics>> AddAsync(Biometrics obj);

    }
}
