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

namespace Api.Application.Test.Login
{
    public class ReturnErrors
    {
        private readonly Mock<ILoginService> _serviceMock = new();
        private LoginController BuildSut()
        {
            return new LoginController(_serviceMock.Object);
        }

        [Fact(DisplayName = "Model state shouldn't be valid")]
        public async Task Isnt_Possible_Login_With_Object_Null()
        {
            var controller = BuildSut();
            controller.ModelState.AddModelError("Email", "Campo obrigatório");

            var result = await controller.Login(new LoginDto());

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequest.StatusCode);
        }

        [Fact(DisplayName = "Should return 400 when loginDto is null")]
        public async Task Isnt_Possible_Login_With_Dto_Null()
        {
            var controller = BuildSut();

            var result = await controller.Login(null);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact(DisplayName = "Should return 404 when service returns not found")]
        public async Task Isnt_Possible_Login_When_Not_Found()
        {
            var faker = new Faker();
            var controller = BuildSut();

            _serviceMock
                .Setup(s => s.Login(It.IsAny<LoginDto>()))
                .ReturnsAsync((LoginDtoResult)null!);

            var dto = new LoginDto { Email = faker.Internet.Email(), Password = faker.Internet.Password() };
            var result = await controller.Login(dto);

            Assert.IsType<NotFoundResult>(result);
            _serviceMock.Verify(s => s.Login(It.IsAny<LoginDto>()), Times.Once);
        }

        [Fact(DisplayName = "Should return 500 when service throws ArgumentException")]
        public async Task Isnt_Possible_Login_When_Throw_Error()
        {
            var faker = new Faker();
            var controller = BuildSut();
            var dto = new LoginDto { Email = faker.Internet.Email(), Password = faker.Internet.Password() };

            _serviceMock
                .Setup(s => s.Login(It.IsAny<LoginDto>()))
                .ThrowsAsync(new ArgumentException("failed"));

            var result = await controller.Login(dto);

            var obj = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, obj.StatusCode);
            Assert.Equal("failed", obj.Value);
            _serviceMock.Verify(s => s.Login(It.IsAny<LoginDto>()), Times.Once);
        }
    }
}