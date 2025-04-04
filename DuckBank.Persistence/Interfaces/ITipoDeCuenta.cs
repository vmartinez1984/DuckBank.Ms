using DuckBank.Persistence.Entities;

namespace DuckBank.Persistence.Interfaces
{
    public interface ITipoDeCuenta
    {
        Task<List<TipoDeCuenta>> ObtenerTodosAsync();
    }
}
