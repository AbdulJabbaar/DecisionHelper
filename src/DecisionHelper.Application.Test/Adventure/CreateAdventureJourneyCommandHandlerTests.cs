using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DecisionHelper.Application.Adventure.Commands.CreateAdventureJourney;
using DecisionHelper.Application.Dependencies;
using DecisionHelper.Domain.Enums;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DecisionHelper.Application.Test.Adventure
{
    public class CreateAdventureJourneyCommandHandlerTests
    {
        private CreateAdventureJourneyCommandHandler _sut;
        private Mock<IMessageRepository> _messageRepositoryMock;
        private Mock<IAdventureRepository> _adventureRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _adventureRepositoryMock = new Mock<IAdventureRepository>(MockBehavior.Strict);
            _messageRepositoryMock = new Mock<IMessageRepository>(MockBehavior.Strict);

            _sut = new CreateAdventureJourneyCommandHandler(_messageRepositoryMock.Object,
                _adventureRepositoryMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _adventureRepositoryMock.VerifyAll();
            _messageRepositoryMock.VerifyAll();
        }

        [Test]
        public async Task Handle_should_create_Adventure_Messages()
        {
            // arrange
            var adventureId = Guid.NewGuid();
            var input = new CreateAdventureJourneyCommand(adventureId, new AdventureMessage
            {
                Title = "You want a phone?",
                IsQuestion = true,
                Messages = new List<AdventureMessage>()
                {
                    new AdventureMessage()
                    {
                        Title = "Get it",
                        IsQuestion = false,
                        ByAnswer = Answer.Yes
                    },
                    new AdventureMessage()
                    {
                        Title = "Run 100 miles first",
                        IsQuestion = false,
                        ByAnswer = Answer.No
                    }
                }
            });

            _adventureRepositoryMock.Setup(x => x.GetByIdAsync(adventureId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Domain.Entities.Adventure());

            _messageRepositoryMock.Setup(x=>x.IsAdventureHasMessages(adventureId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _messageRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Domain.Entities.Message>()))
                .ReturnsAsync(It.IsAny<Domain.Entities.Message>());
            _messageRepositoryMock.Setup(x => x.GetAllMessagesByAdventureId(adventureId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<IEnumerable<Domain.Entities.Message>>());

            // act
            var result = await _sut.Handle(input);

            // assert
            result.Should().NotBeNull();
        }

        [Test]
        public async Task Handle_should_create_Adventure_Not_Found()
        {
            // arrange
            var adventureId = Guid.NewGuid();
            var input = new CreateAdventureJourneyCommand(adventureId, new AdventureMessage());

            _adventureRepositoryMock.Setup(x => x.GetByIdAsync(adventureId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.Adventure)null);

            // act
            var result = await _sut.Handle(input);

            // assert
            result.IsError.Should().BeTrue();
            result.FirstError.Description.Should().Be("Adventure not exists against specified id");
        }

        [Test]
        public async Task Handle_should_create_Adventure_Already_Has_Journey_Defined()
        {
            // arrange
            var adventureId = Guid.NewGuid();
            var input = new CreateAdventureJourneyCommand(adventureId, new AdventureMessage());

            _adventureRepositoryMock.Setup(x => x.GetByIdAsync(adventureId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Domain.Entities.Adventure());

            _messageRepositoryMock.Setup(x => x.IsAdventureHasMessages(adventureId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // act
            var result = await _sut.Handle(input);

            // assert
            result.IsError.Should().BeTrue();
            result.FirstError.Description.Should().Be("Adventure already has Journey defined");
        }
    }
}
