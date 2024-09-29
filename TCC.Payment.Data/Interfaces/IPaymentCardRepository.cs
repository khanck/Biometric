using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Payment.Data.Entities;

namespace TCC.Payment.Data.Interfaces
{
    public interface IPaymentCardRepository
    {
        Task<PaymentCard> GetByCustomerID(string customerID);
        PaymentCard GetByID(Guid id);
        IQueryable<PaymentCard> GetAllAsQuerable();
        PaymentCard Add(PaymentCard obj);
        PaymentCard Update(PaymentCard obj);
        PaymentCard Delete(Guid id);
        List<PaymentCard> GetAll();
        Task<List<PaymentCard>> GetAllAsync();
        void SaveChangesAsync();
        void SaveChanges();

        Task<EntityEntry<PaymentCard>> AddAsync(PaymentCard obj);

    }
}
