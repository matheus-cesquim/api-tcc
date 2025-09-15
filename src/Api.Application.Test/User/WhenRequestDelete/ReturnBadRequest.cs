using System;
using System.Threading.Tasks;
using Api.Application.Controllers;
using Api.Domain.Interfaces.Services.User;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.User.WhenRequestDelete
{
    public class ReturnBadRequest
    {
        private UsersController _controller;

        [Fact(DisplayName = "Isn't possible delete a user")]
        public async Task Isnt_Possible_Call_A_Controller_Delete()
        {
            var serviceMock = new Mock<IUserService>();

            serviceMock.Setup(c => c.Delete(It.IsAny<Guid>())).ReturnsAsync(true);

            _controller  = new UsersController(serviceMock.Object);
            _controller.ModelState.AddModelError("Id", "Id is invalid");

            var result = await _controller.Delete(default(Guid));
            Assert.True(result is BadRequestObjectResult);
            Assert.False(_controller.ModelState.IsValid);
        }
    }
}