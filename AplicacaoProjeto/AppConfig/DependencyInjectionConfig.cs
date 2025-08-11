using ApplicationServices.Services;
using DataAccess.Repositorys;
using Domain.Repositorys;
using Domain.Services;

namespace AplicacaoProjeto.AppConfig
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            //services
            services.AddSingleton<ICategoriaService, CategoriaService>();
            

            //repositorys
            services.AddSingleton<ICategoriaRepository, CategoriaRepository>( x => new CategoriaRepository(configuration["ConnectionStrings:DefaultConnection"]));
            

            services.AddScoped<HttpClient>();
            return services;
        }
    }
}
