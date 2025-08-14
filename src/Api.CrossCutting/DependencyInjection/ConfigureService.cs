using Api.Domain.Interfaces.Services.Security;
using Api.Domain.Interfaces.Services.User;
using Api.Service.Services.Security;
using Api.Service.Services.User;
using Microsoft.Extensions.DependencyInjection;

namespace Api.CrossCutting.DependencyInjection
{
    public class ConfigureService
    {
        public static void ConfigureDependenciesService(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<ILoginService, LoginService>();
            serviceCollection.AddTransient<IEncryptService, EncryptService>();
        }
    }
}
