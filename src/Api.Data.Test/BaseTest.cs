using System;
using Api.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Data.Test
{
    public abstract class BaseTest
    {
        public BaseTest()
        {

        }
    }

    public class DbTest : IDisposable
    {
        private string dataBaseName = $"dbApiTest_{Guid.NewGuid().ToString().Replace("-", string.Empty)}";
        public ServiceProvider ServiceProvider { get; private set; }

        public DbTest()
        {
            var serviceCollection = new ServiceCollection();
            var connectionString =
                $"Server=(localdb)\\MSSQLLocalDB;Initial Catalog={dataBaseName};" +
                "Integrated Security=true;MultipleActiveResultSets=true;";

            var services = new ServiceCollection();
            services.AddDbContext<ApiContext>(o =>
            {
                o.UseSqlServer(connectionString);
                o.EnableSensitiveDataLogging();
                o.EnableDetailedErrors();
            }, ServiceLifetime.Scoped);

            ServiceProvider = services.BuildServiceProvider();

            using var scope = ServiceProvider.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<ApiContext>();
            ctx.Database.EnsureCreated();
        }

        public void Dispose()
        {
            using (var context = ServiceProvider.GetService<ApiContext>())
            {
                context.Database.EnsureDeleted();
            }
        }
    }
}