using DuckBank.Core.Interfaces;
using DuckBank.Persistence.Interfaces;

namespace DuckBank.Persistence.Repositorios
{
    public class Repositorio : IRepositorio
    {
        public IAhorroRepositorio Ahorro { get; }

        public IClienteRepositorio Cliente { get; }

        public Repositorio(IClienteRepositorio clienteRepositorio, IAhorroRepositorio ahorroRepositorio)
        {
            Ahorro = ahorroRepositorio;
            Cliente = clienteRepositorio;
        }
    }
}
