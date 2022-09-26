using System;
using System.Threading;
using System.Threading.Tasks;
using DecisionHelper.Application.Dependencies;
using DecisionHelper.Application.UserChoice.Commands.Create;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DecisionHelper.Application.Test.UserChoice
{
    public class CreateUserChoiceCommandHandlerTests
    {
        private CreateUserChoiceCommandHandler _sut;
        private Mock<IUserChoiceRepository> _userChoiceRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IMessageRepository> _messageRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _userChoiceRepositoryMock = new Mock<IUserChoiceRepository>(MockBehavior.Strict);
            _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            _messageRepositoryMock = new Mock<IMessageRepository>(MockBehavior.Strict);

            _sut = new CreateUserChoiceCommandHandler(_userChoiceRepositoryMock.Object, _userRepositoryMock.Object,
                _messageRepositoryMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _userChoiceRepositoryMock.VerifyAll();
            _userRepositoryMock.VerifyAll();
            _messageRepositoryMock.VerifyAll();
        }

        [Test]
        public async Task Handle_should_create_user_choice()
        {
            // arrange
            var userId = Guid.NewGuid();
            var messageId = Guid.NewGuid();
            var messageResponse = new Domain.Entities.Message
            {
                AdventureId = Guid.NewGuid(),
                Id = messageId
            };
            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Domain.Entities.User());

            _messageRepositoryMock.Setup(x => x.GetByIdAsync(messageId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(messageResponse);

            _userChoiceRepositoryMock.Setup(x=>x.HasUserChoice(userId, messageId, messageResponse.AdventureId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _userChoiceRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Domain.Entities.UserChoice>()))
                .ReturnsAsync(new Domain.Entities.UserChoice());

            // act
            var result = await _sut.Handle(new CreateUserChoiceCommand(userId, messageId, Domain.Enums.Answer.Yes));

            // assert
            result.Should().NotBeNull();
        }

        [Test]
        public async Task Handle_should_reutn_User_Not_Found()
        {
            // arrange
            var userId = Guid.NewGuid();
            var messageId = Guid.NewGuid();
            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.User)null);

            // act
            var result = await _sut.Handle(new CreateUserChoiceCommand(userId, messageId, Domain.Enums.Answer.Yes));

            // assert
            result.IsError.Should().BeTrue();
            result.FirstError.Description.Should().Be("User not exists against specified id");
        }

        [Test]
        public async Task Handle_should_reutn_Message_Not_Found()
        {
            // arrange
            var userId = Guid.NewGuid();
            var messageId = Guid.NewGuid();
            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Domain.Entities.User());

            _messageRepositoryMock.Setup(x => x.GetByIdAsync(messageId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.Message)null);

            // act
            var result = await _sut.Handle(new CreateUserChoiceCommand(userId, messageId, Domain.Enums.Answer.Yes));

            // assert
            result.IsError.Should().BeTrue();
            result.FirstError.Description.Should().Be("Message not exists against specified id");
        }

        [Test]
        public async Task Handle_should_reutn_Duplicate_User_Choice()
        {
            // arrange
            var userId = Guid.NewGuid();
            var messageId = Guid.NewGuid();
            var messageResponse = new Domain.Entities.Message
            {
                AdventureId = Guid.NewGuid(),
                Id = messageId
            };
            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Domain.Entities.User());

            _messageRepositoryMock.Setup(x => x.GetByIdAsync(messageId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(messageResponse);

            _userChoiceRepositoryMock.Setup(x => x.HasUserChoice(userId, messageId, messageResponse.AdventureId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            

            // act
            var result = await _sut.Handle(new CreateUserChoiceCommand(userId, messageId, Domain.Enums.Answer.Yes));

            // assert
            result.IsError.Should().BeTrue();
            result.FirstError.Description.Should().Be("User choice for the selected adventure already exists");
        }
    }
}
