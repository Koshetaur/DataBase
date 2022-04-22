using LibBase;
using DomainLayer;
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using FluentAssertions;

namespace AppReserveTest
{
    class QueryHandlerTests
    {
        private readonly ApplicationContext _context;
        private readonly UnitOfWork<ApplicationContext> _unitOfWork;
        private readonly Room _room;
        private readonly User _user;
        private readonly Reserve _reserve;
        private readonly DateTime min = DateTime.Now;
        private readonly DateTime max = DateTime.Now.AddHours(1);
        public QueryHandlerTests()
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

        [Test]
        public async Task QueryHandler_GetRoomQueryHandler_Ok()
        {
            //Arrange
            var query = new GetRoomQuery { Id = _room.Id };
            var handler = new GetRoomQueryHandler(_unitOfWork);

            //Act
            RoomDto testroom = await handler.Handle(query, CancellationToken.None);

            //Assert
            testroom.Should().NotBeNull();
            testroom.Name.Should().Be(_room.Name);
        }

        [Test]
        public async Task QueryHandler_GetRoomListQueryHandler_Ok()
        {
            //Arrange
            var query = new GetRoomListQuery();
            var handler = new GetRoomListQueryHandler(_unitOfWork);

            //Act
            List<RoomDto> testrooms = await handler.Handle(query, CancellationToken.None);

            //Assert
            testrooms.Count.Should().Be(1);
        }

        [Test]
        public async Task QueryHandler_GetUserQueryHandler_Ok()
        {
            //Arrange
            var query = new GetUserQuery { Id = _user.Id };
            var handler = new GetUserQueryHandler(_unitOfWork);

            //Act
            UserDto testuser = await handler.Handle(query, CancellationToken.None);

            //Assert
            testuser.Should().NotBeNull();
            testuser.Name.Should().Be(_user.Name);
            testuser.Surname.Should().Be(_user.Surname);
        }

        [Test]
        public async Task QueryHandler_GetUserListQueryHandler_Ok()
        {
            //Arrange
            var query = new GetUserListQuery();
            var handler = new GetUserListQueryHandler(_unitOfWork);

            //Act
            List<UserDto> testusers = await handler.Handle(query, CancellationToken.None);

            //Assert
            testusers.Count.Should().Be(1);
        }

        [Test]
        public async Task QueryHandler_GetReserveQueryHandler_Ok()
        {
            //Arrange
            var query = new GetReserveQuery { Id = _reserve.Id };
            var handler = new GetReserveQueryHandler(_unitOfWork);

            //Act
            ReserveDto testreserve = await handler.Handle(query, CancellationToken.None);

            //Assert
            testreserve.Should().NotBeNull();
            testreserve.User.Id.Should().Be(_user.Id);
            testreserve.Room.Id.Should().Be(_room.Id);
        }

        [Test]
        public async Task QueryHandler_GetReserveListQueryHandler_Ok()
        {
            //Arrange
            var query = new GetReserveListQuery { MaxTime = max, MinTime = min };
            var query_null = new GetReserveListQuery { MaxTime = max.AddDays(7), MinTime = min.AddDays(7) };
            var handler = new GetReserveListQueryHandler(_unitOfWork);

            //Act
            List<ReserveDto> testreserves = await handler.Handle(query, CancellationToken.None);
            List<ReserveDto> testreserves_null = await handler.Handle(query_null, CancellationToken.None);

            //Assert
            testreserves.Count.Should().Be(1);
            testreserves_null.Count.Should().Be(0);
        }
    }
}
