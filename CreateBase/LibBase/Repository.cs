using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace LibBase
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Возвращает список всех объектов заданного типа в базе данных в формате List
        /// </summary>
        /// <returns></returns>
        IQueryable<T> Query();
        /// <summary>
        /// Возвращает объект в базе данных с заданным id
        /// </summary>
        /// <param name="id">Id нужного объекта</param>
        /// <returns></returns>
        T Get(int id);
        /// <summary>
        /// Добавляет нового пользователя в базу данных
        /// </summary>
        /// <param name="obj">объект</param>
        void Create(T obj);
        /// <summary>
        /// Удаляет из базы объект
        /// </summary>
        /// <param name="id">Id нужного объекта</param>
        void Delete(int id);
    }

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

        public void Delete(int id)
        {
            db.Set<T>().Remove(Get(id));
        }

        public T Get(int id)
        {
            return db.Set<T>().Find(id);
        }

        public IQueryable<T> Query()
        {
            return db.Set<T>().AsQueryable();
        }
    }

    public interface IUnitOfWork: IDisposable
    {
        /// <summary>
        /// Создаёт репозиторий заданного класса
        /// </summary>
        /// <typeparam name="T">класс репозитория</typeparam>
        /// <returns></returns>
        Repository<T> GetRepository<T>() where T: class;
        /// <summary>
        /// Сохраняет изменения в базе данных
        /// </summary>
        void Save();
    }

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
