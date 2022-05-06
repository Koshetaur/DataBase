using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibBase;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DomainLayer
{
    public class EditReserveCommandHandler : CommandHandler<EditReserveCommand>
    {
        private IUnitOfWork _unitOfWorkLocal;
        public EditReserveCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWorkLocal = unitOfWork;
        }
        protected override async Task Handle(EditReserveCommand command, CancellationToken cancellationToken)
        {
            var query = _mapper.Map<VerifyReserveQuery>(command);
            var result = await QueryHandle(x => new VerifyReserveQueryHandler(x), query, cancellationToken);
            if (result)
            {
                Reserve reserve = GetRepository<Reserve>().Query().Include(res => res.User).Include(res => res.Room).SingleOrDefault(res => res.Id == command.Id);
                reserve.UserId = command.UserId;
                reserve.User = GetRepository<User>().Get(command.UserId);
                reserve.RoomId = command.RoomId;
                reserve.Room = GetRepository<Room>().Get(command.RoomId);
                reserve.TimeStart = command.TimeStart;
                reserve.TimeEnd = command.TimeEnd;
                await SaveAsync();
            }
        }
    }
}
