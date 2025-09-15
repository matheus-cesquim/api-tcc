using System;
using System.Threading.Tasks;
using Api.Application.Controllers;
using Api.Domain.Interfaces.Services.User;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.User.WhenRequestDelete
{
    public class ReturnDeleted
    {
        private UsersController _controller;

        [Fact(DisplayName = "Is possible delete a user")]
        public async Task Is_Possible_Call_A_Controller_Delete()
        {
            var serviceMock = new Mock<IUserService>();

            serviceMock.Setup(c => c.Delete(It.IsAny<Guid>())).ReturnsAsync(true);

            _controller = new UsersController(serviceMock.Object);

            var result = await _controller.Delete(Guid.NewGuid());

            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult) result).Value;

             Assert.NotNull(resultValue);
             Assert.True((Boolean)resultValue);
        }
    }
}