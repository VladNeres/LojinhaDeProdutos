using ApplicationServices.Services;
using DataAccess.Repositorys;
using Domain.Repositorys;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace AplicacaoProjeto.AppConfig
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {

            // Pega a connection string do appsettings.json
            var connectionString = configuration.GetConnectionString("DefaultConnection");

           

            // Configuração do DbContext com MySQL e Lazy Loading
            services.AddDbContext<DatabaseContext>(options =>
             options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Services
            services.AddScoped<ICategoriaService, CategoriaService>();

            // Repositories (não precisa instanciar manualmente)
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();

            // HttpClient
            services.AddScoped<HttpClient>();

            return services;
        }
    }
}
