using System.Threading;
using System.Threading.Tasks;
using LibBase;
using MediatR;

namespace DomainLayer
{
    public class AddReserveCommandHandler : CommandHandler<AddReserveCommand>
    {
        public AddReserveCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        protected override async Task Handle(AddReserveCommand command, CancellationToken cancellationToken)
        {
            Reserve reserve = new Reserve
            {
                UserId = command.UserId,
                User = GetRepository<User>().Get(command.UserId),
                RoomId = command.RoomId,
                Room = GetRepository<Room>().Get(command.RoomId),
                TimeStart = command.TimeStart,
                TimeEnd = command.TimeEnd
            };
            await GetRepository<Reserve>().CreateAsync(reserve);
            await SaveAsync();
        }
    }
}
