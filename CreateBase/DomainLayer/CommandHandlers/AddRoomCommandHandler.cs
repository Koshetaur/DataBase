using System.Threading;
using System.Threading.Tasks;
using LibBase;

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
            var query = _mapper.Map<VerifyRoomQuery>(command);
            var result = await QueryHandle(x => new VerifyRoomQueryHandler(x), query, cancellationToken);
            if (result)
            {
                Room room = _mapper.Map<Room>(command);
                await GetRepository<Room>().CreateAsync(room);
                await SaveAsync();
            }
        }
    }
}
