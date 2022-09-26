using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DecisionHelper.Application.Dependencies;
using DecisionHelper.Application.User;
using DecisionHelper.Application.User.Queries.GetAll;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DecisionHelper.Application.Test.User
{
    public class GetAllUserQueryHandlerTests
    {
        private GetAllUserQueryHandler _sut;
        private Mock<IUserRepository> _userRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            _sut = new GetAllUserQueryHandler(_userRepositoryMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _userRepositoryMock.VerifyAll();
        }

        [Test]
        public async Task Handle_should_return_users_list()
        {
            // arrange
            var userId = Guid.NewGuid();
            var users = new List<Domain.Entities.User>{ new Domain.Entities.User { Id = userId }};

            var expected = new List<UserResult>();
            expected.Add(new UserResult(new Domain.Entities.User { Id = userId }));

            _userRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);

            // act
            var result = await _sut.Handle(new GetAllUserQuery());

            // assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
