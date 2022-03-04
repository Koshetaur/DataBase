using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using LibBase;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var ResList = new List<string>();
            using (Repository db = new Repository())
            {
                DateTime min = DateTime.Today;
                DateTime max = min.AddDays(7);
                var connections = db.GetReserveList(min, max);
                foreach (Reserve c in connections)
                {
                    ResList.Add(c.Id + ". " 
                        + c.User.Name + " " + c.User.Surname + " reserved " + c.Room.Name + " from " + c.TimeStart + " to " + c.TimeEnd + ".");
                }
            }
            ViewBag.Res = ResList;
            return View();
        }

        public IActionResult AddNew()
        {
            return View();
        }

        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNewUser(string username, string usersurname)
        {
            var UsrList = new List<string>();
            using (Repository db = new Repository())
            {
                db.CreateUser(username, usersurname);
                db.Save();
                var usrs = db.GetRoomsList();
                foreach (User u in usrs)
                {
                    UsrList.Add(u.Id + ". " + u.Name + " " + u.Surname + ".");
                }
            }
            ViewBag.Usr = UsrList;
            return View();
        }

        public IActionResult AddRoom()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNewRoom(string roomname)
        {
            var RmsList = new List<string>();
            using (Repository db = new Repository())
            {
                db.CreateRoom(roomname);
                db.Save();
                var rms = db.GetRoomList();
                foreach (Room r in rms)
                {
                    RmsList.Add(r.Id + ". " + r.Name + " " + ".");
                }
            }
            ViewBag.Rms = RmsList;
            return View();
        }

        public IActionResult AddReserve()
        {
            var UsrList = new List<string>();
            var RmsList = new List<string>();
            using (Repository db = new Repository())
            {
                var usrs = db.GetRoomsList();
                foreach (User u in usrs)
                {
                    UsrList.Add(u.Id + ". " + u.Name + " " + u.Surname);
                }
                var rms = db.GetRoomList();
                foreach (Room r in rms)
                {
                    RmsList.Add(r.Id + ". " + r.Name);
                }
            }
            ViewBag.Usr = UsrList;
            ViewBag.Rms = RmsList;
            return View();
        }

        [HttpPost]
        public IActionResult AddNewReserve(string userlist, string roomlist, DateTime starttime, DateTime endtime)
        {
            var ResList = new List<string>();
            using (Repository db = new Repository())
            {
                string userid = userlist.Remove(userlist.IndexOf('.'), userlist.Length - userlist.IndexOf('.'));
                string roomid = roomlist.Remove(roomlist.IndexOf('.'), roomlist.Length - roomlist.IndexOf('.'));
                db.CreateReserve(Int32.Parse(userid), Int32.Parse(roomid), starttime, endtime);
                db.Save();
                var min = new DateTime(2021, 02, 12, 00, 00, 00);
                var max = new DateTime(2023, 02, 12, 00, 00, 00);
                var connections = db.GetReserveList(min, max);
                foreach (Reserve c in connections)
                {
                    ResList.Add(c.Id + ". " + c.User.Name + " " + c.User.Surname + " reserved " + c.Room.Name + " from " + c.TimeStart + " to " + c.TimeEnd + ".");
                }
            }
            ViewBag.Resall = ResList;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
