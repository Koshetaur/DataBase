using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using WebApplication1.Models;
using LibBase;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.Controllers
{
    public class UserViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Surname")]
        public string UserSurname { get; set; }
        public string Message { get; set; }
        public bool IsNoDigits()
        {
            foreach(char cn in this.UserName)
            {
                if (!Char.IsLetter(cn)) { return false; }
            }
            foreach (char cs in this.UserSurname)
            {
                if (!Char.IsLetter(cs)) { return false; }
            }
            return true;
        }
    }

    public class RoomViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Office")]
        public string RoomName { get; set; }
        public string Message { get; set; }
        public bool IsUnique()
        {
            using (Repository db = new Repository())
            {
                var roomlist = db.GetRoomList();
                if (roomlist.Exists(x => x.Name == this.RoomName))
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class ReserveViewModel
    {
        [Display(Name = "Employee")]
        public int SelectedUserId { get; set; }
        public SelectList Users { get; set; }

        [Display(Name = "Office")]
        public int SelectedRoomId { get; set; }
        public SelectList Rooms { get; set; }

        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        [HiddenInput]
        public int Id { get; set; }
    }

    public class ReservesViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<ReservesViewModel> result;

            using (var db = new Repository())
            {
                var min = DateTime.Today;
                var max = min.AddDays(7);
                result = db.GetReserveList(min, max).Select(x => new ReservesViewModel
                {
                    Id = x.Id,
                    Title = $"{x.User.Name} {x.User.Surname} reserved {x.Room.Name} from {x.TimeStart} to {x.TimeEnd}"
                }).ToList();
            }

            return View(result);
        }

        public IActionResult AddNew()
        {
            return View();
        }

        public IActionResult AddUser()
        {
            return View(GetUserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddUser([Bind("UserName,UserSurname")] UserViewModel model)
        {
            if (ModelState.IsValid && model.IsNoDigits())
            {
                using (Repository db = new Repository())
                {
                    db.CreateUser(model.UserName, model.UserSurname);
                    db.Save();
                }
                return View(new UserViewModel { UserName = null, UserSurname = null, Message = "Added successfully." });
            }
            return View(new UserViewModel { Message = "Please enter employee name and surname containing only letters." });
        }

        public IActionResult AddRoom()
        {
            return View(GetRoomViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRoom([Bind("RoomName")] RoomViewModel model)
        {
            if (ModelState.IsValid && model.IsUnique())
            {
                using (Repository db = new Repository())
                {
                    db.CreateRoom(model.RoomName);
                    db.Save();
                }
                return View(new RoomViewModel { RoomName = null, Message = "Added succesfully." });
            }
            return View(new RoomViewModel { Message = "Please enter unique office name." });
        }

        [HttpGet]
        public IActionResult AddReserve()
        {
            return View(GetReserveViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddReserve([Bind("SelectedUserId,SelectedRoomId,StartTime,EndTime")] ReserveViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new Repository())
                {
                    db.CreateReserve(model.SelectedUserId, model.SelectedRoomId, model.StartTime, model.EndTime);
                    db.Save();
                }
                return Redirect("~/Home/Index");
            }

            return View(GetReserveViewModel());
        }

        [HttpGet("{id}")]
        public IActionResult EditReserve(int id)
        {
            return View(GetReserveViewModel(id));
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult EditReserve(int id, [Bind("SelectedUserId,SelectedRoomId,StartTime,EndTime")] ReserveViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new Repository())
                {
                    var row = db.GetReserve(id);
                    if (row != null)
                    {
                        row.UserId = model.SelectedUserId;
                        row.RoomId = model.SelectedRoomId;
                        row.TimeStart = model.StartTime;
                        row.TimeEnd = model.EndTime;
                        db.Save();
                    }
                }

                return Redirect("~/Home/Index");
            }

            return View(GetReserveViewModel(id));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private static ReserveViewModel GetReserveViewModel(int? id = null)
        {
            var model = new ReserveViewModel();

            using var db = new Repository();
            var row = id.HasValue ? db.GetReserve(id.Value) : null;

            var users = db.GetUserList().Select(x => new { x.Id, Name = $"{x.Surname} {x.Name}" }).OrderBy(x => x.Name).ToList();
            model.SelectedUserId = row?.UserId ?? users.Select(x => x.Id).FirstOrDefault();
            model.Users = new SelectList(users, "Id", "Name");

            var rooms = db.GetRoomList().OrderBy(x => x.Name).ToList();
            model.SelectedRoomId = row?.RoomId ?? rooms.Select(x => x.Id).FirstOrDefault();
            model.Rooms = new SelectList(rooms, "Id", "Name");

            var now = DateTime.Now;
            model.StartTime = row?.TimeStart ?? RoundUp(now, TimeSpan.FromMinutes(1));
            model.EndTime = row?.TimeEnd ?? model.StartTime.AddHours(1);

            return model;
        }

        private static RoomViewModel GetRoomViewModel()
        {
            var model = new RoomViewModel();
            model.Message = " ";
            return model;
        }

        private static UserViewModel GetUserViewModel()
        {
            var model = new UserViewModel();
            model.Message = " ";
            return model;
        }

        private static DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }
    }
}
