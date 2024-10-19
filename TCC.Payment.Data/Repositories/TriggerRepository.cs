using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Payment.Data.Entities;
using TCC.Payment.Data.Enums;
using TCC.Payment.Data.Interfaces;

namespace TCC.Payment.Data.Repositories
{
    public class TriggerRepository : CoreRepository<Trigger>, ITriggerRepository
    {
        public TriggerRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<Trigger> GetLive(string device_ID)
        {
            return await DbSet.Where(o => o.device_ID == device_ID && o.status==TriggerStatus.pending && o.createdDate >= DateTime.Now.AddSeconds(-50)).OrderByDescending(o => o.createdDate).FirstOrDefaultAsync();
        }
        public async Task<List<Trigger>> DiscardOldPendingTriggerAsync(string device_ID)
        {
            var list= await DbSet.Where(o => o.device_ID == device_ID && o.status == TriggerStatus.pending).ToListAsync();
            foreach (Trigger item in list)
            {
                item.status = TriggerStatus.expired;
                DbSet.Update(item);
            }
           
            return list;
        }
        public async Task<List<Trigger>> TriggerSuccessAsync(string billNumber)
        {
            var list = await DbSet.Where(o => o.billNumber == billNumber && o.status == TriggerStatus.pending).ToListAsync();
            foreach (Trigger item in list)
            {
                item.status = TriggerStatus.success;
                DbSet.Update(item);
            }
            SaveChangesAsync();
            return list;
        }

    }
}
