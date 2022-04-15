using System;
using MediatR;

namespace DomainLayer
{
    public class AddReserveCommand : IRequest
    {
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
    }
}
