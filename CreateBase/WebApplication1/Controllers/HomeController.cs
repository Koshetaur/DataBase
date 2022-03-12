using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    ResList.Add(c.Id + ". "  + c.User.Name + " " + c.User.Surname + " reserved " + c.Room.Name + " from " + c.TimeStart + " to " + c.TimeEnd + ".");
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
            using (Repository db = new Repository())
            {
                db.CreateUser(username, usersurname);
                db.Save();
            }
            return Redirect("~/Home/Added");
        }

        public IActionResult AddRoom()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNewRoom(string roomname)
        {
            using (Repository db = new Repository())
            {
                db.CreateRoom(roomname);
                db.Save();
            }
            return Redirect("~/Home/Added");
        }

        public IActionResult Added()
        {
            return View();
        }
        public IActionResult AddReserve()
        {
            using (Repository db = new Repository())
            {
                ViewBag.Usr = db.GetUserList();
                ViewBag.Rms = db.GetRoomList();
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddNewReserve(User userlist, Room roomlist, DateTime starttime, DateTime endtime)
        {
            using (Repository db = new Repository())
            {
                db.CreateReserve(userlist.Id, roomlist.Id, starttime, endtime);
                db.Save();
            }
            return Redirect("~/Home/Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
