using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Application.Controllers;
using Api.Domain.Dtos.User;
using Api.Domain.Interfaces.Services.User;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.User.WhenRequestCreate
{
    public class ReturnBadRequest
    {
        private UsersController _controller;

        [Fact(DisplayName = "Isn't possible create a new user")]
        public async Task Isnt_Possible_Call_A_Controller_Create()
        {
            var faker = new Faker();
            var serviceMock = new Mock<IUserService>();
            var userName = faker.Internet.UserName();
            var email = faker.Internet.Email();
            var encryptedPassword = faker.Hashids.Encode(new List<int>{1,2,3});

            serviceMock.Setup(c => c.Post(It.IsAny<UserDtoCreate>())).ReturnsAsync(
                new UserDtoCreateResult
                {
                    Id = Guid.NewGuid(),
                    UserName = userName,
                    Email = email,
                    CreateAt = DateTime.UtcNow
                }
            );

            _controller = new UsersController(serviceMock.Object);
            _controller.ModelState.AddModelError("UserName", "UserName is required");

            Mock<IUrlHelper> url = new Mock<IUrlHelper>();
            url.Setup(u => u.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5000");
            _controller.Url = url.Object;

            var userDtoCreate = new UserDtoCreate()
            {
                UserName = userName,
                Email = email,
                Password = encryptedPassword
            };

            var result = await _controller.Post(userDtoCreate);
            Assert.True(result is BadRequestObjectResult);
            Assert.False(_controller.ModelState.IsValid);
        }
    }
}