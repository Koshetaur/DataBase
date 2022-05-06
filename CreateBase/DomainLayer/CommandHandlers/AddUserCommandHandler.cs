using System.Threading;
using System.Threading.Tasks;
using LibBase;
using MediatR;

namespace DomainLayer
{
    public class AddUserCommandHandler : CommandHandler<AddUserCommand>
    {
        public AddUserCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        protected override async Task Handle(AddUserCommand command, CancellationToken cancellationToken)
        {
            User user = _mapper.Map<User>(command);
            await GetRepository<User>().CreateAsync(user);
            await SaveAsync();
        }
    }
}
