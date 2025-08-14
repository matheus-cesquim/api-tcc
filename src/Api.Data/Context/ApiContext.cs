using System;
using Api.Data.Mapping;
using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Context
{
    public class ApiContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserEntity>(new UserMap().Configure);
            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity
                {
                    Id = Guid.NewGuid(),
                    Email = "admin@test.com",
                    UserName = "admin",
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now,
                    Password = BCrypt.Net.BCrypt.HashPassword("password")
                }
            );
        }

    }
}
