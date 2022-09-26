using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DecisionHelper.Application.Dependencies;
using DecisionHelper.Application.User.Queries.Get;
using DecisionHelper.Domain.Errors;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DecisionHelper.Application.Test.User
{
    public class GetUserQueryHandlerTests
    {
        private GetUserQueryHandler _sut;
        private Mock<IUserRepository> _userRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);

            _sut = new GetUserQueryHandler(_userRepositoryMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _userRepositoryMock.VerifyAll();
        }

        [Test]
        public async Task Handle_should_return_user()
        {
            // arrange
            var userId = Guid.NewGuid();
            var user = new Domain.Entities.User { Id = userId };
            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // act
            var result = await _sut.Handle(new GetUserQuery(userId));

            // assert
            result.Value.User.Should().Be(user);
        }

        [Test]
        public async Task Handle_should_return_user_not_found()
        {
            // arrange
            var userId = Guid.NewGuid();
            var user = new Domain.Entities.User { Id = userId };
            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.User)null);

            // act
            var result = await _sut.Handle(new GetUserQuery(userId));

            // assert
            result.IsError.Should().BeTrue();
            result.FirstError.Description.Should().Be("User not exists against specified id");
        }
    }
}
