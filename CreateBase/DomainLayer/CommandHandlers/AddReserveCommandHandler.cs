using System.Data.Entity;
using System.Linq;
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
            var query = new VerifyReserveQuery { 
                Id = 0,
                RoomId = command.RoomId,
                TimeStart = command.TimeStart,
                TimeEnd = command.TimeEnd};

            var result = await QueryHandle(x => new VerifyReserveQueryHandler(x), query, cancellationToken);

            if (result)
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
}
