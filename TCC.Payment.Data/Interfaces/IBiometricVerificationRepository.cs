using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Payment.Data.Entities;

namespace TCC.Payment.Data.Interfaces
{
    public interface IBiometricVerificationRepository
    {
        BiometricVerification GetByID(Guid id);
        IQueryable<BiometricVerification> GetAllAsQuerable();
        BiometricVerification Add(BiometricVerification obj);
        BiometricVerification Update(BiometricVerification obj);
        BiometricVerification Delete(Guid id);
        List<BiometricVerification> GetAll();
        Task<List<BiometricVerification>> GetAllAsync();
        void SaveChangesAsync();
        void SaveChanges();

        Task<EntityEntry<BiometricVerification>> AddAsync(BiometricVerification obj);

    }
}
