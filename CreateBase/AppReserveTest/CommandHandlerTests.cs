using DomainLayer;
using FluentAssertions;
using LibBase;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppReserveTest
{
    class CommandHandlerTests
    {
        private readonly ApplicationContext _context;
        private readonly UnitOfWork<ApplicationContext> _unitOfWork;
        private readonly Room _room;
        private readonly User _user;
        private readonly Reserve _reserve;
        private readonly DateTime min = DateTime.Now;
        private readonly DateTime max = DateTime.Now.AddHours(1);
        public CommandHandlerTests()
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase("test");
            _context = new ApplicationContext(builder.Options);
            _context.Database.EnsureCreated();
            _unitOfWork = new UnitOfWork<ApplicationContext>(_context);
            _room = new Room { Name = "roomtest1" };
            _user = new User { Name = "usertest_name", Surname = "usertest_surname" };
            _reserve = new Reserve { User = _user, Room = _room, TimeStart = min, TimeEnd = max };
        }

        [SetUp]
        public void Setup()
        {
            var deletereserves = _context.Reservs.ToList();
            _context.Reservs.RemoveRange(deletereserves);
            var deleterooms = _context.Rooms.ToList();
            _context.Rooms.RemoveRange(deleterooms);
            var deleteusers = _context.Users.ToList();
            _context.Users.RemoveRange(deleteusers);
            _context.SaveChanges();
            _reserve.Id = 0; _reserve.Room = _room; _reserve.User = _user;
            _room.Id = 0;
            _user.Id = 0;
            _context.Rooms.Add(_room);
            _context.Users.Add(_user);
            _context.SaveChanges();
            _context.Reservs.Add(_reserve);
            _context.SaveChanges();
        }
        
        private class AddRoomHandlerInternal : AddRoomCommandHandler
        {
            public AddRoomHandlerInternal(IUnitOfWork unitOfWork) : base(unitOfWork)
            {
            }
            public async Task HandleInternal(AddRoomCommand command)
            {
                await Handle(command, CancellationToken.None);
            }
        }

        [Test]
        public async Task CommandHandler_AddRoomHandler_Ok()
        {
            //Arrange
            var room = new Room
            {
                Name = "qwerty"
            };
            var command = new AddRoomCommand { Name = room.Name };
            var handler = new AddRoomHandlerInternal(_unitOfWork);

            //Act
            await handler.HandleInternal(command);

            //Assert
            var testroom = _context.Rooms.FirstOrDefault(x => x.Id != _room.Id);
            testroom.Should().NotBeNull();
            testroom.Name.Should().Be(room.Name);

        }
        
        private class AddUserHandlerInternal : AddUserCommandHandler
        {
            public AddUserHandlerInternal(IUnitOfWork unitOfWork) : base(unitOfWork)
            {
            }
            public async Task HandleInternal(AddUserCommand command)
            {
                await Handle(command, CancellationToken.None);
            }
        }

        [Test]
        public async Task CommandHandler_AddUserHandler_Ok()
        {
            //Arrange
            var user = new User
            {
                Name = "name",
                Surname = "surname"
            };
            var command = new AddUserCommand { Name = user.Name, Surname = user.Surname };
            var handler = new AddUserHandlerInternal(_unitOfWork);

            //Act
            await handler.HandleInternal(command);

            //Assert
            var testuser = _context.Users.FirstOrDefault(x => x.Id != _user.Id);
            testuser.Should().NotBeNull();
            testuser.Name.Should().Be(user.Name);

        }
        
        private class AddReserveHandlerInternal : AddReserveCommandHandler
        {
            public AddReserveHandlerInternal(IUnitOfWork unitOfWork) : base(unitOfWork)
            {
            }
            public async Task HandleInternal(AddReserveCommand command)
            {
                await Handle(command, CancellationToken.None);
            }
        }

        [Test]
        public async Task CommandHandler_AddReserveHandler_Ok()
        {
            //Arrange
            var reserve = new Reserve
            {
                User = _user,
                Room = _room,
                TimeStart = min.AddDays(1),
                TimeEnd = max.AddDays(1)
            };
            var command = new AddReserveCommand { UserId = reserve.User.Id, RoomId = reserve.Room.Id, TimeStart = reserve.TimeStart, TimeEnd = reserve.TimeEnd };
            var handler = new AddReserveHandlerInternal(_unitOfWork);

            //Act
            await handler.HandleInternal(command);

            //Assert
            var testreserve = _context.Reservs.FirstOrDefault(x => x.Id != _reserve.Id);
            testreserve.Should().NotBeNull();
            testreserve.User.Name.Should().Be(_user.Name);

        }
   
        private class DeleteReserveHandlerInternal : DeleteReserveCommandHandler
        {
            public DeleteReserveHandlerInternal(IUnitOfWork unitOfWork) : base(unitOfWork)
            {
            }
            public async Task HandleInternal(DeleteReserveCommand command)
            {
                await Handle(command, CancellationToken.None);
            }
        }

        [Test]
        public async Task CommandHandler_DeleteReserveHandler_Ok()
        {
            //Arrange
            var command = new DeleteReserveCommand { Id = _reserve.Id };
            var handler = new DeleteReserveHandlerInternal(_unitOfWork);

            //Act
            await handler.HandleInternal(command);

            //Assert
            var testreserves = _context.Reservs.ToList();
            testreserves.Count.Should().Be(0);
        }
        
        private class EditReserveHandlerInternal : EditReserveCommandHandler
        {
            public EditReserveHandlerInternal(IUnitOfWork unitOfWork) : base(unitOfWork)
            {
            }
            public async Task HandleInternal(EditReserveCommand command)
            {
                await Handle(command, CancellationToken.None);
            }
        }

        [Test]
        public async Task CommandHandler_EditReserveHandler_Ok()
        {
            //Arrange
            var reserve = new Reserve
            {

                TimeStart = min.AddDays(1),
                TimeEnd = max.AddDays(1)
            };
            var command = new EditReserveCommand { Id = _reserve.Id, UserId = _reserve.User.Id, RoomId = _reserve.Room.Id, TimeStart = reserve.TimeStart, TimeEnd = reserve.TimeEnd };
            var handler = new EditReserveHandlerInternal(_unitOfWork);

            //Act
            await handler.HandleInternal(command);

            //Assert
            var testreserve = _context.Reservs.FirstOrDefault(x => x.Id == _reserve.Id);
            testreserve.TimeStart.Should().Be(reserve.TimeStart);

        }
    }
}
