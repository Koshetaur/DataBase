using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibBase;

namespace DomainLayer
{
    public class VerifyRoomQueryHandler : QueryHandler<VerifyRoomQuery, bool>
    {
        public VerifyRoomQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public override Task<bool> Handle(VerifyRoomQuery query, CancellationToken cancellationToken)
        {
            var reserves = GetQuery<Room>().ToList();
            var result = reserves.Any(res => res.Name == query.RoomName);
            return Task.FromResult(!result);
        }
    }
}
