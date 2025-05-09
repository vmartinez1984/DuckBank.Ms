﻿using DuckBank.Persistence.Interfaces;
using DuckBank.Persistence.Repositorios;
using Microsoft.Extensions.DependencyInjection;

namespace DuckBank.Persistence.Helpers
{
    public static class Extensor
    {
        public static void AgregarDuckBank(this IServiceCollection services)
        {
            services.AddScoped<IAhorroRepositorio, AhorroRepositorio>();
            services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
            services.AddScoped<IUsuario, UsuarioRepositorio>();
            services.AddScoped<IContactoRepositorio, ContactoRepositorio>();

            services.AddScoped<IRepositorio, Repositorio>();
        }
    }
}
