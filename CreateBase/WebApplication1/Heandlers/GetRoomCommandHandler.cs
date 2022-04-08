using System.Threading;
using System.Threading.Tasks;
using LibBase;
using MediatR;

namespace ReserveWebApp.Controllers
{
    public class GetRoomCommandHandler : IRequestHandler<GetRoomCommand, RoomDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetRoomCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<RoomDto> Handle(GetRoomCommand command, CancellationToken cancellationToken)
        {
            ValueTask<Room> valueTaskRoom = await _unitOfWork.GetRepository<Room>().GetAsync(command.Id);
            Room room = valueTaskRoom;
            RoomDto resultRoom = new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
            };
            return resultRoom;
        }
    }
}
