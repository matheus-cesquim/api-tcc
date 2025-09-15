using Api.Domain.Interfaces.Services.Security;
using Moq;
using Xunit;

namespace Api.Service.Test.Encrypt
{
    public class WhenExecuteVerifyPassword : EncryptTests
    {
        private IEncryptService _service;
        private Mock<IEncryptService> _mockEncryptService;

        [Fact(DisplayName = "Is possible to execute verify password")]
        public void Is_Possible_Execute_Verify_Password()
        {
            _mockEncryptService = new Mock<IEncryptService>();
            _mockEncryptService.Setup(c => c.VerifyPassword(Password, EncryptedPassword)).Returns(true);
            _service = _mockEncryptService.Object;

            var result = _service.VerifyPassword(Password, EncryptedPassword);
            Assert.True(result);

            _mockEncryptService = new Mock<IEncryptService>();
            _mockEncryptService.Setup(c => c.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            _service = _mockEncryptService.Object;

            var falseResult = _service.VerifyPassword(InvalidPassword, InvalidEncryptedPassword);
            Assert.False(falseResult);
        }
    }
}