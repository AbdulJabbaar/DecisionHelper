using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DecisionHelper.API.Controllers;
using DecisionHelper.Application.User.Commands.Create;
using DecisionHelper.Application.UserChoice;
using DecisionHelper.Application.UserChoice.Commands.Create;
using DecisionHelper.Application.UserChoice.Queries.Get;
using DecisionHelper.Contracts.UserChoice;
using DecisionHelper.Domain.Entities;
using DecisionHelper.Domain.Enums;
using DecisionHelper.Domain.Errors;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DecisionHelper.API.Test
{
    public class UserChoicesControllerTests
    {
        private UserChoicesController _sut;
        private Mock<ISender> _sender;

        [SetUp]
        public void SetUp()
        {
            _sender = new Mock<ISender>();
            _sut = new UserChoicesController(_sender.Object);
            _sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
        }

        [TearDown]
        public void TearDown()
        {
            _sender.VerifyAll();
        }

        [Test]
        public async Task GetUserChoices_should_return_UserChoices()
        {
            // arrange
            var userId = Guid.NewGuid();
            var messageId = Guid.NewGuid();
            var userChoicesQuery = new GetUserChoiceQuery(userId);
            var userChoice = new UserChoicesResult(userId, messageId, "fake-message-title", Answer.Yes);

            var senderResponse = new List<UserChoicesResult>() { userChoice };
            var expected = new List<UserChoicesResult>() { userChoice };

            _sender.Setup(x => x.Send(userChoicesQuery, It.IsAny<CancellationToken>()))
                .ReturnsAsync(senderResponse);

            // act
            var result = await _sut.GetUserChoices(userId);
            var okResponse = (OkObjectResult)result;

            // assert
            okResponse.Value.Should().BeEquivalentTo(expected);

        }

        [Test]
        public async Task GetUserChoices_should_return_NotFound_when_userId_not_valid()
        {
            // arrange
            var userId = Guid.NewGuid();
            var userChoicesQuery = new GetUserChoiceQuery(userId);

            _sender.Setup(x => x.Send(userChoicesQuery, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Errors.User.NotFound);

            // act
            var result = await _sut.GetUserChoices(userId);
            var okResponse = (ObjectResult)result;

            // assert
            okResponse.StatusCode.Should().Be(404);

        }

        [Test]
        public async Task CreateUserChoice_should_return_OK()
        {
            // arrange
            var userId = Guid.NewGuid();
            var messageId = Guid.NewGuid();

            var createUserChoiceCommand = new CreateUserChoiceCommand(userId, messageId, Answer.Yes);

            _sender.Setup(x => x.Send(createUserChoiceCommand, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UserChoiceResult(new UserChoice()));

            // act
            var result = await _sut.CreateUserChoice(userId, new CreateUserChoice { MessageId = messageId, Answer = Contracts.Enums.Answer.Yes });
            var okResponse = (OkObjectResult)result;

            // assert
            okResponse.Value.Should().NotBeNull();

        }

        [Test]
        public async Task CreateUserChoice_should_return_Conflict_for_duplicate_user_choice()
        {
            // arrange
            var userId = Guid.NewGuid();
            var messageId = Guid.NewGuid();

            var createUserChoiceCommand = new CreateUserChoiceCommand(userId, messageId, Answer.Yes);

            _sender.Setup(x => x.Send(createUserChoiceCommand, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Errors.UserChoice.DuplicateUserChoice);

            // act
            var result = await _sut.CreateUserChoice(userId, new CreateUserChoice { MessageId = messageId, Answer = Contracts.Enums.Answer.Yes });
            var okResponse = (ObjectResult)result;

            // assert
            okResponse.StatusCode.Should().Be(409);

        }

        [TestCaseSource(nameof(CreateChoiceErrorResponses))]
        public async Task CreateUserChoice_should_return_NotFound_when_userId_or_messageId_is_not_valid(ErrorOr.Error error)
        {
            // arrange
            var userId = Guid.NewGuid();
            var messageId = Guid.NewGuid();

            var createUserChoiceCommand = new CreateUserChoiceCommand(userId, messageId, Answer.Yes);

            _sender.Setup(x => x.Send(createUserChoiceCommand, It.IsAny<CancellationToken>()))
                .ReturnsAsync(error);

            // act
            var result = await _sut.CreateUserChoice(userId, new CreateUserChoice{MessageId = messageId, Answer = Contracts.Enums.Answer.Yes});
            var okResponse = (ObjectResult)result;

            // assert
            okResponse.StatusCode.Should().Be(404);

        }

        private static readonly ErrorOr.Error[] CreateChoiceErrorResponses =
        {
            Errors.User.NotFound,
            Errors.Message.NotFound
        };
    }
}
