using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibBase;
using Microsoft.EntityFrameworkCore;

namespace DomainLayer
{
    public class GetReserveListQueryHandler : QueryHandler<GetReserveListQuery, List<ReserveDto>>
    {
        public GetReserveListQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public override Task<List<ReserveDto>> Handle(GetReserveListQuery query, CancellationToken cancellationToken)
        {
            var reserves = GetQuery<Reserve>().Include(res => res.User).Include(res => res.Room).Where(res => res.TimeEnd >= query.MinTime && res.TimeStart <= query.MaxTime).ToList();
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
            return Task.FromResult(resultReserves);
        }
    }
}
