using System.Threading;
using System.Threading.Tasks;
using LibBase;
using MediatR;

namespace ReserveWebApp.Controllers
{
    public class DeleteReserveCommandHandler : AsyncRequestHandler<DeleteReserveCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteReserveCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        protected override async Task Handle(DeleteReserveCommand command, CancellationToken cancellationToken)
        {
            _unitOfWork.GetRepository<Reserve>().Delete(command.Id);
            await _unitOfWork.SaveAsync();
        }
    }
}
