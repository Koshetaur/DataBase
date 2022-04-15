using System.Threading;
using System.Threading.Tasks;
using LibBase;
using MediatR;

namespace DomainLayer
{
    public class DeleteReserveCommandHandler : CommandHandler<DeleteReserveCommand>
    {
        public DeleteReserveCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        protected override async Task Handle(DeleteReserveCommand command, CancellationToken cancellationToken)
        {
            GetRepository<Reserve>().Delete(command.Id);
            await SaveAsync();
        }
    }
}
