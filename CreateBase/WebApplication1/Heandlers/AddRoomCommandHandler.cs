using System.Threading;
using System.Threading.Tasks;
using LibBase;
using MediatR;

namespace ReserveWebApp.Controllers
{
    public class AddRoomCommandHandler : AsyncRequestHandler<AddRoomCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddRoomCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        protected override async Task Handle(AddRoomCommand command, CancellationToken cancellationToken)
        {
            Room room = new Room
            {
                Name = command.Name,
            };
            await _unitOfWork.GetRepository<Room>().CreateAsync(room);
            await _unitOfWork.SaveAsync();
        }
    }
}
