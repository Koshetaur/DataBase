using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibBase;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ReserveWebApp.Controllers
{
    public class GetReserveCommandHandler : IRequestHandler<GetReserveCommand, ReserveDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetReserveCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ReserveDto> Handle(GetReserveCommand command, CancellationToken cancellationToken)
        {
            Reserve reserve = _unitOfWork.GetRepository<Reserve>().Query().Include(res => res.User).Include(res => res.Room).SingleOrDefault(res => res.Id == command.Id);
            ReserveDto resultReserve = new ReserveDto
            {
                Id = reserve.Id,
                User = new UserDto {
                    Id = reserve.User.Id,
                    Name = reserve.User.Name,
                    Surname = reserve.User.Surname
                },
                Room = new RoomDto
                {
                    Id = reserve.Room.Id,
                    Name = reserve.Room.Name
                },
                TimeStart = reserve.TimeStart,
                TimeEnd = reserve.TimeEnd
            };
            return resultReserve;
        }
    }
}
