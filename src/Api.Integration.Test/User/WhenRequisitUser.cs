using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Api.Domain.Dtos.User;
using Bogus;
using Newtonsoft.Json;
using Xunit;

namespace Api.Integration.Test.User
{
    public class WhenRequisitUser : BaseIntegration
    {
        public string _name { get; set; }

        public string _email { get; set; }
        public string _password { get; set; }

        [Fact]
        public async Task Is_Possible_Realize_User_Crud()
        {
            await AddToken();
            var faker = new Faker();
            _name = faker.Name.FirstName();
            _email = faker.Internet.Email();
            _password = faker.Internet.Password();

            var userDto = new UserDtoCreate()
            {
                UserName = _name,
                Email = _email,
                Password = _password
            };

            //Post Section
            var response = await PostJsonAsync(userDto, $"{hostApi}v1/users", client);
            var postResult = await response.Content.ReadAsStringAsync();
            var postRegister = JsonConvert.DeserializeObject<UserDtoCreateResult>(postResult);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(_name, postRegister.UserName);
            Assert.Equal(_email, postRegister.Email);
            Assert.False(postRegister.Id == default(Guid));

            //Get All Section
            response = await client.GetAsync($"{hostApi}v1/users");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var listFromJson = JsonConvert.DeserializeObject<IEnumerable<UserDto>>(jsonResult);
            Assert.NotNull(listFromJson);
            Assert.True(listFromJson.Count() > 0);
            Assert.True(listFromJson.Where(user => user.Id == postRegister.Id).Count() == 1);

            //Get By Id Section
            response = await client.GetAsync($"{hostApi}v1/users/{postRegister.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            jsonResult = await response.Content.ReadAsStringAsync();
            var selectedRegister = JsonConvert.DeserializeObject<UserDto>(jsonResult);
            Assert.NotNull(selectedRegister);
            Assert.Equal(selectedRegister.UserName, postRegister.UserName);
            Assert.Equal(selectedRegister.Email, postRegister.Email);

            //Put Section
            var updateUserDto = new UserDtoUpdate()
            {
                Id = postRegister.Id,
                UserName = faker.Name.FirstName(),
                Email = faker.Internet.Email(),
                Password = faker.Internet.Password()
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(updateUserDto),
                                    Encoding.UTF8, "application/json");
            response = await client.PutAsync($"{hostApi}v1/users", stringContent);
            jsonResult = await response.Content.ReadAsStringAsync();
            var updateRegister = JsonConvert.DeserializeObject<UserDtoUpdateResult>(jsonResult);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEqual(postRegister.UserName, updateRegister.UserName);
            Assert.NotEqual(postRegister.Email, updateRegister.Email);

            //Delete Section
            response = await client.DeleteAsync($"{hostApi}v1/users/{selectedRegister.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            //Get Id After Delete
            response = await client.GetAsync($"{hostApi}v1/users/{postRegister.Id}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}