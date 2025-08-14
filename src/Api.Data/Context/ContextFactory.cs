using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Api.Data.Context
{
    public class ContextFactory : IDesignTimeDbContextFactory<ApiContext>
    {
        public ApiContext CreateDbContext(string[] args)
        {
            var connectionString = "Server=localhost\\MedSafe;Database=TCC;Trusted_Connection=True;";
            var optionsBuilder = new DbContextOptionsBuilder<ApiContext>();
            optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.UseSnakeCaseNamingConvention();
            return new ApiContext(optionsBuilder.Options);
        }
    }
}
