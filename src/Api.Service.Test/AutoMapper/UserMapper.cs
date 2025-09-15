using System;
using System.Collections.Generic;
using System.Linq;
using Api.Domain.Dtos.User;
using Api.Domain.Entities;
using Api.Domain.Models.User;
using Bogus;
using Xunit;

namespace Api.Service.Test.AutoMapper
{
    public class UserMapper : BaseTestService
    {
        [Fact(DisplayName = "Is possible map the models")]
        public void Is_Possible_Map_The_Models()
        {
            var faker = new Faker();
            UserModel model = new Faker<UserModel>("pt_BR")
                .RuleFor(u => u.Id, f => Guid.NewGuid())
                .RuleFor(u => u.UserName, f => faker.Internet.UserName())
                .RuleFor(u => u.Email, f => faker.Internet.Email())
                .RuleFor(u => u.Password, f => faker.Internet.Password())
                .RuleFor(u => u.CreateAt, f => DateTime.UtcNow)
                .RuleFor(u => u.UpdateAt, f => DateTime.UtcNow);

            var entityList = new List<UserEntity>();
            for(int i = 0; i < 5; i++)
            {
                UserEntity item = new Faker<UserEntity>("pt_BR")
                    .RuleFor(u => u.Id, f => Guid.NewGuid())
                    .RuleFor(u => u.UserName, f => faker.Internet.UserName())
                    .RuleFor(u => u.Email, f => faker.Internet.Email())
                    .RuleFor(u => u.Password, f => faker.Internet.Password())
                    .RuleFor(u => u.CreateAt, f => DateTime.UtcNow)
                    .RuleFor(u => u.UpdateAt, f => DateTime.UtcNow);

                entityList.Add(item);
            }

            //Model to Entity
            var entity = Mapper.Map<UserEntity>(model);
            Assert.Equal(entity.Id, model.Id);
            Assert.Equal(entity.UserName, model.UserName);
            Assert.Equal(entity.Email, model.Email);
            Assert.Equal(entity.Password, model.Password);
            Assert.Equal(entity.CreateAt, model.CreateAt);
            Assert.Equal(entity.UpdateAt, model.UpdateAt);

            //Entity to Dto
            var dto = Mapper.Map<UserDto>(entity);
            Assert.Equal(dto.Id, entity.Id);
            Assert.Equal(dto.UserName, entity.UserName);
            Assert.Equal(dto.Email, entity.Email);
            Assert.Equal(dto.CreateAt, entity.CreateAt);

            var dtoList = Mapper.Map<List<UserDto>>(entityList);
            Assert.True(dtoList.Count() == entityList.Count());

            for(int i = 0; i < dtoList.Count(); i++)
            {
                Assert.Equal(dtoList[i].Id, entityList[i].Id);
                Assert.Equal(dtoList[i].UserName, entityList[i].UserName);
                Assert.Equal(dtoList[i].Email, entityList[i].Email);
                Assert.Equal(dtoList[i].CreateAt, entityList[i].CreateAt);
            }

            var dtoCreateResult = Mapper.Map<UserDtoCreateResult>(entity);
            Assert.Equal(dtoCreateResult.Id, entity.Id);
            Assert.Equal(dtoCreateResult.UserName, entity.UserName);
            Assert.Equal(dtoCreateResult.Email, entity.Email);
            Assert.Equal(dtoCreateResult.CreateAt, entity.CreateAt);

            var dtoUpdateResult = Mapper.Map<UserDtoUpdateResult>(entity);
            Assert.Equal(dtoUpdateResult.Id, entity.Id);
            Assert.Equal(dtoUpdateResult.UserName, entity.UserName);
            Assert.Equal(dtoUpdateResult.Email, entity.Email);
            Assert.Equal(dtoUpdateResult.UpdateAt, entity.UpdateAt);

            //Dto to Model
            var userModel = Mapper.Map<UserModel>(dto);
            Assert.Equal(userModel.Id, dto.Id);
            Assert.Equal(userModel.UserName, dto.UserName);
            Assert.Equal(userModel.Email, dto.Email);
            Assert.Equal(userModel.CreateAt, dto.CreateAt);

            var dtoCreate = Mapper.Map<UserDtoCreate>(userModel);
            Assert.Equal(dtoCreate.UserName, userModel.UserName);
            Assert.Equal(dtoCreate.Email, userModel.Email);
            Assert.Equal(dtoCreate.Password, userModel.Password);

            var dtoUpdate = Mapper.Map<UserDtoUpdate>(userModel);
            Assert.Equal(dtoUpdate.Id, userModel.Id);
            Assert.Equal(dtoUpdate.UserName, userModel.UserName);
            Assert.Equal(dtoUpdate.Email, userModel.Email);
            Assert.Equal(dtoUpdate.Password, userModel.Password);
        }
    }
}