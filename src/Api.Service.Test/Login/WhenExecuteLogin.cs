using System;
using System.Threading.Tasks;
using Api.Domain.Dtos;
using Api.Domain.Interfaces.Services.User;
using Bogus;
using Moq;
using Xunit;

namespace Api.Service.Test.Login
{
    public class WhenExecuteLogin
    {
        private ILoginService _service;
        private Mock<ILoginService> _mockUserService;

        [Fact(DisplayName = "Is possible to execute login request")]
        public async Task Is_Possible_Execute_Login_Request()
        {
            var faker = new Faker();
            var email = faker.Internet.Email();
            var returnObject = new LoginDtoResult
            {
                Authenticated = true,
                Created = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                Expiration = DateTime.UtcNow.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss"),
                AccessToken = Guid.NewGuid().ToString(),
                Email = email,
                Message = "Successfully authenticated user"
            };

            var loginDto = new LoginDto {
                Email = email
            };

            _mockUserService = new Mock<ILoginService>();
            _mockUserService.Setup(c => c.Login(loginDto)).ReturnsAsync(returnObject);
            _service = _mockUserService.Object;
            
            var result = await _service.Login(loginDto);
            Assert.NotNull(result);
            Assert.True(result.Authenticated);
        }
    }
}