using System.Collections.Generic;
using Bogus;

namespace Api.Service.Test.Encrypt
{
    public class EncryptTests
    {
        public static string Password { get; set;}

        public static string EncryptedPassword { get; set; }
        public static string InvalidPassword { get; set; }
        public static string InvalidEncryptedPassword { get; set; }

        public EncryptTests()
        {
            var faker = new Faker();
            Password = faker.Internet.Password();
            EncryptedPassword = faker.Hashids.Encode(new List<int>{1,2,3});
            InvalidPassword = faker.Internet.Password(5);
            InvalidEncryptedPassword = faker.Hashids.Encode(new List<int>{1});
        }
    }
}