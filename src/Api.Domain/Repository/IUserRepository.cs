using System.Threading.Tasks;
using Api.Domain.Entities;
using Domain.Repository;

namespace Api.Domain.Repository
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        Task<UserEntity> FindByEmail(string email);
    }
}
