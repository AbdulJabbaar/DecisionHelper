using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DecisionHelper.API.Controllers;
using DecisionHelper.Application.User;
using DecisionHelper.Application.User.Commands.Create;
using DecisionHelper.Application.User.Queries.Get;
using DecisionHelper.Application.User.Queries.GetAll;
using DecisionHelper.Contracts.User;
using DecisionHelper.Domain.Entities;
using DecisionHelper.Domain.Errors;
using FluentAssertions;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DecisionHelper.API.Test
{
    public class UsersControllerTests
    {
        private UsersController _sut;
        private Mock<ISender> _senderMock;
        [SetUp]
        public void SetUp()
        {
            _senderMock = new Mock<ISender>(MockBehavior.Strict);
            _sut = new UsersController(_senderMock.Object, SetupMapper());
            _sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

        }

        [TearDown]
        public void TearDown()
        {
            _senderMock.VerifyAll();
        }

        [Test]
        public async Task Create_should_create_and_return__user()
        {
            // arrange
            var userId = Guid.NewGuid();
            var user = new CreateUser
            {
                Email = "fake-email@email.com",
                Name = "fake-user"
            };

            var createUserCommand = new CreateUserCommand("fake-user", "fake-email@email.com");

            var expected = new UserResult(new User { Id = userId });
            _senderMock.Setup(x => x.Send(createUserCommand, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // act
            var result = await _sut.Create(user);
            var userDetail = (OkObjectResult)result;

            // assert
            userDetail.Value.Should().Be(expected);

        }

        [Test]
        public async Task Create_should_create_and_return__conflict_when_user_email_already_exists()
        {
            // arrange
            var userId = Guid.NewGuid();
            var user = new CreateUser
            {
                Email = "fake-email@email.com",
                Name = "fake-user"
            };

            var createUserCommand = new CreateUserCommand("fake-user", "fake-email@email.com");

            var expected = new UserResult(new User { Id = userId });
            _senderMock.Setup(x => x.Send(createUserCommand, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Errors.User.DuplicateEmail);

            // act
            var result = await _sut.Create(user);
            var userDetail = (ObjectResult)result;

            // assert
            userDetail.StatusCode.Should().Be(409);

        }

        [Test]
        public async Task GetAll_should_return__list_of_users()
        {
            // arrange
            var userQuery = new GetAllUserQuery();
            
            var listOfUsers = new List<UserResult>
            {
                new UserResult(
                new User
                    {
                        Id = Guid.NewGuid(),
                        Email = "fakeuser@email.com",
                        Name = "fake-user"
                    }
                )
            };
            _senderMock.Setup(x => x.Send(userQuery, It.IsAny<CancellationToken>()))
                .ReturnsAsync(listOfUsers);

            // act
            var result = await _sut.GetAll();
            var userDetails = (OkObjectResult)result;

            // assert
            userDetails.Value.Should().Be(listOfUsers);
        }

        [Test]
        public async Task GetById_should_return_user()
        {
            // arrange
            var userId = Guid.NewGuid();
            var userQuery = new GetUserQuery(userId);

            var expected = new User
            {
                Id = userId,
                Email = "fakeuser@email.com",
                Name = "fake-user"
            };
            _senderMock.Setup(x => x.Send(userQuery, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UserResult(expected));

            // act
            var result = await _sut.GetById(userId);
            var userDetails = (OkObjectResult)result;

            // assert
            userDetails.Value.Should().Be(new UserResult(expected));
        }

        [Test]
        public async Task GetById_should_not_found_when_user_not_exists()
        {
            // arrange
            var userId = Guid.NewGuid();
            var userQuery = new GetUserQuery(userId);
            
            _senderMock.Setup(x => x.Send(userQuery, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Errors.User.NotFound);

            // act
            var result = await _sut.GetById(userId);
            var userDetails = (ObjectResult)result;

            // assert
            userDetails.StatusCode.Should().Be(404);
        }

        private IMapper SetupMapper()
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.NewConfig<CreateUser, CreateUserCommand>()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Email, src => src.Email);

            return new Mapper(config);
        }
    }
}
