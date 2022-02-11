using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CreateBase
{
    class Program
    {
        static void Main(string[] args)
        {
            using (IRepository db = new Repository())
            {
                /*db.CreateUser("Sergey", "Razumovskiy");
                db.CreateUser("Oleg", "Volkov");
                db.CreateRoom("Room A");
                db.CreateRoom("Room B");
                db.Save();
                var start = new DateTime(2022, 02, 12, 13, 15, 00);
                var end = new DateTime(2022, 02, 12, 13, 40, 00);
                db.CreateReserve(1, 2, start, end);
                db.Save();*/
                var min = new DateTime(2022, 02, 12, 00, 00, 00);
                var max = new DateTime(2022, 02, 12, 12, 00, 00);
                var connections = db.GetReserveList(min, max);
                foreach (Reserve c in connections)
                {
                    Console.Write($"{c.Id}. {c.User.Name} {c.User.Surname} reserved {c.Room.Name} from {c.TimeStart} to {c.TimeEnd}.\n");
                }
                Console.Read();
            }
        }
    }
}
