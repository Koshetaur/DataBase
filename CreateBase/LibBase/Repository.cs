using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LibBase
{

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext db;

        public Repository(DbContext context)
        {
            db = context;
        }
        public void Create(T obj)
        {
            db.Set<T>().Add(obj);
        }

        public async Task CreateAsync(T obj)
        {
            await db.Set<T>().AddAsync(obj);
        }

        public void Delete(int id)
        {
            db.Set<T>().Remove(Get(id));
        }

        public T Get(int id)
        {
            return db.Set<T>().Find(id);
        }

        public async Task<T> GetAsync(int id)
        {
            return await db.Set<T>().FindAsync(id);
        }

        public IQueryable<T> Query()
        {
            return db.Set<T>().AsQueryable();
        }
    }
}
