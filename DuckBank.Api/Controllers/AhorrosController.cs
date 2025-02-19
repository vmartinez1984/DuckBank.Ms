using DuckBank.BusinessLayer.Bl;
using DuckBank.Core.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DuckBank.Api.Controllers
{
    /// <summary>
    /// Ahorros
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AhorrosController : ControllerBase
    {
        private readonly AhorroBl _repositorio;

        /// <summary>
        /// 
        /// </summary>        
        /// <param name="repositorio"></param>
        public AhorrosController(
            AhorroBl repositorio
        )
        {
            _repositorio = repositorio;
        }

        /// <summary>
        /// Agregar ahorro
        /// </summary>
        /// <param name="ahorroDtoIn"></param>
        /// <returns></returns>
        /// <response code="200">Elemento existente</response>
        /// <response code="201">Elemento creado</response>   
        [HttpPost]
        [ProducesResponseType(typeof(AhorroDto), 200)]
        [ProducesResponseType(typeof(IdDto), 201)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [Produces("application/json")]
        public async Task<IActionResult> Post([FromBody] AhorroDtoIn ahorroDtoIn)
        {
            int id;
            AhorroDto ahorro;

            ahorro = await _repositorio.ObtenerPorIdAsync(ahorroDtoIn.Guid);
            if (ahorro is not null)
                return Ok(ahorro);
            id = await _repositorio.AgregarAsync(ahorroDtoIn);

            return Created($"Ahorros/{id}", new IdDto { Id = id, EncodedKey = ahorroDtoIn.Guid, FechaDeRegistro = ahorro.FechaDeRegistro });
        }

        /// <summary>
        /// Depositar
        /// </summary>
        /// <param name="id"></param>
        /// <param name="movimiento"></param>
        /// <returns></returns>
        /// <response code="200">Elemento existente</response>
        /// <response code="201">Elemento creado</response>   
        [HttpPost("{id}/Depositos")]
        [ProducesResponseType(typeof(MovimientoDto), 200)]
        [ProducesResponseType(typeof(IdDto), 201)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [Produces("application/json")]
        public async Task<IActionResult> Depositar(string id, [FromBody] MovimientoDtoIn movimiento)
        {
            var ahorro = await _repositorio.ObtenerPorIdAsync(id);
            if (ahorro is null)
                return NotFound(new { Mensaje = $"Ahorro: {id} no encontrado", Fecha = DateTime.Now });

            var movimientoExistente = await _repositorio.ObtenerMovimientoAsync(id, movimiento.EncodedKey);
            if (movimientoExistente is not null)
                return Ok(movimientoExistente);

            var idDto = await _repositorio.DepositarAsync(id, movimiento);

            return Created("", idDto);
        }

        /// <summary>
        /// Retirar
        /// </summary>
        /// <param name="id"></param>
        /// <param name="movimiento"></param>
        /// <returns></returns>
        /// <response code="200">Elemento existente</response>
        /// <response code="201">Elemento creado</response> 
        [HttpPost("{id}/Retiros")]
        [ProducesResponseType(typeof(MovimientoDto), 200)]
        [ProducesResponseType(typeof(IdDto), 201)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [Produces("application/json")]
        public async Task<IActionResult> Retirar(string id, [FromBody] MovimientoDtoIn movimiento)
        {
            var ahorro = await _repositorio.ObtenerPorIdAsync(id);
            if (ahorro is null)
                return NotFound(new { Mensaje = $"Ahorro: {id} no encontrado", Fecha = DateTime.Now });

            if (ahorro.Total < movimiento.Cantidad)
                return BadRequest(new ProblemDetails
                {
                    Status = 400,
                    Title = "No hay suficientes fondos",
                    Detail = $"Total: {ahorro.Total.ToString("c")}"
                });

            var movimientoExistente = await _repositorio.ObtenerMovimientoAsync(id, movimiento.EncodedKey);
            if (movimientoExistente is not null)
                return Ok(movimientoExistente);

            var idDto = await _repositorio.RetirarAsync(id, movimiento);

            return Created("", idDto);
        }

        ///// <summary>
        ///// Obtener ahorro por ahorroId
        ///// </summary>
        ///// <param name="ahorroId"></param>
        ///// <returns></returns>
        //[HttpGet("{ahorroId}")]
        //public async Task<IActionResult> Get(string ahorroId)
        //{
        //    AhorroConDetalleDto ahorroDto;
        //    Ahorro ahorro;

        //    ahorro = await _repositorio.ObtenerPorIdAsync(ahorroId);
        //    if (ahorro == null)
        //        return NotFound(new { Mensaje = "Ahorro no encontrado" });
        //    ahorroDto = new AhorroConDetalleDto
        //    {
        //        Id = ahorro.Id,
        //        Nombre = ahorro.Nombre,
        //        Total = ahorro.Total,
        //        Guid = ahorro.Guid,
        //        ClienteId = ahorro.ClienteId,
        //        Interes = ahorro.Interes,
        //        Estado = ahorro.Estado,
        //        FechaDeRegistro = ahorro.FechaDeRegistro,
        //        Depositos = ahorro.Depositos.Select(x => new MovimientoDto
        //        {
        //            Cantidad = x.Cantidad,
        //            FechaDeRegistro = x.FechaDeRegistro,
        //            Concepto = x.Concepto,
        //            Referencia = x.Referencia,
        //            SaldoFinal = x.SaldoFinal,
        //            SaldoInicial = x.SaldoInicial
        //        }).ToList(),
        //        Retiros = ahorro.Retiros.Select(x => new MovimientoDto
        //        {
        //            Cantidad = x.Cantidad,
        //            FechaDeRegistro = x.FechaDeRegistro,
        //            Concepto = x.Concepto,
        //            Referencia = x.Referencia,
        //            SaldoFinal = x.SaldoFinal,
        //            SaldoInicial = x.SaldoInicial
        //        }).ToList(),
        //        Otros = ahorro.Otros
        //    };

        //    return Ok(ahorroDto);
        //}

        ///// <summary>
        ///// Lista de ahorros 
        ///// </summary>        
        ///// <returns>ahorroDtos</returns>
        //[HttpGet()]
        //public async Task<IActionResult> Get()
        //{
        //    List<AhorroDto> ahorroDto;
        //    List<Ahorro> ahorros;

        //    ahorros = await _repositorio.ObtenerAsync();
        //    ahorroDto = ahorros.Select(ahorro => new AhorroDto
        //    {
        //        Id = ahorro.Id,
        //        Nombre = ahorro.Nombre,
        //        Total = ahorro.Total,
        //        Guid = ahorro.Guid,
        //        ClienteId = ahorro.ClienteId,
        //        Otros = ahorro.Otros,
        //        Estado = ahorro.Estado,
        //        Interes = ahorro.Interes,
        //        FechaDeRegistro = ahorro.FechaDeRegistro
        //    }).ToList();

        //    return Ok(ahorroDto);
        //}

        /// <summary>
        /// Obtener lista de ahorros por cliente Id
        /// </summary>
        /// <param name="clienteId"></param>
        /// <returns></returns>
        //[HttpGet("Clientes/{clienteId}")]
        //public async Task<IActionResult> ObtenerListaDeAhorrosPorClienteIdAsync(string clienteId)
        //{
        //    List<AhorroDto> lista;

        //    lista = await _repositorio.ObtenerAhorrosPorClienteIdAsync(clienteId);

        //    return Ok(lista);
        //}



        ///// <summary>
        ///// Actualizar el valor o lo guarda en caso de no existir
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="otros"></param>
        ///// <returns></returns>
        //[HttpPost("{id}/Otros")]
        //public async Task<IActionResult> AgregarOtros(string id, Dictionary<string, string> otros)
        //{
        //    Ahorro ahorro;

        //    ahorro = await _repositorio.ObtenerPorIdAsync(id);
        //    foreach (var item in otros)
        //    {
        //        var otro = ahorro.Otros.Where(x => x.Key == item.Key).FirstOrDefault();
        //        if (otro.Key != null)
        //        {
        //            ahorro.Otros[otro.Key] = item.Value;
        //        }
        //        else
        //        {
        //            ahorro.Otros.Add(item.Key, item.Value);
        //        }
        //    }
        //    await _repositorio.ActualizarAsync(ahorro);

        //    return Accepted();
        //}

        ///// <summary>
        ///// Obtener por otro
        ///// </summary>
        ///// <param name="otro"></param>
        ///// <param name="valor"></param>
        ///// <returns></returns>
        //[HttpGet("Otros/{otro}/{valor}")]
        //public async Task<IActionResult> ObtenerPorOtros(string otro, string valor)
        //{
        //    Ahorro ahorro;

        //    ahorro = await _repositorio.ObtenerPorOtroAsync(otro, valor);
        //    if (ahorro == null)
        //        return NotFound();

        //    return Ok(ahorro);
        //}

        ///// <summary>
        ///// Actializar datos del ahorro
        ///// </summary>
        ///// <param name="ahorroId"></param>
        ///// <param name="ahorro"></param>
        ///// <returns></returns>
        //[HttpPut("{ahorroId}")]
        //public async Task Actualizar(string ahorroId, AhorroDtoUpd ahorro)
        //{
        //    Ahorro ahorroOriginal;

        //    ahorroOriginal = await _repositorio.ObtenerPorIdAsync(ahorroId);
        //    ahorroOriginal.ClienteId = ahorro.ClienteId;
        //    ahorroOriginal.Nombre = ahorro.Nombre;
        //    ahorroOriginal.Estado = ahorro.Estado;
        //    ahorroOriginal.Interes = ahorro.Interes;

        //    await _repositorio.ActualizarAsync(ahorroOriginal);
        //}
    }
}