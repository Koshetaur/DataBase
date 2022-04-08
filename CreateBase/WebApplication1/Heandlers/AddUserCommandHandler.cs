using System.Threading;
using System.Threading.Tasks;
using LibBase;
using MediatR;

namespace ReserveWebApp.Controllers
{
    public class AddUserCommandHandler : AsyncRequestHandler<AddUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        protected override async Task Handle(AddUserCommand command, CancellationToken cancellationToken)
        {
            User user = new User
            {
                Name = command.Name,
                Surname = command.Surname
            };
            await _unitOfWork.GetRepository<User>().CreateAsync(user);
            await _unitOfWork.SaveAsync();
        }
    }
}
