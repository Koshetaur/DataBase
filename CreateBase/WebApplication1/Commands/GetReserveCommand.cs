using MediatR;

namespace ReserveWebApp.Controllers
{
    public class GetReserveCommand : IRequest<ReserveDto>
    {
        public int Id { get; set; }
    }
}
