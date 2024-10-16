using Microsoft.EntityFrameworkCore;
using TCC.Payment.Data.Entities;

namespace TCC.Payment.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Customer> customers { get; set; }
        public DbSet<Biometrics> biometrics { get; set; }
        public DbSet<BiometricVerification> biometricVerifications { get; set; }
        public DbSet<PaymentCard> paymentCards { get; set; }
        public DbSet<Transaction> transactions { get; set; }
        public DbSet<Business> business { get; set; }
        public DbSet<Account> accounts { get; set; }
        public DbSet<Trigger> triggers { get; set; }     


    }
}
