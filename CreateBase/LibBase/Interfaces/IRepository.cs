using System.Linq;
using System.Threading.Tasks;

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
        Task<ValueTask<T>> GetAsync(int id);
        /// <summary>
        /// Добавляет нового пользователя в базу данных
        /// </summary>
        /// <param name="obj">объект</param>
        void Create(T obj);
        Task CreateAsync(T obj);
        /// <summary>
        /// Удаляет из базы объект
        /// </summary>
        /// <param name="id">Id нужного объекта</param>
        void Delete(int id);
    }
}
