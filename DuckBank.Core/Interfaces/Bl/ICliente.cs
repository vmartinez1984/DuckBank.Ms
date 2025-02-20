using DuckBank.Core.Dtos;

namespace DuckBank.Core.Interfaces.Bl
{
    public interface ICliente
    {
        Task<int> AgregarAsync(ClienteDtoIn dto);
        Task<ClienteDto> ObtenerPorIdAsync(string id);
    }
}
