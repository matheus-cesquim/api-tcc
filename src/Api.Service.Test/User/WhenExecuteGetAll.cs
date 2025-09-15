using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Domain.Dtos.User;
using Api.Domain.Interfaces.Services.User;
using Moq;
using Xunit;

namespace Api.Service.Test.User
{
    public class WhenExecuteGetAll : UserTests
    {
        private IUserService _service;
        private Mock<IUserService> _mockUserService;

        [Fact(DisplayName = "Is possible to execute a get all request")]
        public async Task Is_Possible_Execute_A_Get_Request()
        {
            _mockUserService = new Mock<IUserService>();
            _mockUserService.Setup(c => c.GetAll()).ReturnsAsync(userDtoList);
            _service = _mockUserService.Object;

            var result = await _service.GetAll();
            Assert.NotNull(result);
            Assert.True(result.Count() == 10);

            var _listResult = new List<UserDto>();
            _mockUserService = new Mock<IUserService>();
            _mockUserService.Setup(c => c.GetAll()).ReturnsAsync(_listResult);
            _service = _mockUserService.Object;

            var emptyResult = await _service.GetAll();
            Assert.Empty(emptyResult);
            Assert.True(emptyResult.Count() == 0);
        }
    }
}