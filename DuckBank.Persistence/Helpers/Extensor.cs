using DuckBank.Core.Interfaces;
using DuckBank.Persistence.Interfaces;
using DuckBank.Persistence.Repositorios;
using Microsoft.Extensions.DependencyInjection;

namespace DuckBank.Persistence.Helpers
{
   public static class Extensor
    {
        public static void AgregarRepositorios(this IServiceCollection services)
        {
            services.AddScoped<IAhorroRepositorio, AhorroRepositorio>();
            services.AddScoped<IClienteRepositorio, ClienteRepositorio>();

            services.AddScoped<IRepositorio, Repositorio>();
        }
    }
}
