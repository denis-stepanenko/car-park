using CarPark.Exceptions;
using CarPark.Models;
using CarPark.Repos.Interfaces;
using CarPark.Services;
using Moq;
using System.Linq.Expressions;

namespace CarPark.UnitTests
{
    public class CarTest
    {
        [Fact]
        public async void Should_ReturnCar_When_IdIs1()
        {
            var repoMock = new Mock<ICarRepo>();
            repoMock.Setup(x => x.GetByIdAsync("1"))
                .Returns(Task.FromResult<Car?>(new Car { Id = "1", Make = "Toyota", Name = "Highlander" }));

            var service = new CarService(repoMock.Object);

            var car = await service.GetByIdAsync("1");

            Assert.Equal("1", car?.Id);

            repoMock.VerifyAll();
        }

        [Fact]
        public async void Should_ReturnPaginatedResultWithItems_When_PageNumberIs1()
        {
            var repoMock = new Mock<ICarRepo>();
            repoMock.Setup(x => x.GetPaginatedAsync(It.IsAny<int>(), It.IsAny<Expression<Func<Car, bool>>?>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new PaginatedResult<Car>(new List<Car>()
                {
                    new Car { Make = "Toyota", Name = "Highlander" }
                }, 1, 10)));

            var service = new CarService(repoMock.Object);

            var cars = await service.GetPaginatedAsync(1);

            Assert.NotEmpty(cars.Items);

            repoMock.VerifyAll();
        }

        [Fact]
        public async void Should_FailToAdd_When_ThereIsAlreadySuchCar()
        {
            Car car = new Car { Make = "Toyota", Name = "Camry" };

            var repoMock = new Mock<ICarRepo>();
            repoMock.Setup(x => x.IsThereSuchCarAsync(car))
                .Returns(Task.FromResult(true));

            var service = new CarService(repoMock.Object);

            await Assert.ThrowsAsync<BusinessLogicException>(() => service.AddAsync(car));

            repoMock.VerifyAll();
        }

        [Fact]
        public async void Should_Add_When_ThereIsNoSuchCar()
        {
            Car car = new Car { Make = "Toyota", Name = "Camry" };

            var repoMock = new Mock<ICarRepo>();
            repoMock.Setup(x => x.IsThereSuchCarAsync(car))
                .Returns(Task.FromResult(false));

            var service = new CarService(repoMock.Object);

            await service.AddAsync(car);

            repoMock.VerifyAll();
        }

        [Fact]
        public async void Should_FailToUpdate_When_ThereAnotherCarWithSuchMakeAndName()
        {
            Car car = new Car { Make = "Toyota", Name = "Camry" };

            var repoMock = new Mock<ICarRepo>();
            repoMock.Setup(x => x.IsThereSuchCarWithDifferentIdAsync(car))
                .Returns(Task.FromResult(true));

            var service = new CarService(repoMock.Object);

            await Assert.ThrowsAsync<BusinessLogicException>(() => service.UpdateAsync(car));

            repoMock.VerifyAll();
        }

        [Fact]
        public async void Should_Update_When_MakeAndNameAreUnique()
        {
            Car car = new Car { Make = "Toyota", Name = "Camry" };

            var repoMock = new Mock<ICarRepo>();
            repoMock.Setup(x => x.IsThereSuchCarWithDifferentIdAsync(car))
                .Returns(Task.FromResult(false));

            var service = new CarService(repoMock.Object);

            await service.UpdateAsync(car);

            repoMock.VerifyAll();
        }

        [Fact]
        public void Should_FailToPassValidation_When_MakeIsEmpty()
        {
            var model = new Car { Make = "", Name = "Camry" };
            var validationResult = ModelValidationHelper.ValidateModel(model);

            Assert.NotEmpty(validationResult);
        }

        [Fact]
        public void Should_FailToPassValidation_When_NameIsEmpty()
        {
            var model = new Car { Make = "Toyota", Name = "" };
            var validationResult = ModelValidationHelper.ValidateModel(model);

            Assert.NotEmpty(validationResult);
        }

        [Fact]
        public void Should_PassValidation_When_MakeAndNameAreValid()
        {
            var model = new Car { Make = "Toyota", Name = "Camry" };
            var validationResult = ModelValidationHelper.ValidateModel(model);

            Assert.Empty(validationResult);
        }
    }
}