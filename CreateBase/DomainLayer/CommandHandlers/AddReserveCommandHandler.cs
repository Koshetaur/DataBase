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
            var query = _mapper.Map<VerifyReserveQuery>(command);

            var result = await QueryHandle(x => new VerifyReserveQueryHandler(x), query, cancellationToken);

            if (result)
            {
                Reserve reserve = _mapper.Map<Reserve>(command);
                reserve.User = _mapper.Map<User>(GetRepository<User>().Get(command.UserId));
                reserve.Room = _mapper.Map<Room>(GetRepository<Room>().Get(command.RoomId));
                await GetRepository<Reserve>().CreateAsync(reserve);
                await SaveAsync();
            }
        }
    }
}
