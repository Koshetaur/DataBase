using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibBase;

namespace DomainLayer
{
    public class VerifyReserveQueryHandler : QueryHandler<VerifyReserveQuery, bool>
    {
        public VerifyReserveQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public override Task<bool> Handle(VerifyReserveQuery query, CancellationToken cancellationToken)
        {
            var reserves = GetQuery<Reserve>().Where(res => res.TimeEnd >= query.TimeStart && res.TimeStart <= query.TimeEnd).ToList();
            var result = reserves.Any(res => res.TimeEnd >= query.TimeStart && res.TimeStart <= query.TimeEnd && res.RoomId == query.RoomId && res.Id != query.Id);
            return Task.FromResult(!result);
        }
    }
}