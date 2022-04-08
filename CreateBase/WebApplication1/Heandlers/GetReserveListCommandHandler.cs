using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibBase;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ReserveWebApp.Controllers
{
    public class GetReserveListCommandHandler : IRequestHandler<GetReserveListCommand, List<ReserveDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetReserveListCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<ReserveDto>> Handle(GetReserveListCommand command, CancellationToken cancellationToken)
        {
            var reserves = _unitOfWork.GetRepository<Reserve>().Query().Include(res => res.User).Include(res => res.Room).Where(res => res.TimeEnd >= command.MinTime && res.TimeStart <= command.MaxTime).ToList();
            List<ReserveDto> resultReserves = new List<ReserveDto>();
            foreach (Reserve r in reserves)
            {
                resultReserves.Add(new ReserveDto
                {
                    Id = r.Id,
                    User = new UserDto
                    {
                        Id = r.User.Id,
                        Name = r.User.Name,
                        Surname = r.User.Surname
                    },
                    Room = new RoomDto
                    {
                        Id = r.Room.Id,
                        Name = r.Room.Name
                    },
                    TimeStart = r.TimeStart,
                    TimeEnd = r.TimeEnd
                });
            }
            return resultReserves;
        }
    }
}
