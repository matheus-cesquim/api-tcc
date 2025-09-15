using System;
using System.Threading.Tasks;
using Api.Data.Context;
using Api.Data.Implementations;
using Api.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Bogus;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Test
{
    public class UserCrudThrowError : BaseTest, IClassFixture<DbTest>
    {
        private readonly DbTest _db;

        public UserCrudThrowError(DbTest db)
        {
            _db = db;
        }

        [Fact(DisplayName = "Delete async should return false")]
        public async Task DeleteAsync_ReturnsFalse_WhenEntityNotFound()
        {
            using var scope = _db.ServiceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
            await context.Database.EnsureCreatedAsync();
            var repo = new UserImplementation(context);

            var idInexistente = Guid.NewGuid();

            var deleted = await repo.DeleteAsync(idInexistente);

            Assert.False(deleted);
        }

        [Fact(DisplayName = "UpdateAsync should return null")]
        public async Task UpdateAsync_ReturnsNull_WhenEntityNotFound()
        {
            using var scope = _db.ServiceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
            await context.Database.EnsureCreatedAsync();
            var repo = new UserImplementation(context);

            var item = new UserEntity
            {
                Id = Guid.NewGuid(),
                Email = "naoexiste@exemplo.com",
                UserName = "ghost"
            };

            var updated = await repo.UpdateAsync(item);

            Assert.Null(updated);
        }

        [Fact(DisplayName = "Repository methods should throw when DbContext is disposed")]
        public async Task All_Methods_Throw_When_Context_Disposed()
        {
            using var scope = _db.ServiceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
            var repo = new UserImplementation(context);

            context.Dispose();

            var dummy = new UserEntity { Id = Guid.NewGuid(), Email = "a@b.com", UserName = "u" };

            await Assert.ThrowsAnyAsync<Exception>(() => repo.InsertAsync(dummy));
            await Assert.ThrowsAnyAsync<Exception>(() => repo.UpdateAsync(dummy));
            await Assert.ThrowsAnyAsync<Exception>(() => repo.DeleteAsync(Guid.NewGuid()));
            await Assert.ThrowsAnyAsync<Exception>(() => repo.SelectAsync(Guid.NewGuid()));
            await Assert.ThrowsAnyAsync<Exception>(() => repo.SelectAsync());
            await Assert.ThrowsAnyAsync<Exception>(() => repo.ExistAsync(Guid.NewGuid()));
        }
    }
}