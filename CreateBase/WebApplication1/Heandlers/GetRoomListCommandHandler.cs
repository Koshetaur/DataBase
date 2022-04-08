using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibBase;
using MediatR;

namespace ReserveWebApp.Controllers
{
    public class GetRoomListCommandHandler : IRequestHandler<GetRoomListCommand, List<RoomDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetRoomListCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<RoomDto>> Handle(GetRoomListCommand command, CancellationToken cancellationToken)
        {
            var rooms = _unitOfWork.GetRepository<Room>().Query().ToList();
            List<RoomDto> resultRooms = new List<RoomDto>();
            foreach (Room r in rooms)
            {
                resultRooms.Add(new RoomDto
                {
                    Id = r.Id,
                    Name = r.Name,
                });
            }
            return resultRooms;
        }
    }
}
