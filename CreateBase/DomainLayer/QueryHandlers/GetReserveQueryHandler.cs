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
            ReserveDto resultReserve = _mapper.Map<ReserveDto>(reserve);
            return resultReserve;
        }
    }
}
