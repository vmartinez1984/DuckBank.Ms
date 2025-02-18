using DuckBank.BusinessLayer.Helpers;
using DuckBank.Core.Dtos;
using DuckBank.Core.Entities;
using DuckBank.Core.Interfaces.Repositories;

namespace DuckBank.BusinessLayer.Bl
{
    /// <summary>
    /// Reglas de negocio de clientes
    /// </summary>
    public class ClienteBl
    {
        private readonly IClienteRepositorio _repositorio;

        /// <summary>
        /// Inyeccion del maper y el repositorio
        /// </summary>
        /// <param name="repositorio"></param>
        /// <param name="mapper"></param>
        public ClienteBl(IClienteRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        /// <summary>
        /// Agregar cliente
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> AgregarAsync(ClienteDtoIn dto)
        {
            int id;
            Cliente cliente;

            cliente = dto.ToEntity();
            id = await _repositorio.AgregarAsync(cliente);

            return id;
        }

        /// <summary>
        /// Obtener todos los clientes
        /// </summary>
        /// <returns></returns>
        public async Task<List<ClienteDto>> ObtenerTodosAsync()
        {
            List<ClienteDto> dtos;
            List<Cliente> clientes;

            clientes = await _repositorio.ObtenerTodosAsync();
            dtos = clientes.ToDtos();

            return dtos;
        }

        public async Task ActualizarAsync(string id, ClienteDtoIn cliente)
        {
            Cliente clienteAnterior;

            clienteAnterior = await _repositorio.ObtenerPorIdAsync(id);
            clienteAnterior.Nombre = cliente.Nombre;
            clienteAnterior.PrimerApellido = cliente.PrimerApellido;
            clienteAnterior.SegundoApellido = cliente.SegundoApellido;
            clienteAnterior.Telefono = cliente.Telefono;
            clienteAnterior.Correo = cliente.Correo;
            clienteAnterior.Direccion.CalleYNumero = cliente.Direccion.CalleYNumero;
            clienteAnterior.Direccion.Referencia = cliente.Direccion.Referencia;
            clienteAnterior.Direccion.Colonia = cliente.Direccion.Colonia;
            clienteAnterior.Direccion.Alcaldia = cliente.Direccion.Alcaldia;
            clienteAnterior.Direccion.Estado = cliente.Direccion.Estado;
            clienteAnterior.Direccion.CoordenadasGps = cliente.Direccion.CoordenadasGps;

            await _repositorio.ActualizarAsync(clienteAnterior);
        }

        public async Task<ClienteDto> ObtenerPorCorreoAsync(string correo)
        {
            ClienteDto dto;
            Cliente cliente;

            cliente = await _repositorio.ObtenerPorCorreoAsync(correo);
            dto = cliente.ToDto();

            return dto;
        }

        public async Task<ClienteDto> ObtenerPorIdAsync(string id)
        {
            ClienteDto dto;
            Cliente cliente;

            cliente = await _repositorio.ObtenerPorIdAsync(id);
            dto = cliente.ToDto();

            return dto;
        }
    }
}
