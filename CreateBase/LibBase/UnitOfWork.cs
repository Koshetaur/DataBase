using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace LibBase
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext: DbContext
    {
        private TContext db; 

        public UnitOfWork(TContext context)
        {
            db = context;
        }

        public Repository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>(db);
        }

        public void Save()
        {
            db.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
