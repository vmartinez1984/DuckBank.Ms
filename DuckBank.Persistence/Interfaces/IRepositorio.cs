using DuckBank.Core.Interfaces;

namespace DuckBank.Persistence.Interfaces
{
    public interface IRepositorio
    {
        public IAhorroRepositorio Ahorro { get; }

        public IClienteRepositorio Cliente { get; }
    }
}
