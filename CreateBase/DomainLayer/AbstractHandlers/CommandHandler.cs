using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using LibBase;
using MediatR;

namespace DomainLayer
{
    public abstract class CommandHandler<TRequest> : AsyncRequestHandler<TRequest> where TRequest : IRequest
    {
        private readonly IUnitOfWork _unitOfWork;
        public IMapper _mapper;
        public CommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = new MapperConfiguration(x =>
            {
                x.CreateMap<EditReserveCommand, VerifyReserveQuery>();
                x.CreateMap<AddReserveCommand, VerifyReserveQuery>();
                x.CreateMap<AddReserveCommand, Reserve>();
                x.CreateMap<AddRoomCommand, VerifyRoomQuery>();
                x.CreateMap<AddUserCommand, User>();
                x.CreateMap<AddRoomCommand, Room>();
                x.CreateMap<UserDto, User>();
                x.CreateMap<RoomDto, Room>();
            }).CreateMapper();
        }

        protected IRepository<T> GetRepository<T>() where T : class
        {
            return _unitOfWork.GetRepository<T>();
        }

        protected Task<TResp> QueryHandle<TReq, TResp>(Func<IUnitOfWork, QueryHandler<TReq, TResp>> action, TReq query, CancellationToken token)
            where TReq : IRequest<TResp>
        {
            return action(_unitOfWork).Handle(query, token);
        }

        protected async Task SaveAsync()

        {
            await _unitOfWork.SaveAsync();
        }

        protected override abstract Task Handle(TRequest request, CancellationToken cancellationToken);
    }

}
