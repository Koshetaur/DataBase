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
            List<ReserveDto> resultReserves = _mapper.Map<List<ReserveDto>>(reserves.ToList());
            return Task.FromResult(resultReserves);
        }
    }
}
