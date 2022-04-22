using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using LibBase;
using System.Linq;
using FluentAssertions;
using System;
using System.Threading.Tasks;

namespace AppReserveTest
{
    public class RepositoryTests
    {
        private readonly ApplicationContext _context;
        private readonly Room _room;

        public RepositoryTests()
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseInMemoryDatabase("test");
            _context = new ApplicationContext(builder.Options);
            _context.Database.EnsureCreated();
            _room = new Room { Name = "roomtest1" };
        }

        [SetUp]
        public void Setup()
        {
            var deleterooms = _context.Rooms.ToList();
            _context.Rooms.RemoveRange(deleterooms);
            _context.SaveChanges();
            _room.Id = 0;
            _context.Rooms.Add(_room);
            _context.SaveChanges();
        }

        [Test]
        public void Repository_Create_Ok()
        {

            // Arrange
            var room = new Room
            {
                Name = "qwerty"
            };

            // Act
            var repository = new Repository<Room>(_context);
            repository.Create(room);
            _context.SaveChanges();

            // Assert
            var testroom = _context.Rooms.FirstOrDefault(x => x.Id != _room.Id);
            testroom.Should().NotBeNull();
            testroom.Name.Should().Be(room.Name);
        }

        [Test]
        public async Task Repository_CreateAsync_Ok()
        {

            // Arrange
            var room = new Room
            {
                Name = "qwerty"
            };

            // Act
            var repository = new Repository<Room>(_context);
            await repository.CreateAsync(room);
            _context.SaveChanges();

            // Assert
            var testroom = _context.Rooms.FirstOrDefault(x => x.Id != _room.Id);
            testroom.Should().NotBeNull();
            testroom.Name.Should().Be(room.Name);
        }

        [Test]
        public void Repository_Delete_Ok()
        {

            // Arrange
            var testroom = _context.Rooms.First();

            // Act
            var repository = new Repository<Room>(_context);
            repository.Delete(testroom.Id);
            _context.SaveChanges();

            // Assert
            var testrooms = _context.Rooms.ToList();
            testrooms.Count.Should().Be(0);
        }

        [Test]
        public void Repository_Get_Ok()
        {

            // Arrange
            

            // Act
            var repository = new Repository<Room>(_context);
            var testroom = repository.Get(_room.Id);

            // Assert
            testroom.Name.Should().Be(_room.Name);
        }

        [Test]
        public async Task Repository_GetAsync_Ok()
        {

            // Arrange


            // Act
            var repository = new Repository<Room>(_context);
            var testroom = await repository.GetAsync(_room.Id);

            // Assert
            testroom.Name.Should().Be(_room.Name);
        }

        [Test]
        public void Repository_Query_Ok()
        {

            // Arrange


            // Act
            var repository = new Repository<Room>(_context);
            var testquery = repository.Query();

            // Assert
            testquery.FirstOrDefault(x => x.Id == _room.Id).Name.Should().Be(_room.Name);
        }

      /*  [Test]
        public void Repository_Create_Error()
        {

            // Arrange
            var room = new Room
            {
                Name = _room.Name
            };

            // Act
            var repository = new Repository<Room>(_context);
            repository.Create(room);
            _context.SaveChanges();

            // Assert
            var testrooms = _context.Rooms.ToList();
            testrooms.Count.Should().Be(2);
            //_context.Invoking(x => x.SaveChanges()).Should().Throw<InvalidOperationException>();
        }*/
    }
}