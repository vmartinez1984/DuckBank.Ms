using DuckBank.Persistence.Entities;
using DuckBank.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckBank.Persistence.Repositorios
{
    internal class TipoDeCuentaRepositorio : ITipoDeCuentaRepositorio
    {
        public Task<List<TipoDeCuenta>> ObtenerTodosAsync()
        {
            throw new NotImplementedException();
        }
    }
}
