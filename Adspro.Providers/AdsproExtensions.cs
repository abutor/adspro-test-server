using Adspro.Contract.Providers;
using Adspro.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Adspro.Providers
{
    public static class AdsproExtensions
    {
        public static IServiceCollection AddProviders(this IServiceCollection services)
        {
            services.AddSingleton<ITokenProvider, TokenProvider>();
            services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
            services.AddScoped<IUserProvider, UserProvider>();
            services.AddScoped<IAuthProvider, AuthProvider>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(c => c.AddProfile<MapperProfile>());
            return services;
        }
    }
}
