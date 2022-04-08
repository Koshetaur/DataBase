using System.Threading;
using System.Threading.Tasks;
using LibBase;
using MediatR;

namespace ReserveWebApp.Controllers
{
    public class GetUserCommandHandler : IRequestHandler<GetUserCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<UserDto> Handle(GetUserCommand command, CancellationToken cancellationToken)
        {
            ValueTask<User> valueTaskUser = await _unitOfWork.GetRepository<User>().GetAsync(command.Id);
            User user = valueTaskUser;
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
