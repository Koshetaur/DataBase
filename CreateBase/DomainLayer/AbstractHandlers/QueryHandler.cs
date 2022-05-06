using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using LibBase;
using MediatR;

namespace DomainLayer
{
    public abstract class QueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public IMapper _mapper;
        public QueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = new MapperConfiguration(x =>
            {
                x.CreateMap<User, UserDto>();
                x.CreateMap<Room, RoomDto>();
                x.CreateMap<Reserve, ReserveDto>();
            }).CreateMapper();
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
