using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using LibBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using ReserveWebApp.Models;

namespace ReserveWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        #region Validators

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyRoomName(string roomName)
        {
            bool result;
            using (var db = new Repository())
                result = db.IsRoomUnique(roomName);
            return Json(result);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifySurname([RegularExpression(@"^[^\-][\p{L}\-]*[^\-]$")] string userSurname)
        {
            return Json(ModelState.IsValid);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyName([RegularExpression(@"^[\p{L}]*$")] string userName)
        {
            return Json(ModelState.IsValid);
        }

        #endregion

        public IActionResult AddNew()
        {
            return View();
        }

        public IActionResult Index()
        {
            List<ReservesViewModel> result;

            using (var db = new Repository())
            {
                var min = DateTime.Today;
                var max = min.AddDays(7);
                result = db.GetReserveList(min, max)
                    .OrderBy(x => x.TimeStart)
                    .Select(x => new ReservesViewModel
                    {
                        Id = x.Id,
                        Title =
                            $"{x.User.Name} {x.User.Surname} reserved {x.Room.Name} from {x.TimeStart} to {x.TimeEnd}"
                    })
                    .ToList();
            }

            return View(result);
        }

        public IActionResult AddUser()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddUser(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(new UserViewModel());

            using (Repository db = new Repository())
            {
                db.CreateUser(model.UserName, model.UserSurname);
                db.Save();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddRoom()
        {
            return View(new RoomViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRoom(RoomViewModel model)
        {
            if (!ModelState.IsValid)
                return View(new RoomViewModel());

            using (Repository db = new Repository())
            {
                db.CreateRoom(model.RoomName);
                db.Save();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult AddReserve()
        {
            return View(GetReserveViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddReserve(ReserveViewModel model)
        {
            if (!ModelState.IsValid)
                return View(GetReserveViewModel());

            using (var db = new Repository())
            {
                db.CreateReserve(model.SelectedUserId, model.SelectedRoomId, model.StartTime, model.EndTime);
                db.Save();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("{id}")]
        public IActionResult EditReserve(int id)
        {
            return View(GetReserveViewModel(id));
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult EditReserve(int id, ReserveViewModel model)
        {
            if (!ModelState.IsValid)
                return View(GetReserveViewModel(id));

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

            return RedirectToAction(nameof(Index));
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

        private static DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }
    }
}
