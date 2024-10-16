using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Payment.Data.Entities;

namespace TCC.Payment.Data.Interfaces
{
    public interface ITriggerRepository
    {
        Task<Trigger> GetLive(string device_ID);
        Trigger GetByID(Guid id);
        IQueryable<Trigger> GetAllAsQuerable();
        Trigger Add(Trigger obj);
        Trigger Update(Trigger obj);
        Trigger Delete(Guid id);
        List<Trigger> GetAll();
        Task<List<Trigger>> GetAllAsync();
        void SaveChangesAsync();
        void SaveChanges();


        Task<EntityEntry<Trigger>> AddAsync(Trigger obj);

    }
}
