using System.Threading;
using System.Threading.Tasks;
using LibBase;
using MediatR;

namespace DomainLayer
{
    public abstract class CommandHandler<TRequest> : AsyncRequestHandler<TRequest> where TRequest : IRequest
    {
        private readonly IUnitOfWork _unitOfWork;
        public CommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected IRepository<T> GetRepository<T>() where T : class
        {
            return _unitOfWork.GetRepository<T>();
        }

        protected async Task SaveAsync()
        {
            await _unitOfWork.SaveAsync();
        }

        protected override abstract Task Handle(TRequest request, CancellationToken cancellationToken);
    }

}
