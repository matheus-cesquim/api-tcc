using Api.Domain.Interfaces.Services.Security;

namespace Api.Service.Services.Security
{
    public class EncryptService : IEncryptService
    {
        public bool VerifyPassword(string password, string encryptedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, encryptedPassword);
        }

        public string EncryptPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}