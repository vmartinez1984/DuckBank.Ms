using DuckBank.Persistence.Entities;

namespace DuckBank.Persistence.Interfaces
{
    public interface IAhorroRepositorio
    {
        Task ActualizarAsync(Ahorro ahorro);
        Task<int> AgregarAsync(Ahorro item);
        Task<int> DepositarAsync(string idGuid, Movimiento movimiento);
        Task<int> RetirarAsync(string idGuid, Movimiento movimiento);
        Task<List<Ahorro>> ObtenerAsync();        
        Task<List<Ahorro>> ObtenerListaDeAhorrosPorClienteIdAsync(string clienteId);       
        Task<Ahorro> ObtenerPorIdAsync(string idGuid);        
        Task<Ahorro> ObtenerPorOtroAsync(string otro, string valor);
    }
}
