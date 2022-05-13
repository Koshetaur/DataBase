using AutoMapper;
using DomainLayer;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReserveWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReserveWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public EventController(IMediator mediator)
        {
            _mediator = mediator;
            _mapper = new MapperConfiguration(x =>
            {
                x.CreateMap<ReserveDto, EventModel>()
                .ForMember(dst => dst.start_date, opt => opt.MapFrom(src => src.TimeStart))
                .ForMember(dst => dst.end_date, opt => opt.MapFrom(src => src.TimeEnd))
                .ForMember(dst => dst.holder, opt => opt.MapFrom(src => $"{src.User.Name} {src.User.Surname}"))
                .ForMember(dst => dst.Room, opt => opt.MapFrom(src => src.Room.Name))
                .ForMember(dst => dst.Text, opt => opt.MapFrom(src => $"ROOM RESERVED"));
                x.CreateMap<RoomDto, Pair>()
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Label, opt => opt.MapFrom(src => src.Name));
                x.CreateMap<EventModel, AddReserveCommand>()
                .ForMember(dst => dst.TimeStart, opt => opt.MapFrom(src => src.start_date))
                .ForMember(dst => dst.TimeEnd, opt => opt.MapFrom(src => src.end_date));
            }).CreateMapper();
        }

        public async Task<bool> VerifyReserve(int SelectedRoomId, DateTime StartTime, DateTime EndTime, int Id)
        {
            var result = await _mediator.Send(new VerifyReserveQuery
            {
                Id = Id,
                RoomId = SelectedRoomId,
                TimeStart = StartTime,
                TimeEnd = EndTime
            });
            return result;
        }

        [HttpGet]
        public async Task<EventListModel> Get(DateTime from, DateTime to)
        {
            var reserves = await _mediator.Send(new GetReserveListQuery
            {
                MinTime = from,
                MaxTime = to
            });
            var events = _mapper.Map<List<EventModel>>(reserves);
            var rooms = await _mediator.Send(new GetRoomListQuery());
            var roomsPair = _mapper.Map<List<Pair>>(rooms);
            return new EventListModel
            {
                Data = events,
                Collections = new Collections
                {
                    Rooms = roomsPair
                }
            };
        }

        [HttpGet("{id}")]
        public async Task<EventModel> Get(int id)
        {
            var reserve = await _mediator.Send(new GetReserveQuery { Id = id });
            var eventr = _mapper.Map<EventModel>(reserve);
            return eventr;
        }

        [HttpPost]
        public async Task<IActionResult> Post(RequestModel model)
        {
            switch (model.Action)
            {
                case "delete":
                    {
                        await _mediator.Send(new DeleteReserveCommand { Id = model.Data.Id });
                        break;
                    }
                case "insert":
                    {
                        await _mediator.Send(_mapper.Map<AddReserveCommand>(model));
                        break;
                    }
                case "edit":
                    {
                        await _mediator.Send(_mapper.Map<EditReserveCommand>(model));
                        break;
                    }
            }
            return Ok(new { tid = model.Data.Id, action = "" });
        }
    }
}
