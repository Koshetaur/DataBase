using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibBase;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ReserveWebApp.Controllers
{
    public class EditReserveCommandHandler : AsyncRequestHandler<EditReserveCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public EditReserveCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        protected override async Task Handle(EditReserveCommand command, CancellationToken cancellationToken)
        {
            Reserve reserve = _unitOfWork.GetRepository<Reserve>().Query().Include(res => res.User).Include(res => res.Room).SingleOrDefault(res => res.Id == command.Id);
            reserve.UserId = command.UserId;
            reserve.User = _unitOfWork.GetRepository<User>().Get(command.UserId);
            reserve.RoomId = command.RoomId;
            reserve.Room = _unitOfWork.GetRepository<Room>().Get(command.RoomId);
            reserve.TimeStart = command.TimeStart;
            reserve.TimeEnd = command.TimeEnd;
            await _unitOfWork.SaveAsync();
        }
    }
}
