using System;
using System.Collections.Generic;
using MediatR;

namespace ReserveWebApp.Controllers
{
    public class GetReserveListCommand : IRequest<List<ReserveDto>>
    { 
        public DateTime MinTime { get; set; }
        public DateTime MaxTime { get; set; }
    }
}
