using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibBase
{
    public interface IRepository : IDisposable
    {
        /// <summary>
        /// Возвращает список всех пользователей в базе данных в формате List
        /// </summary>
        /// <returns></returns>
        List<User> GetUserList();
        /// <summary>
        /// Возвращает список всех комнат в базе данных в формате List
        /// </summary>
        /// <returns></returns>
        List<Room> GetRoomList();
        /// <summary>
        /// Возвращает список резервов за указанный промежуток времени в формате List
        /// </summary>
        /// <param name="TimeMin">Минимальное время начала резерва</param>
        /// <param name="TimeMax">Максимальное время начала резерва</param>
        /// <returns></returns>
        List<Reserve> GetReserveList(DateTime TimeMin, DateTime TimeMax);
        /// <summary>
        /// Возвращает объект пользователя в базе данных с заданным id
        /// </summary>
        /// <param name="id">Id нужного пользователя</param>
        /// <returns></returns>
        User GetUser(int id); 
        /// <summary>
        /// Возвращает объект комнаты в базе данных с заданным id
        /// </summary>
        /// <param name="id">Id нужной комнаты</param>
        /// <returns></returns>
        Room GetRoom(int id);
        /// <summary>
        /// Возвращает объект резерва в базе данных с заданным id
        /// </summary>
        /// <param name="id">Id нужного резерва</param>
        /// <returns></returns>
        Reserve GetReserve(int id);
        /// <summary>
        /// Добавляет нового пользователя в базу данных
        /// </summary>
        /// <param name="name">Имя пользователя</param>
        /// <param name="surname">Фамилия пользователя</param>
        void CreateUser(string name, string surname); 
        /// <summary>
        /// Добавляет новую комнату в базу данных
        /// </summary>
        /// <param name="name">Название комнаты</param>
        void CreateRoom(string name); 
        /// <summary>
        /// Создаёт новый резерв
        /// </summary>
        /// <param name="IdUser">Id пользователя, резервирующего комнату</param>
        /// <param name="IdRoom">Id Резервируемой комнаты</param>
        /// <param name="TimeSt">Время начала резерва</param>
        /// <param name="TimeEn">Время окончания резерва</param>
        void CreateReserve(int IdUser, int IdRoom, DateTime TimeSt, DateTime TimeEn);
        /// <summary>
        /// Удаляет из базы пользователя
        /// </summary>
        /// <param name="id">Id нужного пользователя</param>
        void DeleteUser(int id);
        /// <summary>
        /// Удаляет из базы комнату
        /// </summary>
        /// <param name="id">Id нужной комнаты</param>
        void DeleteRoom(int id);
        /// <summary>
        /// Удаляет из базы резерв
        /// </summary>
        /// <param name="id">Id нужного резерва</param>
        void DeleteReserve(int id);
        /// <summary>
        /// Проверяет уникальность имени добавляемой комнаты
        /// </summary>
        /// <param name="room">Название комнаты</param>
        /// <returns></returns>
        bool IsRoomUnique(string room);
        /// <summary>
        /// Сохраняет изменения в базе данных
        /// </summary>
        void Save();
    }

    public class Repository : IRepository
    {
        private ApplicationContext db;

        public Repository()
        {
            var opt = new ApplicationDbContextOptions
            {
                ConnectionString = Helper.ConnectionString
            };
            db = new ApplicationContext(opt);
        }

        public List<User> GetUserList()
        {
            return db.Users.ToList();
        }

        public List<Room> GetRoomList()
        {
            return db.Rooms.ToList();
        }

        public List<Reserve> GetReserveList(DateTime TimeMin, DateTime TimeMax)
        {
            var res = db.Reservs.Include(res => res.User).Include(res => res.Room).Where(res => res.TimeStart>=TimeMin && res.TimeEnd<=TimeMax).ToList();
            return res;
        }

        public User GetUser(int id)
        {
            return db.Users.Find(id);
        }

        public Room GetRoom(int id)
        {
            return db.Rooms.Find(id);
        }

        public Reserve GetReserve(int id)
        {
            return db.Reservs.Include(res => res.User).Include(res => res.Room).SingleOrDefault(res => res.Id == id);
        }

        public void CreateUser(string name, string surname)
        {
            User user = new User { Name = name, Surname = surname };
            db.Users.Add(user);
        }

        public void CreateRoom(string name)
        {
            Room room = new Room { Name = name };
            db.Rooms.Add(room);
        }

        public void CreateReserve(int IdUser, int IdRoom, DateTime TimeSt, DateTime TimeEn)
        {
            User user = GetUser(IdUser);
            Room room = GetRoom(IdRoom);
            Reserve res = new Reserve { User = user, Room = room, TimeStart = TimeSt, TimeEnd = TimeEn };
            db.Reservs.Add(res);
        }

        public void DeleteUser(int id)
        {
            User user = GetUser(id);
            db.Users.Remove(user);
        }

        public void DeleteRoom(int id)
        {
            Room room = GetRoom(id);
            db.Rooms.Remove(room);
        }

        public void DeleteReserve(int id)
        {
            Reserve reserve = GetReserve(id);
            db.Reservs.Remove(reserve);
        }

        public bool IsRoomUnique(string room)
        {
            return !db.Rooms.Any(x => x.Name == room);
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
