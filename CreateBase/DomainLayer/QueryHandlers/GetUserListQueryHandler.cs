using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibBase;

namespace DomainLayer
{
    public class GetUserListQueryHandler : QueryHandler<GetUserListQuery, List<UserDto>>
    {
        public GetUserListQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public override Task<List<UserDto>> Handle(GetUserListQuery query, CancellationToken cancellationToken)
        {
            var users = GetQuery<User>().ToList();
            List<UserDto> resultUsers = _mapper.Map<List<UserDto>>(users.ToList());
            return Task.FromResult(resultUsers);
        }
    }
}
