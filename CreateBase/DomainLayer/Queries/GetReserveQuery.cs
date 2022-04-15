using MediatR;

namespace DomainLayer
{
    public class GetReserveQuery : IRequest<ReserveDto>
    {
        public int Id { get; set; }
    }
}
