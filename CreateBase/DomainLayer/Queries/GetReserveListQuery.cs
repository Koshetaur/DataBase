using System;
using System.Collections.Generic;
using MediatR;

namespace DomainLayer
{
    public class GetReserveListQuery : IRequest<List<ReserveDto>>
    { 
        public DateTime MinTime { get; set; }
        public DateTime MaxTime { get; set; }
    }
}
