using System.Threading;
using System.Threading.Tasks;
using LibBase;

namespace DomainLayer
{
    public class GetUserQueryHandler : QueryHandler<GetUserQuery, UserDto>
    {
        public GetUserQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public override async Task<UserDto> Handle(GetUserQuery query, CancellationToken cancellationToken)
        {
            User user = await GetAsync<User>(query.Id);
            UserDto resultUser = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname
            };
            return resultUser;
        }
    }
}
