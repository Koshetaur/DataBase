using MediatR;

namespace ReserveWebApp.Controllers
{
    public class DeleteReserveCommand : IRequest
    {
        public int Id { get; set; }
    }
}
