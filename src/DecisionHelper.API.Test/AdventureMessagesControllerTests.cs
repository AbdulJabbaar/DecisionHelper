using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DecisionHelper.API.Controllers;
using DecisionHelper.Application.Adventure;
using DecisionHelper.Application.Adventure.Commands.CreateAdventureJourney;
using DecisionHelper.Application.Message;
using DecisionHelper.Application.Message.Command.Create;
using DecisionHelper.Application.Message.Queries.UserNextMessage;
using DecisionHelper.Contracts.Enums;
using DecisionHelper.Contracts.Message;
using DecisionHelper.Domain.Entities;
using FluentAssertions;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DecisionHelper.API.Test
{
    public class AdventureMessagesControllerTests
    {
        public AdventureMessagesController _sut;
        public Mock<ISender> _senderMock;

        [SetUp]
        public void SetUp()
        {
            _senderMock = new Mock<ISender>(MockBehavior.Strict);

            _sut = new AdventureMessagesController(_senderMock.Object, SetupMapper());
        }

        [TearDown]
        public void TearDown()
        {
            _senderMock.VerifyAll();
        }

        [Test]
        public async Task CreateMessage_should_create_adventure_message()
        {
            // arrange
            var adventureId = Guid.NewGuid();
            var message = new CreateMessage
            {
                ParentId = null,
                ByAnswer = Answer.Yes,
                IsQuestion = true,
                Title = "fake-message"
            };

            var command = new CreateMessageCommand("fake-message", true, adventureId, null, Domain.Enums.Answer.Yes);
            var expected = new MessageResult(new Domain.Entities.Message
            {
                Id = Guid.NewGuid(),
                AdventureId = adventureId,
                Title = "fake-message",
                ByAnswer = Domain.Enums.Answer.Yes,
                IsQuestion = true,
                ParentId = null,
            });

            _senderMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // act
            var result = await _sut.CreateMessage(adventureId, message);
            var adventureMessageResult = (OkObjectResult)result;

            // assert
            adventureMessageResult.Value.Should().Be(expected);
        }

        [Test]
        public async Task CreateAdventureJourney_should_create_AdventureJourney()
        {
            // arrange
            var adventureId = Guid.NewGuid();
            var input = new AdventureJourney
            {
                Message = new Contracts.Message.Message
                {
                    Title = "fake-message1",
                    IsQuestion = true,
                    ByAnswer = null
                }
            };
            var expected =
                new AdventureMessageResult(new Adventure { Id = adventureId }, new List<Domain.Entities.Message>());
            _senderMock.Setup(x => x.Send(It.IsAny<CreateAdventureJourneyCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // act
            var result = await _sut.CreateAdventureJourney(adventureId, input);
            var adventureJourneyResult = (OkObjectResult)result;

            // assert
            adventureJourneyResult.Value.Should().Be(expected);
        }

        [Test]
        public async Task GetUserNextMessage_should_return_UsersNextMessage()
        {
            // arrange
            var userId = Guid.NewGuid();
            var adventureId = Guid.NewGuid();

            var expected = new MessageResult(new Domain.Entities.Message());

            _senderMock.Setup(x => x.Send(It.IsAny<GetUserNextMessageQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // act
            var result = await _sut.GetUserNextMessage(adventureId, userId);
            var nextMessageResult = (OkObjectResult)result;
            
            // assert
            nextMessageResult.Value.Should().Be(expected);
        }

        private IMapper SetupMapper()
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.NewConfig<Contracts.Message.Message, AdventureMessage>();

            return new Mapper(config);
        }
    }
}
