using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Payment.Data.Context;

namespace TCC.Payment.Data.Repositories
{
    public class CoreRepository<T> : IDisposable where T : class
    {
        private IServiceProvider _serviceProvider;
        protected AppDbContext _context;


        public CoreRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _context = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());
            DbSet = _context.Set<T>();
        }
        protected DbSet<T> DbSet
        {
            get; set;
        }
        public T Add(T obj)
        {
            return DbSet.Add(obj).Entity;
        }
        public async Task<EntityEntry<T>> AddAsync(T obj)
        {          
            return await DbSet.AddAsync(obj);
        }
        public T Update(T obj)
        {
            return DbSet.Update(obj).Entity;
        }
        public List<T> GetAll()
        {
            return DbSet.ToListAsync().Result;
        }
        public async Task<List<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public IQueryable<T> GetAllAsQuerable()
        {
            return DbSet.AsQueryable();
        }
        public T GetByID(Guid id)
        {
            return DbSet.Find(id);
        }
        public async Task<T> GetByIDAsync(Guid id)
        {
            return await DbSet.FindAsync(id);
        }
        public T Delete(Guid id)
        {
            T obj = DbSet.Find(id);
            if (obj == null)
                return null;
            return DbSet.Remove(obj).Entity;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();

        }
        public void SaveChangesAsync()
        {
            _context.SaveChangesAsync();

        }
        public void DisableValidationAndSaveChanges()
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            _context.SaveChanges();
            _context.ChangeTracker.AutoDetectChangesEnabled = true;
        }

        public virtual void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }

        }


    }
}
