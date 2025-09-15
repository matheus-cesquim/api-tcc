using System;
using System;
using System.Threading.Tasks;
using Api.Data.Context;
using Api.Data.Implementations;
using Api.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Bogus;
using System.Linq;

namespace Api.Data.Test
{
    public class UserCrud : BaseTest, IClassFixture<DbTest>
    {
        private readonly DbTest _db;

        public UserCrud(DbTest db)
        {
            _db = db;
        }

        [Fact(DisplayName = "User CRUD")]
        [Trait("CRUD", "UserEntity")]
        public async Task Is_Possible_Do_User_CRUD()
        {
            using var scope = _db.ServiceProvider.CreateScope();
            using (var context = scope.ServiceProvider.GetRequiredService<ApiContext>())
            {
                await context.Database.EnsureCreatedAsync();
                var faker = new Faker();
                UserImplementation _repository = new UserImplementation(context);
                UserEntity _entity = new Faker<UserEntity>("pt_BR")
                    .RuleFor(u => u.Email, f => f.Internet.Email())
                    .RuleFor(u => u.UserName, f => f.Internet.UserName())
                    .RuleFor(u => u.Password, f => f.Internet.Password());

                var _createdRegister = await _repository.InsertAsync(_entity);
                Assert.NotNull(_createdRegister);
                Assert.Equal(_entity.Email, _createdRegister.Email);
                Assert.Equal(_entity.UserName, _createdRegister.UserName);
                Assert.False(_createdRegister.Id == Guid.Empty);

                _entity.UserName = faker.Name.FirstName();
                var _updatedRegister = await _repository.UpdateAsync(_entity);
                Assert.NotNull(_updatedRegister);
                Assert.Equal(_entity.Email, _updatedRegister.Email);
                Assert.Equal(_entity.UserName, _updatedRegister.UserName);

                var _registerExists = await _repository.ExistAsync(_updatedRegister.Id);
                Assert.True(_registerExists);

                var _selectedRegister = await _repository.SelectAsync(_updatedRegister.Id);
                Assert.NotNull(_selectedRegister);
                Assert.Equal(_updatedRegister.Email, _selectedRegister.Email);
                Assert.Equal(_updatedRegister.UserName, _selectedRegister.UserName);

                var _allRegisters = await _repository.SelectAsync();
                Assert.NotNull(_allRegisters);
                Assert.True(_allRegisters.Count() > 0);

                var _findByLogin = await _repository.FindByEmail(_createdRegister.Email);
                Assert.NotNull(_findByLogin);
                Assert.Equal(_createdRegister.Email, _findByLogin.Email);
                Assert.Equal(_createdRegister.UserName, _findByLogin.UserName);

                var _deleted = await _repository.DeleteAsync(_selectedRegister.Id);
                Assert.True(_deleted);
            }
        }
    }
}