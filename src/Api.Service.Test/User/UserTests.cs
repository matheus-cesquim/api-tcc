using System;
using System.Collections.Generic;
using Api.Domain.Dtos.User;
using Bogus;

namespace Api.Service.Test.User
{
    public class UserTests
    {
        public static string UserUsername { get; set; }
        public static string UserEmail { get; set; }
        public static string UserPassword { get; set; } 
        public static string ChangedUserUsername { get; set; }
        public static string ChangedUserEmail { get; set; }
        public static string ChangedUserPassword { get; set; } 
        public static Guid UserId { get; set; }
        public List<UserDto> userDtoList = new List<UserDto>();
        public UserDto userDto;
        public UserDtoCreate userDtoCreate;
        public UserDtoCreateResult userDtoCreateResult;
        public UserDtoUpdate userDtoUpdate;
        public UserDtoUpdateResult userDtoUpdateResult;

        public UserTests()
        {
            var faker = new Faker();
            UserId = Guid.NewGuid();
            UserUsername = faker.Name.FullName();
            UserEmail = faker.Internet.Email();
            UserPassword = faker.Internet.Password();
            ChangedUserUsername = faker.Name.FullName();
            ChangedUserEmail = faker.Internet.Email();
            ChangedUserPassword = faker.Internet.Password();

            for (int i = 0; i < 10; i++)
            {
                var dto = new Faker<UserDto>("pt_BR")
                    .RuleFor(u => u.Id, f => Guid.NewGuid())
                    .RuleFor(u => u.Email, f => f.Internet.Email())
                    .RuleFor(u => u.UserName, f => f.Internet.UserName());
                    
                userDtoList.Add(dto);
            }

            userDto = new UserDto
            {
                Id = UserId,
                UserName = UserUsername,
                Email = UserEmail,
            };

            userDtoCreate = new UserDtoCreate
            {
                Email = UserEmail,
                Password = UserPassword,
                UserName = UserUsername,
            };

            userDtoCreateResult = new UserDtoCreateResult
            {
                Id = UserId,
                Email = UserEmail,
                UserName = UserUsername,
                CreateAt = DateTime.UtcNow
            };

            userDtoUpdate = new UserDtoUpdate 
            {
                Id = UserId,
                Email = ChangedUserEmail,
                Password = ChangedUserPassword,
                UserName = ChangedUserUsername,
            };

            userDtoUpdateResult = new UserDtoUpdateResult
            {
                Id = UserId,
                Email = ChangedUserEmail,
                UserName = ChangedUserUsername,
                UpdateAt = DateTime.UtcNow
            };
        }
    }
}