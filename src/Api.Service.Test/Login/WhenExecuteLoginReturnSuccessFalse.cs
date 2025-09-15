using System;
using System.Threading.Tasks;
using Api.Domain.Dtos;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Services.Security;
using Api.Domain.Interfaces.Services.User;
using Api.Domain.Repository;
using Api.Domain.Security;
using Api.Service.Services.User;
using Bogus;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

namespace Api.Service.Test.Login
{
    public class WhenExecuteLoginReturnSuccessFalse
    {
        private readonly Mock<IUserRepository> _repoMock = new();
        private readonly Mock<IEncryptService> _encMock = new();
        private readonly Mock<IConfiguration> _configMock = new();
        private readonly Api.Domain.Security.SigningConfigurations _signing;

        private class SigningConfigurations
        {
            public SigningCredentials SigningCredentials { get; }
            public SigningConfigurations()
            {
                var key = new SymmetricSecurityKey(Guid.NewGuid().ToByteArray());
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            }
        }

        public WhenExecuteLoginReturnSuccessFalse()
        {
            _signing = new Api.Domain.Security.SigningConfigurations();

            Environment.SetEnvironmentVariable("ISSUER", "test-issuer");
            Environment.SetEnvironmentVariable("AUDIENCE", "test-audience");
            Environment.SetEnvironmentVariable("SECONDS", "3600");
        }

        private LoginService BuildSut()
        {
            return new LoginService(
                _repoMock.Object,
                _signing,
                _configMock.Object,
                _encMock.Object
            );
        }

        [Fact(DisplayName = "Isn't possible login with user equals null")]
        public async Task Login_UserNull_ReturnsFalse()
        {
            var sut = BuildSut();

            var result = await sut.Login(null);

            Assert.NotNull(result);
            Assert.False(result.Authenticated);
            Assert.Equal("Failed to authenticate user", result.Message);
        }

        [Fact(DisplayName = "Isn't possible login with email empty")]
        public async Task Login_EmailEmpty_ReturnsFalse()
        {
            var sut = BuildSut();

            var result = await sut.Login(new LoginDto { Email = "   ", Password = "x" });

            Assert.NotNull(result);
            Assert.False(result.Authenticated);
            Assert.Equal("Failed to authenticate user", result.Message);
        }

        [Fact(DisplayName = "Isn't possible login with user not found")]
        public async Task Login_UserNotFound_ReturnsFalse()
        {
            var sut = BuildSut();
            var email = "naoexiste@exemplo.com";

            _repoMock.Setup(r => r.FindByEmail(email))
                     .ReturnsAsync((UserEntity)null!);

            var result = await sut.Login(new LoginDto { Email = email, Password = "qualquer" });

            Assert.NotNull(result);
            Assert.False(result.Authenticated);
            Assert.Equal("Failed to authenticate user", result.Message);
            _repoMock.Verify(r => r.FindByEmail(email), Times.Once);
            _encMock.Verify(e => e.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact(DisplayName = "Deve retornar Authenticated=false quando a senha for inválida")]
        public async Task Login_PasswordInvalid_ReturnsFalse()
        {
            var sut = BuildSut();
            var email = "user@exemplo.com";

            var userEntity = new UserEntity
            {
                Email = email,
                UserName = "user",
                Password = "hashed_pass"
            };

            _repoMock.Setup(r => r.FindByEmail(email))
                     .ReturnsAsync(userEntity);

            _encMock.Setup(e => e.VerifyPassword("senhaErrada", "hashed_pass"))
                    .Returns(false);

            var result = await sut.Login(new LoginDto { Email = email, Password = "senhaErrada" });

            Assert.NotNull(result);
            Assert.False(result.Authenticated);
            Assert.Equal("Failed to authenticate user", result.Message);
            _encMock.Verify(e => e.VerifyPassword("senhaErrada", "hashed_pass"), Times.Once);
        }

        [Fact(DisplayName = "Deve autenticar e retornar token quando credenciais forem válidas")]
        public async Task Login_Success_ReturnsToken()
        {
            var sut = BuildSut();
            var faker = new Faker();
            var email = faker.Internet.Email();

            var userEntity = new UserEntity
            {
                Email = email,
                UserName = "usuario",
                Password = "hashed_ok"
            };

            _repoMock.Setup(r => r.FindByEmail(email))
                     .ReturnsAsync(userEntity);

            _encMock.Setup(e => e.VerifyPassword("Senha@123", "hashed_ok"))
                    .Returns(true);

            var result = await sut.Login(new LoginDto { Email = email, Password = "Senha@123" });

            Assert.NotNull(result);
            Assert.True(result.Authenticated);
            Assert.False(string.IsNullOrWhiteSpace(result.AccessToken));
            Assert.Equal(email, result.Email);
        }
    }
}