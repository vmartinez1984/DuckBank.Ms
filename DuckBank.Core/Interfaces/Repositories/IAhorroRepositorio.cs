using DuckBank.Core.Entities;

namespace DuckBank.Core.Interfaces.Repositories
{
    public interface IAhorroRepositorio
    {
        Task ActualizarAsync(Ahorro ahorro);
        Task<int> AgregarAsync(Ahorro item);                
        Task<List<Ahorro>> ObtenerAsync();        
        Task<List<Ahorro>> ObtenerListaDeAhorrosPorClienteIdAsync(string clienteId);       
        Task<Ahorro> ObtenerPorIdAsync(string idGuid);        
        Task<Ahorro> ObtenerPorOtroAsync(string otro, string valor);
    }
}
