using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReserveWebApp.Models;
using DomainLayer;

namespace ReserveWebApp.Controllers
{
    public class HomeController : Controller
    {

        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Validators

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyRoomName(string roomName)
        {
            var result = await _mediator.Send(new VerifyRoomQuery
            {
                RoomName = roomName
            });
            return Json(result);
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyReserve(int SelectedRoomId, DateTime StartTime, DateTime EndTime, int Id)
        {
            var result = await _mediator.Send(new VerifyReserveQuery
            {
                Id = Id,
                RoomId = SelectedRoomId,
                TimeStart = StartTime,
                TimeEnd = EndTime
            });
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


        public async Task<IActionResult> Index()
        {
            var reserves = await _mediator.Send(new GetReserveListQuery
            {
                MinTime = DateTime.Today,
                MaxTime = DateTime.Today.AddDays(7)
            });
            var result = reserves
                .OrderBy(res => res.TimeStart)
                .Select(res => new ReservesViewModel
                {
                    Id = res.Id,
                    User =
                        $"{res.User.Name} {res.User.Surname}",
                    Room = res.Room.Name,
                    TimeStart = $"{res.TimeStart}",
                    TimeEnd = $"{res.TimeEnd}"
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
        public async Task<IActionResult> AddUser(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(new UserViewModel());
            await _mediator.Send(new AddUserCommand
            {
                Name = model.UserName,
                Surname = model.UserSurname
            });
            
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddRoom()
        {
            return View(new RoomViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoom(RoomViewModel model)
        {
            if (!ModelState.IsValid)
                return View(new RoomViewModel());
            await _mediator.Send(new AddRoomCommand
            {
                Name = model.RoomName,
            });

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> AddReserve()
        {
            return View(await GetReserveViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReserve(ReserveViewModel model)
        {
            if (!ModelState.IsValid)
                return View(await GetReserveViewModel());

            await _mediator.Send(new AddReserveCommand
            {
                UserId = model.SelectedUserId,
                RoomId = model.SelectedRoomId,
                TimeStart = model.StartTime,
                TimeEnd = model.EndTime
            }); ;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> EditReserve(int id)
        {
            return View(await GetReserveViewModel(id));
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReserve(int id, ReserveViewModel model)
        {
            if (!ModelState.IsValid)
                return View(await GetReserveViewModel(id));

            await _mediator.Send(new EditReserveCommand
            {
                Id = id,
                UserId = model.SelectedUserId,
                RoomId = model.SelectedRoomId,
                TimeStart = model.StartTime,
                TimeEnd = model.EndTime,
            });

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteReserve(int id)
        {
            await _mediator.Send(new DeleteReserveCommand { Id = id });
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<ReserveViewModel> GetReserveViewModel(int? id = null)
        {
            var model = new ReserveViewModel();


            var row = id.HasValue ? await _mediator.Send(new GetReserveQuery { Id = id.Value }) : null;

            var userList = await _mediator.Send(new GetUserListQuery());
            var users = userList.Select(x => new
            {
                x.Id,
                Name = $"{x.Surname} {x.Name}"
            })
            .OrderBy(x => x.Name)
            .ToList();
            model.SelectedUserId = row?.User.Id ?? users.Select(x => x.Id).FirstOrDefault();
            model.Users = new SelectList(users, "Id", "Name");

            var roomList = await _mediator.Send(new GetRoomListQuery());
            var rooms = roomList.OrderBy(x => x.Name).ToList();
            model.SelectedRoomId = row?.Room.Id ?? rooms.Select(x => x.Id).FirstOrDefault();
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
