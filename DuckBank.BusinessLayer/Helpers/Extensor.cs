using DuckBank.BusinessLayer.Bl;
using DuckBank.Core.Interfaces.Bl;
using DuckBank.Persistence.Helpers;
using DuckBank.Persistence.Repositorios;
using Microsoft.Extensions.DependencyInjection;

namespace DuckBank.BusinessLayer.Helpers
{
   public static class Extensor
    {
        public static void AgregarDuckBank(this IServiceCollection services)
        {

            services.AgregarRepositorios();

            services.AddScoped<ClienteBl>();
            services.AddScoped<AhorroBl>();


            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
