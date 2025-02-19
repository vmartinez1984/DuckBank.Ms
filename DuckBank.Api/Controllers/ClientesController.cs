using DuckBank.BusinessLayer.Bl;
using DuckBank.Core.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DuckBank.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteBl _cliente;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="logger"></param>
        public ClientesController(ClienteBl cliente, ILogger<ClientesController> logger)
        {
            _cliente = cliente;
            logger.LogInformation("Iniciando");
        }

        /// <summary>
        /// Agregar clientes
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <response code="200">Elemento existente</response>
        /// <response code="201">Elemento creado</response>   
        [ProducesResponseType(typeof(ClienteDto), 200)]
        [ProducesResponseType(typeof(IdDto), 201)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> AgregarAsync(ClienteDtoIn dto)
        {
            int id;
            ClienteDto clienteDto;

            clienteDto = await _cliente.ObtenerPorIdAsync(dto.EncodedKey);
            if (clienteDto is not null)
                return Ok(clienteDto);

            id = await _cliente.AgregarAsync(dto);

            return Created("", new IdDto
            {
                EncodedKey = dto.EncodedKey,
                Id = id
            });
        }

        /// <summary>
        /// Obtener todos
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public async Task<IActionResult> ObtenerTodosAsync()
        //{

        //    var lista = await _cliente.ObtenerTodosAsync();

        //    HttpContext.Response.Headers.Add("TotalDeRegistros", lista.Count().ToString());
        //    return Ok(lista);
        //}

        /// <summary>
        /// Obtener por id o encodedKey
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Elemento existente</response>
        /// <response code="404">Elemento no encontrado</response>  
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClienteDto), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [Produces("application/json")]
        public async Task<IActionResult> ObtenerPorIdEncodedKeyAsync(string id)
        {
            var item = await _cliente.ObtenerPorIdAsync(id);
            if (item == null)
                return NotFound(new ProblemDetails { Status = 404, Detail = $"No se encontro cliente con {id}" });

            return Ok(item);
        }

        /// <summary>
        /// Actualizar
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cliente"></param>
        /// <returns></returns>
        /// <response code="200">Elemento existente</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ClienteDto), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [Produces("application/json")]
        public async Task<IActionResult> ActualizarAsync(string id, ClienteDtoIn cliente)
        {
            var item = await _cliente.ObtenerPorIdAsync(id);
            if (item == null)
                return NotFound(new { Mensaje = "Cliente no encontrado" });
            await _cliente.ActualizarAsync(id, cliente);

            return Accepted(new ProblemDetails { Status = 202, Detail = "Datos actualizados" });
        }

        /// <summary>
        /// Obtener cliente por correo
        /// </summary>
        /// <param name="correo"></param>
        /// <returns></returns>
        /// <response code="200">Elemento existente</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpGet("Correos/{correo}")]
        [ProducesResponseType(typeof(ClienteDto), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [Produces("application/json")]
        public async Task<IActionResult> ObtenerPorCorreo(string correo)
        {
            var item = await _cliente.ObtenerPorCorreoAsync(correo);
            if (item == null)
                return NotFound(new { Mensaje = "Cliente no encontrado" });

            return Ok(item);
        }
    }
}
