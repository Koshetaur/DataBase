using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using LibBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReserveWebApp.Models;

namespace ReserveWebApp.Controllers
{
    public class HomeController : Controller
    {

        private readonly UnitOfWork _unitOfWork;

        public HomeController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Validators

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyRoomName(string roomName)
        {
            var result = _unitOfWork.IsRoomUnique(roomName);
            return Json(result);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyReserve(int SelectedRoomId, DateTime StartTime, DateTime EndTime, int Id)
        {
            var result = _unitOfWork.IsReserveCorrect(SelectedRoomId, StartTime, EndTime, Id);
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


        public IActionResult Index()
        {
            var min = DateTime.Today;
            var max = min.AddDays(7);
            var result = _unitOfWork.Reserves.GetList(min, max)
                .OrderBy(x => x.TimeStart)
                .Select(x => new ReservesViewModel
                {
                    Id = x.Id,
                    User =
                        $"{x.User.Name} {x.User.Surname}",
                    Room = x.Room.Name,
                    TimeStart = $"{x.TimeStart}",
                    TimeEnd = $"{x.TimeEnd}"
                })
                .ToList();

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
            _unitOfWork.Users.Create(new User
            {
                Name = model.UserName,
                Surname = model.UserSurname
            });
            _unitOfWork.Save();
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
            _unitOfWork.Rooms.Create(new Room
            {
                Name = model.RoomName,
            });
            _unitOfWork.Save();

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
            _unitOfWork.Reserves.Create(new Reserve
            {
                User = _unitOfWork.Users.Get(model.SelectedUserId),
                Room = _unitOfWork.Rooms.Get(model.SelectedRoomId),
                TimeStart = model.StartTime,
                TimeEnd = model.EndTime
            });
            _unitOfWork.Save();

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

            var row = _unitOfWork.Reserves.Get(id);
            if (row != null)
            {
                row.UserId = model.SelectedUserId;
                row.RoomId = model.SelectedRoomId;
                row.TimeStart = model.StartTime;
                row.TimeEnd = model.EndTime;
                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteReserve(int id)
        {
            _unitOfWork.Reserves.Delete(id);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private ReserveViewModel GetReserveViewModel(int? id = null)
        {
            var model = new ReserveViewModel();

            var row = id.HasValue ? _unitOfWork.Reserves.Get(id.Value) : null;

            var users = _unitOfWork.Users.GetList()
                .Select(x => new
                {
                    x.Id, 
                    Name = $"{x.Surname} {x.Name}"
                })
                .OrderBy(x => x.Name)
                .ToList();
            model.SelectedUserId = row?.UserId ?? users.Select(x => x.Id).FirstOrDefault();
            model.Users = new SelectList(users, "Id", "Name");

            var rooms = _unitOfWork.Rooms.GetList().OrderBy(x => x.Name).ToList();
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
