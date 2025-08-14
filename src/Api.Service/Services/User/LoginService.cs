using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Api.Domain.Dtos;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Services.Security;
using Api.Domain.Interfaces.Services.User;
using Api.Domain.Repository;
using Api.Domain.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Api.Service.Services.User
{
    public class LoginService : ILoginService
    {
        private readonly SigningConfigurations _signingConfigurations;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _repository;
        private readonly IEncryptService _encryptService;
        public LoginService(IUserRepository repository, 
                            SigningConfigurations signingConfigurations, 
                            IConfiguration configuration,
                            IEncryptService encryptService)
        {
            _repository = repository;
            _signingConfigurations = signingConfigurations;
            _configuration = configuration;
            _encryptService = encryptService;
        }

        public async Task<object> Login(LoginDto user)
        {
            var baseUser = new UserEntity();
            if (user != null && !string.IsNullOrWhiteSpace(user.Email))
            {
                baseUser = await _repository.FindByEmail(user.Email);
                if(baseUser != null)
                {
                    if(_encryptService.VerifyPassword(user.Password, baseUser.Password)) {
                        ClaimsIdentity identity = new ClaimsIdentity(
                            new GenericIdentity(baseUser.Email),
                            new []
                            {
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                new Claim(JwtRegisteredClaimNames.Email, baseUser.Email),
                                new Claim(JwtRegisteredClaimNames.UniqueName, baseUser.UserName)
                            }
                        );

                        DateTime createDate = DateTime.Now;
                        DateTime expirationDate = createDate + TimeSpan.FromSeconds(Convert.ToInt32(Environment.GetEnvironmentVariable("SECONDS")));

                        var handler = new JwtSecurityTokenHandler(); 
                        string token = CreateToken(identity, createDate, expirationDate, handler);
                        return SuccessObject(createDate, expirationDate, token, user);
                    } else {
                        return new {
                            authenticated = false,
                            message = "Failed to authenticate user"
                        };
                    }
                }
                else
                {
                    return new {
                        authenticated = false,
                        message = "Failed to authenticate user"
                    };
                }
            }
            else
            {
                return new {
                    authenticated = false,
                    message = "Failed to authenticate user"
                };
            }
        }

        private string CreateToken(ClaimsIdentity identity, DateTime createDate, DateTime expirationDate, JwtSecurityTokenHandler handler)
        {
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = Environment.GetEnvironmentVariable("ISSUER"),
                Audience = Environment.GetEnvironmentVariable("AUDIENCE"),
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = createDate,
                Expires = expirationDate
            });

            var token = handler.WriteToken(securityToken);
            return token;
        }

        private object SuccessObject(DateTime createDate, DateTime expirationDate, string token, LoginDto user)
        {
            return new {
                authenticated = true,
                created = createDate.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                accessToken = token,
                email = user.Email,
                message = "Successfully authenticated user"
            };
        }
    }
}
