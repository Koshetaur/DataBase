using System.Threading;
using System.Threading.Tasks;
using LibBase;
using Microsoft.EntityFrameworkCore;

namespace DomainLayer
{
    public class GetReserveQueryHandler : QueryHandler<GetReserveQuery, ReserveDto>
    {
        public GetReserveQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public override async Task<ReserveDto> Handle(GetReserveQuery query, CancellationToken cancellationToken)
        {
            Reserve reserve = await GetQuery<Reserve>().Include(res => res.User).Include(res => res.Room).SingleOrDefaultAsync(res => res.Id == query.Id, cancellationToken);
            if (reserve == null)
            {
                return null;
            }
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
