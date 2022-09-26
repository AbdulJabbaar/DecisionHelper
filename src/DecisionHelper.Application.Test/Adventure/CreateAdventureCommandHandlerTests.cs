using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DecisionHelper.Application.Adventure.Commands.Create;
using DecisionHelper.Application.Dependencies;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DecisionHelper.Application.Test.Adventure
{
    public class CreateAdventureCommandHandlerTests
    {
        private CreateAdventureCommandHandler _sut;
        private Mock<IAdventureRepository> _mockAdventureRepository;

        [SetUp]
        public void SetUp()
        {
            _mockAdventureRepository = new Mock<IAdventureRepository>(MockBehavior.Strict);
            _sut = new CreateAdventureCommandHandler(_mockAdventureRepository.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _mockAdventureRepository.VerifyAll();
        }

        [Test]
        public async Task Handle_should_create_Adventure()
        {
            // arrange
            var adventureName = "fake-adventure";
            var command = new CreateAdventureCommand(adventureName);
            _mockAdventureRepository.Setup(x => x.GetByName(adventureName, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.Adventure)null);
            _mockAdventureRepository.Setup(x => x.AddAsync(It.IsAny<Domain.Entities.Adventure>()))
                .ReturnsAsync(new Domain.Entities.Adventure());

            // act
            var result = await _sut.Handle(command);

            // assert
            result.Should().NotBeNull();

        }

        [Test]
        public async Task Handle_should_return__DuplicateName_Error()
        {
            // arrange
            var adventureName = "fake-adventure";
            var command = new CreateAdventureCommand(adventureName);
            _mockAdventureRepository.Setup(x => x.GetByName(adventureName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Domain.Entities.Adventure());

            // act
            var result = await _sut.Handle(command);

            // assert
            result.IsError.Should().BeTrue();
            result.FirstError.Description.Should().Be("Adventure name already exists");
        }
    }
}
