using MediatR;

namespace DomainLayer
{
    public class DeleteReserveCommand : IRequest
    {
        public int Id { get; set; }
    }
}
