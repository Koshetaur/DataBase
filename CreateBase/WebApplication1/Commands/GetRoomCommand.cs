using MediatR;

namespace ReserveWebApp.Controllers
{
    public class GetRoomCommand : IRequest<RoomDto>
    {
        public int Id { get; set; }
    }
}
