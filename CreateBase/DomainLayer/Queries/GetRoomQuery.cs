using MediatR;

namespace DomainLayer
{
    public class GetRoomQuery : IRequest<RoomDto>
    {
        public int Id { get; set; }
    }
}
