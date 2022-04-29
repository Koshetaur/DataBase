using System.Threading;
using System.Threading.Tasks;
using LibBase;
using MediatR;

namespace DomainLayer
{
    public class AddRoomCommandHandler : CommandHandler<AddRoomCommand>
    {
        private IUnitOfWork _unitOfWorkLocal;
        public AddRoomCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWorkLocal = unitOfWork;
        }
        protected override async Task Handle(AddRoomCommand command, CancellationToken cancellationToken)
        {
            var query = new VerifyRoomQuery
            {
                RoomName = command.Name
            };
            var result = await QueryHandle(x => new VerifyRoomQueryHandler(x), query, cancellationToken);
            if (result)
            {
                Room room = new Room
                {
                    Name = command.Name,
                };
                await GetRepository<Room>().CreateAsync(room);
                await SaveAsync();
            }
        }
    }
}
