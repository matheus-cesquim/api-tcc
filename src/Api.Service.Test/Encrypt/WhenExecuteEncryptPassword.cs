using Api.Domain.Interfaces.Services.Security;
using Moq;
using Xunit;

namespace Api.Service.Test.Encrypt
{
    public class WhenExecuteEncryptPassword : EncryptTests
    {
        private IEncryptService _service;
        private Mock<IEncryptService> _mockEncryptService;
        
        [Fact(DisplayName = "Is possible to execute encrypt password")]
        public void Is_Possible_Execute_Encrypt_Password()
        {
            _mockEncryptService = new Mock<IEncryptService>();
            _mockEncryptService.Setup(c => c.EncryptPassword(Password)).Returns(EncryptedPassword);
            _service = _mockEncryptService.Object;

            var result = _service.EncryptPassword(Password);

            Assert.NotNull(result);
            Assert.Equal(EncryptedPassword, result);
        }
    }
}