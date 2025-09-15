using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Application.Controllers;
using Api.Domain.Dtos.User;
using Api.Domain.Interfaces.Services.User;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.User.WhenRequestGetAll
{
    public class ReturnGetAll
    {
        private UsersController _controller;

        [Fact(DisplayName = "Is possible get all")]
        public async Task Is_Possible_Call_A_Controller_Get_All()
        {
            var faker = new Faker();
            var serviceMock = new Mock<IUserService>();

            serviceMock.Setup(c => c.GetAll()).ReturnsAsync(
                new List<UserDto>
                {
                    new UserDto{
                        Id = Guid.NewGuid(),
                        UserName = faker.Internet.UserName(),
                        Email = faker.Internet.Email(),
                        Password = faker.Hashids.Encode(new List<int>{1,2,3}),
                        CreateAt = DateTime.UtcNow
                    },
                    new UserDto{
                        Id = Guid.NewGuid(),
                        UserName = faker.Internet.UserName(),
                        Email = faker.Internet.Email(),
                        Password = faker.Hashids.Encode(new List<int>{1,2,3}),
                        CreateAt = DateTime.UtcNow
                    },
                }
            );

            _controller = new UsersController(serviceMock.Object);
        
            var result = await _controller.GetAll();
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as IEnumerable<UserDto>;
            Assert.True(resultValue.Count() == 2);
        }
    }
}