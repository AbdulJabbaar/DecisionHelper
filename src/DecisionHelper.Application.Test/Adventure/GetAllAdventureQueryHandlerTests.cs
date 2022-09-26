using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DecisionHelper.Application.Adventure;
using DecisionHelper.Application.Adventure.Queries.GeteAll;
using DecisionHelper.Application.Dependencies;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DecisionHelper.Application.Test.Adventure
{
    public class GetAllAdventureQueryHandlerTests
    {
        private GetAllAdventureQueryHandler _sut;
        private Mock<IAdventureRepository> _adventureRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _adventureRepositoryMock = new Mock<IAdventureRepository>(MockBehavior.Strict);
            _sut = new GetAllAdventureQueryHandler(_adventureRepositoryMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _adventureRepositoryMock.VerifyAll();
        }

        [Test]

        public async Task Handle_should_return_adventures_list()
        {
            // arrange
            var adventureId = Guid.NewGuid();
            var adventures = new List<Domain.Entities.Adventure>
            {
                new Domain.Entities.Adventure()
                {
                    Id = adventureId,
                    Name = "fake-adventure"
                }
            };

            var adventuresResult = new List<AdventureResult>();
            adventuresResult.Add(new AdventureResult(
                new Domain.Entities.Adventure()
                {
                    Id = adventureId,
                    Name = "fake-adventure"
                }));

            _adventureRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(adventures);

            // act
            var result = await _sut.Handle(new GetAllAdventureQuery());

            // assert
            result.Should().BeEquivalentTo(adventuresResult);
        }
    }
}
