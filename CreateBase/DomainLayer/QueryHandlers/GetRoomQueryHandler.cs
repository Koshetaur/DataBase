using System.Threading;
using System.Threading.Tasks;
using LibBase;

namespace DomainLayer
{
    public class GetRoomQueryHandler : QueryHandler<GetRoomQuery, RoomDto>
    {
        public GetRoomQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public override async Task<RoomDto> Handle(GetRoomQuery query, CancellationToken cancellationToken)
        {
            Room room = await GetAsync<Room>(query.Id);
            RoomDto resultRoom = new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
            };
            return resultRoom;
        }
    }
}
