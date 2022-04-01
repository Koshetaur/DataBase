using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibBase
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Возвращает список всех объектов заданного типа в базе данных в формате List
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetList();
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

    public class UnitOfWork : IDisposable
    {
        private ApplicationContext db = new ApplicationContext(new ApplicationDbContextOptions { ConnectionString = Helper.ConnectionString });
        private UserRepository userRepository;
        private RoomRepository roomRepository;
        private ReserveRepository reserveRepository;

        public UserRepository Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(db);
                return userRepository;
            }
        }

        public RoomRepository Rooms
        {
            get
            {
                if (roomRepository == null)
                    roomRepository = new RoomRepository(db);
                return roomRepository;
            }
        }

        public ReserveRepository Reserves
        {
            get
            {
                if (reserveRepository == null)
                    reserveRepository = new ReserveRepository(db);
                return reserveRepository;
            }
        }

        public bool IsRoomUnique(string room)
        {
            return !db.Rooms.Any(x => x.Name == room);
        }

        public bool IsReserveCorrect(int roomId, DateTime timeStart, DateTime timeEnd, int id)
        {
            return !db.Reservs.Any(res => res.TimeEnd >= timeStart && res.TimeStart <= timeEnd && res.RoomId == roomId && res.Id != id);
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

    public class UserRepository : IRepository<User>
    {
        private ApplicationContext db;

        public UserRepository(ApplicationContext context)
        {
            this.db = context;
        }
        public IEnumerable<User> GetList()
        {
            return db.Users.ToList();
        }
        public User Get(int id)
        {
            return db.Users.Find(id);
        }
        public void Create(User user)
        {
            db.Users.Add(user);
        }
        public void Delete(int id)
        {
            User user = Get(id);
            db.Users.Remove(user);
        }
    }

    public class RoomRepository : IRepository<Room>
    {
        private ApplicationContext db;

        public RoomRepository(ApplicationContext context)
        {
            this.db = context;
        }
        public IEnumerable<Room> GetList()
        {
            return db.Rooms.ToList();
        }
        public Room Get(int id)
        {
            return db.Rooms.Find(id);
        }
        public void Create(Room room)
        {
            db.Rooms.Add(room);
        }
        public void Delete(int id)
        {
            Room room = Get(id);
            db.Rooms.Remove(room);
        }
    }

    public class ReserveRepository : IRepository<Reserve>
    {
        private ApplicationContext db;

        public ReserveRepository(ApplicationContext context)
        {
            this.db = context;
        }
        public IEnumerable<Reserve> GetList()
        {
            return db.Reservs.Include(res => res.User).Include(res => res.Room).ToList();
        }
        public IEnumerable<Reserve> GetList(DateTime TimeMin, DateTime TimeMax)
        {
            return db.Reservs.Include(res => res.User).Include(res => res.Room).Where(res => res.TimeEnd >= TimeMin && res.TimeStart <= TimeMax).ToList();
        }
        public Reserve Get(int id)
        {
            return db.Reservs.Include(res => res.User).Include(res => res.Room).SingleOrDefault(res => res.Id == id);
        }
        public void Create(Reserve reserve)
        {
            db.Reservs.Add(reserve);
        }
        public void Delete(int id)
        {
            Reserve reserve = db.Reservs.SingleOrDefault(res => res.Id == id);
            db.Reservs.Remove(reserve);
        }
    }
}
