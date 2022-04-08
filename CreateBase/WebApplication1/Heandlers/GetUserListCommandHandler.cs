using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibBase;
using MediatR;

namespace ReserveWebApp.Controllers
{
    public class GetUserListCommandHandler : IRequestHandler<GetUserListCommand, List<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetUserListCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<UserDto>> Handle(GetUserListCommand command, CancellationToken cancellationToken)
        {
            var users = _unitOfWork.GetRepository<User>().Query().ToList();
            List<UserDto> resultUsers = new List<UserDto>();
            foreach (User u in users)
            {
                resultUsers.Add(new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Surname = u.Surname
                });
            }
            return resultUsers;
        }
    }
}
