using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibBase;
using MediatR;

namespace DomainLayer
{
    public abstract class QueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public QueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected IQueryable<T> GetQuery<T>() where T : class
        {
            return _unitOfWork.GetRepository<T>().Query();
        }

        protected async Task<T> GetAsync<T>(int id) where T : class
        {
            return await _unitOfWork.GetRepository<T>().GetAsync(id);
        }

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

    }

}
