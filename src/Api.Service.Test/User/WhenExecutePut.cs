using System.Threading.Tasks;
using Api.Domain.Interfaces.Services.User;
using Moq;
using Xunit;

namespace Api.Service.Test.User
{
    public class WhenExecutePut : UserTests
    {
        private IUserService _service;
        private Mock<IUserService> _mockUserService;

        [Fact(DisplayName = "Is possible to execute a put request")]
        public async Task Is_Possible_Execute_A_Put_Request()
        {
            _mockUserService = new Mock<IUserService>();
            _mockUserService.Setup(c => c.Post(userDtoCreate)).ReturnsAsync(userDtoCreateResult);
            _service = _mockUserService.Object;

            var result = await _service.Post(userDtoCreate);

            Assert.NotNull(result);
            Assert.Equal(UserUsername, result.UserName);
            Assert.Equal(UserEmail, result.Email);

            _mockUserService = new Mock<IUserService>();
            _mockUserService.Setup(c => c.Put(userDtoUpdate)).ReturnsAsync(userDtoUpdateResult);
            _service = _mockUserService.Object;

            var resultUpdate = await _service.Put(userDtoUpdate);

            Assert.NotNull(resultUpdate);
            Assert.Equal(ChangedUserUsername, resultUpdate.UserName);
            Assert.Equal(ChangedUserEmail, resultUpdate.Email);
        }
    }
}