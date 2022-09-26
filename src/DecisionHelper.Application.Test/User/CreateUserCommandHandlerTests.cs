using System.Threading;
using System.Threading.Tasks;
using DecisionHelper.Application.Dependencies;
using DecisionHelper.Application.User.Commands.Create;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DecisionHelper.Application.Test.User
{
    public class CreateUserCommandHandlerTests
    {
        private CreateUserCommandHandler _sut;
        private Mock<IUserRepository> _userRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);

            _sut = new CreateUserCommandHandler(_userRepositoryMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _userRepositoryMock.VerifyAll();
        }

        [Test]
        public async Task Handler_should_create_user()
        {
            // arrange
            _userRepositoryMock.Setup(x => x.GetUserByEmail("fake-email@email.com", It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.User?)null);
            _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Domain.Entities.User>()))
                .ReturnsAsync(new Domain.Entities.User());

            // act
            var result = await _sut.Handle(new CreateUserCommand("fake-user", "fake-email@email.com"));

            // assert
            result.Value.User.Should().NotBeNull();
        }

        [Test]
        public async Task Handler_should_return__user_already_exists()
        {
            // arrange
            _userRepositoryMock.Setup(x => x.GetUserByEmail("fake-email@email.com", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Domain.Entities.User());

            // act
            var result = await _sut.Handle(new CreateUserCommand("fake-user", "fake-email@email.com"));

            // assert
            result.IsError.Should().BeTrue();
            result.FirstError.Description.Should().Be("User email already exists");
        }
    }
}
