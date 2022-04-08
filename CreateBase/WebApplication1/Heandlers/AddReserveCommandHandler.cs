using System.Threading;
using System.Threading.Tasks;
using LibBase;
using MediatR;

namespace ReserveWebApp.Controllers
{
    public class AddReserveCommandHandler : AsyncRequestHandler<AddReserveCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddReserveCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        protected override async Task Handle(AddReserveCommand command, CancellationToken cancellationToken)
        {
            Reserve reserve = new Reserve
            {
                UserId = command.UserId,
                User = _unitOfWork.GetRepository<User>().Get(command.UserId),
                RoomId = command.RoomId,
                Room = _unitOfWork.GetRepository<Room>().Get(command.RoomId),
                TimeStart = command.TimeStart,
                TimeEnd = command.TimeEnd
            };
            await _unitOfWork.GetRepository<Reserve>().CreateAsync(reserve);
            await _unitOfWork.SaveAsync();
        }
    }
}
