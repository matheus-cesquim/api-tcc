using System;
using System.Threading.Tasks;
using Api.Domain.Dtos.User;
using Api.Domain.Interfaces.Services.User;
using Moq;
using Xunit;

namespace Api.Service.Test.User
{
    public class WhenExecuteGet : UserTests
    {
        private IUserService _service;
        private Mock<IUserService> _mockUserService;

        [Fact(DisplayName = "Is possible to execute a get request")]
        public async Task Is_Possible_Execute_A_Get_Request()
        {
            _mockUserService = new Mock<IUserService>();
            _mockUserService.Setup(c => c.Get(UserId)).ReturnsAsync(userDto);
            _service = _mockUserService.Object;

            var result = await _service.Get(UserId);
            Assert.NotNull(result); 
            Assert.True(UserId == result.Id);
            Assert.Equal(UserUsername, result.UserName);

            _mockUserService = new Mock<IUserService>();
            _mockUserService.Setup(c => c.Get(It.IsAny<Guid>())).Returns(Task.FromResult((UserDto) null));
            _service = _mockUserService.Object;
            
            var record = await _service.Get(UserId);
            Assert.Null(record);
        }
    }
}