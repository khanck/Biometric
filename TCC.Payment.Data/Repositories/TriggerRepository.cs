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

        //public async Task<Trigger> DiscardOldTrigger(string device_ID)
        //{
        //    return await DbSet.Update(o => o.device_ID == device_ID && o.status == TriggerStatus.pending).OrderByDescending(o => o.createdDate).FirstOrDefaultAsync();
        //}

    }
}
