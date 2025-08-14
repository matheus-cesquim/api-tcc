namespace Api.Domain.Interfaces.Services.Security
{
    public interface IEncryptService
    {
        bool VerifyPassword(string password, string encryptedPassword);


        string EncryptPassword(string password);
    }
}