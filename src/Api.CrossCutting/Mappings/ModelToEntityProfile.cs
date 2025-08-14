using Api.Domain.Entities;
using Api.Domain.Models.User;
using AutoMapper;

namespace Api.CrossCutting.Mappings
{
    public class ModelToEntityProfile : Profile
    {
        public ModelToEntityProfile()
        {
            #region User
            CreateMap<UserEntity, UserModel>()
                .ReverseMap();
            #endregion
        }
    }
}