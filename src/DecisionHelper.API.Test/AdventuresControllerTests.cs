using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DecisionHelper.API.Controllers;
using DecisionHelper.Application.Adventure;
using DecisionHelper.Application.Adventure.Commands.Create;
using DecisionHelper.Application.Adventure.Queries.GeteAll;
using DecisionHelper.Application.User;
using DecisionHelper.Contracts.Adventure;
using DecisionHelper.Domain.Entities;
using DecisionHelper.Domain.Errors;
using FluentAssertions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DecisionHelper.API.Test
{
    public class AdventuresControllerTests
    {
        private AdventuresController _sut;
        private Mock<ISender> _senderMock;

        [SetUp]
        public void SetUp()
        {
            _senderMock = new Mock<ISender>(MockBehavior.Strict);

            _sut = new AdventuresController(_senderMock.Object);
            _sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
        }

        [TearDown]
        public void TearDown()
        {
            _senderMock.VerifyAll();
        }

        [Test]
        public async Task Create_should_create__Adventure()
        {
            // arrange
            var adventureId = Guid.NewGuid();
            var request = new CreateAdventure()
            {
                Name = "fake-adventure"
            };

            var createAdventureCommand = new CreateAdventureCommand("fake-adventure");

            var expected = new AdventureResult(new Adventure { Id = adventureId, Name = "fake-adventure"});
            _senderMock.Setup(x => x.Send(createAdventureCommand, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // act
            var result = await _sut.Create(request);
            var userDetail = (OkObjectResult)result;

            // assert
            userDetail.Value.Should().Be(expected);

        }

        [Test]
        public async Task Create_should_return_Conflict_DuplicateAdventureName()
        {
            // arrange
            var adventureId = Guid.NewGuid();
            var request = new CreateAdventure()
            {
                Name = "fake-adventure"
            };

            var createAdventureCommand = new CreateAdventureCommand("fake-adventure");

            var expected = new AdventureResult(new Adventure { Id = adventureId, Name = "fake-adventure" });
            _senderMock.Setup(x => x.Send(createAdventureCommand, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Errors.Adventure.DuplicateName);

            // act
            var result = await _sut.Create(request);
            var errorDetail = (ObjectResult)result;

            // assert
            errorDetail.StatusCode.Should().Be(409);

        }

        [Test]
        public async Task GetAll_should_return__List_of_Adventures()
        {
            // arrange
            var adventureQuery = new GetAllAdventureQuery();

            var listOfAdventures = new List<AdventureResult>
            {
                new AdventureResult
                (
                    new Adventure
                    {
                        Id = Guid.NewGuid(),
                        Name = "fake-adventure-name"
                    })
            };
            _senderMock.Setup(x => x.Send(adventureQuery, It.IsAny<CancellationToken>()))
                .ReturnsAsync(listOfAdventures);

            // act
            var result = await _sut.GetAll();
            var userDetails = (OkObjectResult)result;

            // assert
            userDetails.Value.Should().Be(listOfAdventures);
        }
    }
}
