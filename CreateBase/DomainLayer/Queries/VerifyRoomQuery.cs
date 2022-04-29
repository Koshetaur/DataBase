using System;
using MediatR;

namespace DomainLayer
{
    public class VerifyRoomQuery : IRequest<bool>
    {
        public string RoomName { get; set; }
    }
}
