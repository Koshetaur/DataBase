using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CreateBase
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ApplicationContext db1 = new ApplicationContext())
            {
                /*
                //Для создания новой базы
                
                User user1 = new User { Name = "Sergey", Surname = "Razumovskiy" };
                User user2 = new User { Name = "Oleg", Surname = "Volkov" };
                db1.Users.Add(user1);
                db1.Users.Add(user2);
                Room room1 = new Room { Name = "Room A" };
                Room room2 = new Room { Name = "Room B" };
                db1.Rooms.Add(room1);
                db1.Rooms.Add(room2);
                Connect reserve = new Connect { IdUser = 1, IdRoom = 2, Time = "3.15 pm" };
                db1.Connections.Add(reserve);
                db1.SaveChanges();*/

                var connections = db1.Connections.ToList();
                foreach (Connect c in connections)
                {
                    var users = db1.Users.ToList();
                    foreach (User u in users)
                    {
                        if (u.Id == c.IdUser){ Console.Write($"{u.Name} {u.Surname} "); }
                    }
                    var rooms = db1.Rooms.ToList();
                    foreach (Room r in rooms)
                    {
                        if (r.Id == c.IdRoom) { Console.Write($"reserved {r.Name} for {c.Time}."); }
                    }
                }

                Console.Read();
            }
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }

    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Connect
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdRoom { get; set; }
        public string Time { get; set; }
    }

    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Connect> Connections { get; set; }
        public string DbPath { get; }
        public ApplicationContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "usersandrooms.db");
            //Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
    }

}
