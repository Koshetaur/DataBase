using System.Threading;
using System.Threading.Tasks;
using LibBase;
using MediatR;

namespace DomainLayer
{
    public class AddRoomCommandHandler : CommandHandler<AddRoomCommand>
    {
        public AddRoomCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        protected override async Task Handle(AddRoomCommand command, CancellationToken cancellationToken)
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
