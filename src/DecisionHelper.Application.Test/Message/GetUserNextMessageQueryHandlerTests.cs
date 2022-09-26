using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DecisionHelper.Application.Dependencies;
using DecisionHelper.Application.Message.Queries.UserNextMessage;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DecisionHelper.Application.Test.Message
{
    public class GetUserNextMessageQueryHandlerTests
    {
        private GetUserNextMessageQueryHandler _sut;
        private Mock<IMessageRepository> _messageRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IAdventureRepository> _adventureRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _messageRepositoryMock = new Mock<IMessageRepository>(MockBehavior.Strict);
            _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            _adventureRepositoryMock = new Mock<IAdventureRepository>(MockBehavior.Strict);

            _sut = new GetUserNextMessageQueryHandler(_messageRepositoryMock.Object, _userRepositoryMock.Object,
                _adventureRepositoryMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _messageRepositoryMock.VerifyAll();
            _userRepositoryMock.VerifyAll();
            _adventureRepositoryMock.VerifyAll();
        }

        [Test]
        public async Task Handle_should_return__MessageResult()
        {
            // arrange
            var userId = Guid.NewGuid();
            var adventureId = Guid.NewGuid();
            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Domain.Entities.User());

            _adventureRepositoryMock.Setup(x => x.GetByIdAsync(adventureId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Domain.Entities.Adventure());

            _messageRepositoryMock.Setup(x => x.GetUserNextChoice(userId, adventureId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Domain.Entities.Message());

            // act
            var result = await _sut.Handle(new GetUserNextMessageQuery(adventureId, userId));

            // assert
            result.Value.Message.Should().NotBeNull();
        }

        [Test]
        public async Task Handle_should_return__User_Not_Found()
        {
            // arrange
            var userId = Guid.NewGuid();
            var adventureId = Guid.NewGuid();
            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.User)null);

            // act
            var result = await _sut.Handle(new GetUserNextMessageQuery(adventureId, userId));

            // assert
            result.IsError.Should().BeTrue();
            result.FirstError.Description.Should().Be("User not exists against specified id");
        }

        [Test]
        public async Task Handle_should_return__Adventure_Not_Found()
        {
            // arrange
            var userId = Guid.NewGuid();
            var adventureId = Guid.NewGuid();
            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Domain.Entities.User());

            _adventureRepositoryMock.Setup(x => x.GetByIdAsync(adventureId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.Adventure)null);

            // act
            var result = await _sut.Handle(new GetUserNextMessageQuery(adventureId, userId));

            // assert
            result.IsError.Should().BeTrue();
            result.FirstError.Description.Should().Be("Adventure not exists against specified id");
        }
    }
}
