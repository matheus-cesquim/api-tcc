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

namespace Api.Application.Test.User.WhenRequestUpdate
{
    public class ReturnBadRequest
    {
        private UsersController _controller;

        [Fact(DisplayName = "Isn't possible update a user")]
        public async Task Isnt_Possible_Call_A_Controller_Update()
        {
            var faker = new Faker();
            var serviceMock = new Mock<IUserService>();
            var userName = faker.Internet.UserName();
            var email = faker.Internet.Email();
            var encryptedPassword = faker.Hashids.Encode(new List<int>{1,2,3});

            serviceMock.Setup(c => c.Put(It.IsAny<UserDtoUpdate>())).ReturnsAsync(
                new UserDtoUpdateResult
                {
                    Id = Guid.NewGuid(),
                    UserName = userName,
                    Email = email,
                    UpdateAt = DateTime.UtcNow
                }
            );

            _controller  = new UsersController(serviceMock.Object);
            _controller.ModelState.AddModelError("Email", "Email is required");

            var userDtoUpdate = new UserDtoUpdate
            {
                Id = Guid.NewGuid(),
                UserName = userName,
                Email = email,
                Password = encryptedPassword
            };

            var result = await _controller.Put(userDtoUpdate);
            Assert.True(result is BadRequestObjectResult);
            Assert.False(_controller.ModelState.IsValid);
        }
    }
}