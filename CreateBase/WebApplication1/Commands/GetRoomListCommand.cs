using System.Collections.Generic;
using MediatR;

namespace ReserveWebApp.Controllers
{
    public class GetRoomListCommand : IRequest<List<RoomDto>>
    { }
}
