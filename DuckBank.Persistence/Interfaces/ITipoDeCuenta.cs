using DuckBank.Persistence.Entities;

namespace DuckBank.Persistence.Interfaces
{
    public interface ITipoDeCuentaRepositorio
    {
        Task<List<TipoDeCuenta>> ObtenerTodosAsync();
    }
}
