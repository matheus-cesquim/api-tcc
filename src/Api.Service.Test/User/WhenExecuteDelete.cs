using System;
using System.Threading.Tasks;
using Api.Domain.Interfaces.Services.User;
using Moq;
using Xunit;

namespace Api.Service.Test.User
{
    public class WhenExecuteDelete : UserTests
    {
        private IUserService _service;
        private Mock<IUserService> _mockUserService;

        [Fact(DisplayName = "Is possible to execute a delete request")]
        public async Task Is_Possible_Execute_A_Get_Request()
        {
            _mockUserService = new Mock<IUserService>();
            _mockUserService.Setup(c => c.Delete(UserId)).ReturnsAsync(true);
            _service = _mockUserService.Object;

            var result = await _service.Delete(UserId);
            Assert.True(result);

            _mockUserService = new Mock<IUserService>();
            _mockUserService.Setup(c => c.Delete(It.IsAny<Guid>())).ReturnsAsync(false);
            _service = _mockUserService.Object;

            var falseResult = await _service.Delete(Guid.NewGuid());
            Assert.False(falseResult);
        }
    }
}