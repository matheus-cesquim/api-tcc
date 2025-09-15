using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Api.Application.Controllers;
using Api.Domain.Dtos;
using Api.Domain.Dtos.User;
using Api.Domain.Interfaces.Services.User;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.User.ReturnErrors
{
    public class ReturnErrors
    {
        private readonly Mock<IUserService> _serviceMock = new();
        private UsersController BuildSut()
        {
            return new UsersController(_serviceMock.Object);
        }

        [Fact(DisplayName = "Post should return null")]
        public async Task Post_ShouldReturnBadRequest_WhenServiceReturnsNull()
        {
            var controller = BuildSut();

            _serviceMock
                .Setup(s => s.Post(It.IsAny<UserDtoCreate>()))
                .ReturnsAsync((UserDtoCreateResult)null!);

            var result = await controller.Post(new UserDtoCreate());

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact(DisplayName = "Should return 500 on post")]
        public async Task Post_ShouldReturn500_WhenServiceThrows()
        {
            var faker = new Bogus.Faker();
            var controller = BuildSut();

            var dto = new UserDtoCreate
            {
                Email = faker.Internet.Email(),
                UserName = faker.Internet.UserName(),
                Password = faker.Internet.Password()
            };

            _serviceMock
                .Setup(s => s.Post(It.Is<UserDtoCreate>(u => u.Email == dto.Email)))
                .ThrowsAsync(new ArgumentException("failed"));

            var result = await controller.Post(dto);

            var obj = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, obj.StatusCode);
            Assert.Equal("failed", obj.Value);

            _serviceMock.Verify(s => s.Post(It.Is<UserDtoCreate>(u => u.Email == dto.Email)), Times.Once);
        }

        [Fact(DisplayName = "Should return 500 on get all")]
        public async Task GetAll_ShouldReturn500()
        {
            var controller = BuildSut();

            _serviceMock
                .Setup(s => s.GetAll())
                .ThrowsAsync(new ArgumentException("failed"));

            var result = await controller.GetAll();

            var obj = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, obj.StatusCode);
            Assert.Equal("failed", obj.Value);
            _serviceMock.Verify(s => s.GetAll(), Times.Once);
        }

        [Fact(DisplayName = "Should return 500 on get by guid")]
        public async Task GetByGuid_ShouldReturn500()
        {
            var controller = BuildSut();
            var id = Guid.NewGuid();
            _serviceMock
                .Setup(s => s.Get(It.IsAny<Guid>()))
                .ThrowsAsync(new ArgumentException("failed"));

            var result = await controller.Get(id);

            var obj = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, obj.StatusCode);
            Assert.Equal("failed", obj.Value);
            _serviceMock.Verify(s => s.Get(id), Times.Once);
        }

        [Fact(DisplayName = "Update should return null")]
        public async Task Update_ShouldReturnNull()
        {
            var controller = BuildSut();

            _serviceMock
                .Setup(s => s.Put(It.IsAny<UserDtoUpdate>()))
                .ReturnsAsync((UserDtoUpdateResult)null!);

            var result = await controller.Put(new UserDtoUpdate());

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact(DisplayName = "Should return 500 on put")]
        public async Task Put_ShouldReturn500_WhenServiceThrows()
        {
            var faker = new Bogus.Faker();
            var controller = BuildSut();

            var dto = new UserDtoUpdate
            {
                Id = Guid.NewGuid(),
                Email = faker.Internet.Email(),
                UserName = faker.Internet.UserName(),
                Password = faker.Internet.Password()
            };

            _serviceMock
                .Setup(s => s.Put(It.Is<UserDtoUpdate>(u => u.Id == dto.Id)))
                .ThrowsAsync(new ArgumentException("failed"));

            var result = await controller.Put(dto);

            var obj = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, obj.StatusCode);
            Assert.Equal("failed", obj.Value);

            _serviceMock.Verify(s => s.Put(It.Is<UserDtoUpdate>(u => u.Id == dto.Id)), Times.Once);
        }

        [Fact(DisplayName = "Should return 500 on delete")]
        public async Task Delete_ShouldReturn500_WhenServiceThrows()
        {
            var controller = BuildSut();
            var id = Guid.NewGuid();

            _serviceMock
                .Setup(s => s.Delete(id))
                .ThrowsAsync(new ArgumentException("failed"));

            var result = await controller.Delete(id);

            var obj = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, obj.StatusCode);
            Assert.Equal("failed", obj.Value);

            _serviceMock.Verify(s => s.Delete(id), Times.Once);
        }
    }
}