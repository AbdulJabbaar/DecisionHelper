using System;
using System.Threading;
using System.Threading.Tasks;
using DecisionHelper.Application.Dependencies;
using DecisionHelper.Application.Message;
using DecisionHelper.Application.Message.Command.Create;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DecisionHelper.Application.Test.Message
{
    public class CreateMessageCommandHandlerTests
    {
        private CreateMessageCommandHandler _sut;
        private Mock<IMessageRepository> _messageRepositoryMock;
        private Mock<IAdventureRepository> _adventureRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _adventureRepositoryMock = new Mock<IAdventureRepository>(MockBehavior.Strict);
            _messageRepositoryMock = new Mock<IMessageRepository>(MockBehavior.Strict);

            _sut = new CreateMessageCommandHandler(_messageRepositoryMock.Object, _adventureRepositoryMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _adventureRepositoryMock.VerifyAll();
            _messageRepositoryMock.VerifyAll();
        }

        [Test]
        public async Task Handle_should_return_MessageResult()
        {
            // arrange
            var adventureId = Guid.NewGuid();

            _adventureRepositoryMock.Setup(x => x.GetByIdAsync(adventureId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Domain.Entities.Adventure());

            _messageRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Domain.Entities.Message>()))
                .ReturnsAsync(new Domain.Entities.Message());

            // act
            var result = await _sut.Handle(new CreateMessageCommand("fake-title", false, adventureId, null, null));

            // assert
            result.Value.Should().NotBeNull();
            result.Value.Should().BeOfType<MessageResult>();
        }

        [Test]
        public async Task Handle_should_return_Adventure_Not_Found()
        {
            // arrange
            var adventureId = Guid.NewGuid();

            _adventureRepositoryMock.Setup(x => x.GetByIdAsync(adventureId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.Adventure)null);

            // act
            var result = await _sut.Handle(new CreateMessageCommand("fake-title", false, adventureId, null, null));

            // assert
            result.IsError.Should().BeTrue();
            result.FirstError.Description.Should().Be("Adventure not exists against specified id");
        }

        [Test]
        public async Task Handle_should_return_Invalid_Parent_MessageId()
        {
            // arrange
            var adventureId = Guid.NewGuid();
            var parentMessageId = Guid.NewGuid();

            _adventureRepositoryMock.Setup(x => x.GetByIdAsync(adventureId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Domain.Entities.Adventure());

            _messageRepositoryMock.Setup(x => x.GetByIdAsync(parentMessageId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.Message)null);

            // act
            var result = await _sut.Handle(new CreateMessageCommand("fake-title", false, adventureId, parentMessageId, null));

            // assert
            result.IsError.Should().BeTrue();
            result.FirstError.Description.Should().Be("Invalid message parent id");
        }
    }
}
