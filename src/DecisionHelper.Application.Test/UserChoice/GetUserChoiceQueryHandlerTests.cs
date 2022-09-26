using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DecisionHelper.Application.Dependencies;
using DecisionHelper.Application.UserChoice.Queries.Get;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DecisionHelper.Application.Test.UserChoice
{
    public class GetUserChoiceQueryHandlerTests
    {
        private GetUserChoiceQueryHandler _sut;
        private Mock<IUserChoiceRepository> _userChoiceRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _userChoiceRepositoryMock = new Mock<IUserChoiceRepository>(MockBehavior.Strict);
            _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);

            _sut = new GetUserChoiceQueryHandler(_userChoiceRepositoryMock.Object, _userRepositoryMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _userChoiceRepositoryMock.VerifyAll();
            _userRepositoryMock.VerifyAll();
        }

        [Test]
        public async Task Handle_should_return__UserChoiceResult()
        {
            // arrange
            var userId = Guid.NewGuid();

            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Domain.Entities.User{Id = userId});
            _userChoiceRepositoryMock.Setup(x=>x.GetUserChoices(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Domain.Entities.UserChoice>());

            // act
            var result = await _sut.Handle(new GetUserChoiceQuery(userId));

            // assert
            result.Should().NotBeNull();
        }

        [Test]
        public async Task Handle_should_return__User_Not_Found_when_userid_not_valid()
        {
            // arrange
            var userId = Guid.NewGuid();

            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.User)null);

            // act
            var result = await _sut.Handle(new GetUserChoiceQuery(userId));

            // assert
            result.IsError.Should().BeTrue();
            result.FirstError.Description.Should().Be("User not exists against specified id");
        }
    }
}
