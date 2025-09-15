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

namespace Api.Application.Test.User.WhenRequestGet
{
    public class ReturnBadRequest
    {
        private UsersController _controller;

        [Fact(DisplayName = "Isn't possible get a user")]
        public async Task Isnt_Possible_Call_A_Controller_Get_User()
        {
            var faker = new Faker();
            var serviceMock = new Mock<IUserService>();
            var userName = faker.Internet.UserName();
            var email = faker.Internet.Email();
            var encryptedPassword = faker.Hashids.Encode(new List<int>{1,2,3});

            serviceMock.Setup(c => c.Get(It.IsAny<Guid>())).ReturnsAsync(
                new UserDto
                {
                    Id = Guid.NewGuid(),
                    UserName = userName,
                    Email = email,
                    Password = encryptedPassword,
                    CreateAt = DateTime.UtcNow
                }
            );

            _controller = new UsersController(serviceMock.Object);
            _controller.ModelState.AddModelError("Id", "Id is invalid");

            var result = await _controller.Get(Guid.NewGuid());
            Assert.True(result is BadRequestObjectResult);
        }
    }
}