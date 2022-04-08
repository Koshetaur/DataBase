using System;
using System.Threading.Tasks;

namespace LibBase
{
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
        Task SaveAsync();
    }
}
