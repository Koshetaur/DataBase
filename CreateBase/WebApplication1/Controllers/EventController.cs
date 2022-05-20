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
                x.CreateMap<ReserveDto, DayPilotEventModel>()
                .ForMember(dst => dst.Start, opt => opt.MapFrom(src => src.TimeStart))
                .ForMember(dst => dst.End, opt => opt.MapFrom(src => src.TimeEnd))
                .ForMember(dst => dst.Text, opt => opt.MapFrom(src => $"{src.User.Name} {src.User.Surname} reserved {src.Room.Name}"))
                .ForMember(dst => dst.BarColor, opt => opt.MapFrom(src => $"#6aa84f"));
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
        public async Task<DayPilotEventListModel> Event(/*DateTime from, DateTime to*/)
        {
            var reserves = await _mediator.Send(new GetReserveListQuery
            {
                MinTime = DateTime.Today,
                MaxTime = DateTime.Today.AddDays(7)
            });
            var events = _mapper.Map<List<DayPilotEventModel>>(reserves);
            return new DayPilotEventListModel
            {
                DPEvents = events
            };
        }

        /*[HttpGet("{id}")]
        public async Task<EventModel> Get(int id)
        {
            var reserve = await _mediator.Send(new GetReserveQuery { Id = id });
            var eventr = _mapper.Map<EventModel>(reserve);
            return eventr;
        }

        [HttpPost]
        public async Task<IActionResult> Post(RequestApiModel model)
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
        }*/
    }
}
