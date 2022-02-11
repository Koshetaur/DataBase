using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateBase
{
    interface IRepository : IDisposable
    {
        List<User> GetUserList(); //получить список пользователей
        List<Room> GetRoomList(); //получить список комнат
        List<Reserve> GetReserveList(DateTime TimeMin, DateTime TimeMax); //получить список резервов
        User GetUser(int id); //получить пользователя по id
        Room GetRoom(int id); //получить комнату по id
        Reserve GetReserve(int id); //получить резерв по id
        void CreateUser(string name, string surname); //добавить пользователя
        void CreateRoom(string name); //добавить комнату
        void CreateReserve(int IdUser, int IdRoom, DateTime TimeSt, DateTime TimeEn); //добавить резерв
        void Save(); //сохранить изменения в базу данных
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
            var res = db.Reservs.Include(res => res.User).Include(res => res.Room).Where(res => res.TimeStart>TimeMin).Where(res => res.TimeEnd<TimeMax).ToList();
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
            return db.Reservs.Find(id);
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
