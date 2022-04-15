using System.Collections.Generic;
using MediatR;

namespace DomainLayer
{
    public class GetRoomListQuery : IRequest<List<RoomDto>>
    { }
}
