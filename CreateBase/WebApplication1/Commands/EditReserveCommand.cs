using System;
using MediatR;

namespace ReserveWebApp.Controllers
{
    public class EditReserveCommand : IRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
    }
}
