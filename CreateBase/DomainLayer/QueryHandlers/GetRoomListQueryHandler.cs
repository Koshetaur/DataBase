using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibBase;

namespace DomainLayer
{
    public class GetRoomListQueryHandler : QueryHandler<GetRoomListQuery, List<RoomDto>>
    {
        public GetRoomListQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public override Task<List<RoomDto>> Handle(GetRoomListQuery query, CancellationToken cancellationToken)
        {
            var rooms = GetQuery<Room>().ToList();
            List<RoomDto> resultRooms = new List<RoomDto>();
            foreach (Room r in rooms)
            {
                resultRooms.Add(new RoomDto
                {
                    Id = r.Id,
                    Name = r.Name,
                });
            }
            return Task.FromResult(resultRooms);
        }
    }
}
