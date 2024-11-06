using DuckBank.Api.Dtos;
using DuckBank.Api.Entidades;
using DuckBank.Api.Persistencia;
using Microsoft.AspNetCore.Mvc;

namespace DuckBank.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AhorrosController : ControllerBase
    {
        private readonly ILogger<AhorrosController> _logger;
        private readonly AhorroRepositorio _repositorio;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="repositorio"></param>
        public AhorrosController(
            ILogger<AhorrosController> logger,
            AhorroRepositorio repositorio
        )
        {
            _logger = logger;
            _repositorio = repositorio;

            //_logger.LogInformation(new EventId(), "Hola mundo");
        }

        /// <summary>
        /// Agregar ahorro
        /// </summary>
        /// <param name="ahorroDtoIn"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AhorroDtoIn ahorroDtoIn)
        {
            int id;
            Ahorro ahorro;

            ahorro = await _repositorio.ObtenerPorIdAsync(ahorroDtoIn.Guid);
            if (ahorro is not null)
                return Ok(ahorro);
            id = await _repositorio.AgregarAsync(new Ahorro
            {
                Guid = string.IsNullOrEmpty(ahorroDtoIn.Guid) ? Guid.NewGuid().ToString() : ahorroDtoIn.Guid.ToString(),
                Nombre = ahorroDtoIn.Nombre,
                ClienteId = ahorroDtoIn.ClienteId,
                Otros = ahorroDtoIn.Otros,
                Interes = ahorroDtoIn.Interes
            });

            return Created($"Ahorros/{id}", new { id });
        }

        /// <summary>
        /// Obtener ahorro por ahorroId
        /// </summary>
        /// <param name="ahorroId"></param>
        /// <returns></returns>
        [HttpGet("{ahorroId}")]
        public async Task<IActionResult> Get(string ahorroId)
        {
            AhorroConDetalleDto ahorroDto;
            Ahorro ahorro;

            ahorro = await _repositorio.ObtenerPorIdAsync(ahorroId);
            if (ahorro == null)
                return NotFound(new { Mensaje = "Ahorro no encontrado" });
            ahorroDto = new AhorroConDetalleDto
            {
                Id = ahorro.Id,
                Nombre = ahorro.Nombre,
                Total = ahorro.Total,
                Guid = ahorro.Guid,
                ClienteId = ahorro.ClienteId,
                Interes = ahorro.Interes,
                Estado = ahorro.Estado,
                Depositos = ahorro.Depositos.Select(x => new MovimientoDto
                {
                    Cantidad = x.Cantidad,
                    FechaDeRegistro = x.FechaDeRegistro,
                    Concepto = x.Concepto,
                    Referencia = x.Referencia,
                    SaldoFinal = x.SaldoFinal,
                    SaldoInicial = x.SaldoInicial
                }).ToList(),
                Retiros = ahorro.Retiros.Select(x => new MovimientoDto
                {
                    Cantidad = x.Cantidad,
                    FechaDeRegistro = x.FechaDeRegistro,
                    Concepto = x.Concepto,
                    Referencia = x.Referencia,
                    SaldoFinal = x.SaldoFinal,
                    SaldoInicial = x.SaldoInicial
                }).ToList(),
                Otros = ahorro.Otros
            };

            return Ok(ahorroDto);
        }

        /// <summary>
        /// Lista de ahorros paginados
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        //[HttpGet]
        //public async Task<IActionResult> Get([FromQuery] PagerEntity pager)
        //{
        //    List<AhorroDto> ahorroDto;
        //    List<Ahorro> ahorros;

        //    ahorros = await _repositorio.GetAsync(pager);
        //    ahorroDto = ahorros.Select(ahorro => new AhorroDto
        //    {
        //        Id = ahorro.Id,
        //        Nombre = ahorro.Nombre,
        //        //Total = ahorro.Total,
        //        Guid = ahorro.Guid,
        //        ClienteId = ahorro.ClienteId,
        //        Otros = ahorro.Otros
        //    }).ToList();

        //    return Ok(new
        //    {
        //        PaginaActual = pager.PageCurrent,
        //        RegistrosPorPagina = pager.RecordsPerPage,
        //        TotalDeRegistros = pager.TotalRecords,
        //        TotalDeRegistrosFiltrados = pager.TotalRecordsFiltered,
        //        ahorroDto
        //    });
        //}

        /// <summary>
        /// Lista de ahorros 
        /// </summary>        
        /// <returns>ahorroDtos</returns>
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            List<AhorroDto> ahorroDto;
            List<Ahorro> ahorros;

            ahorros = await _repositorio.ObtenerAsync();
            ahorroDto = ahorros.Select(ahorro => new AhorroDto
            {
                Id = ahorro.Id,
                Nombre = ahorro.Nombre,
                Total = ahorro.Total,
                Guid = ahorro.Guid,
                ClienteId = ahorro.ClienteId,
                Otros = ahorro.Otros
            }).ToList();

            return Ok(ahorroDto);
        }

        /// <summary>
        /// Obtener lista de ahorros por cliente Id
        /// </summary>
        /// <param name="clienteId"></param>
        /// <returns></returns>
        [HttpGet("Clientes/{clienteId}")]
        public async Task<IActionResult> ObtenerListaDeAhorrosPorClienteIdAsync(string clienteId)
        {
            List<AhorroDto> lista;

            lista = (await _repositorio.ObtenerListaDeAhorrosPorClienteIdAsync(clienteId))
                .Select(x => new AhorroDto
                {
                    ClienteId = x.ClienteId,
                    Guid = x.Guid,
                    Id = x.Id,
                    Interes = x.Interes,
                    Nombre = x.Nombre,
                    Otros= x.Otros,
                    Estado = x.Estado,
                    Total = x.Total                   
                })
                .ToList();

            return Ok(lista);
        }

        /// <summary>
        /// Depositar
        /// </summary>
        /// <param name="id"></param>
        /// <param name="movimiento"></param>
        /// <returns></returns>
        [HttpPost("{id}/Depositos")]
        public async Task<IActionResult> Depositar(string id, [FromBody] MovimientoDtoIn movimiento)
        {
            Ahorro ahorro;
            Movimiento movimientoEntity;

            ahorro = await _repositorio.ObtenerPorIdAsync(id.ToString());
            movimientoEntity = new Movimiento
            {
                Cantidad = movimiento.Cantidad,
                Concepto = movimiento.Concepto,
                FechaDeRegistro = DateTime.Now,
                //Id = string.IsNullOrEmpty(movimiento.Id) ? Guid.NewGuid().ToString() : movimiento.Id,
                Referencia = movimiento.Referencia,
                SaldoInicial = ahorro.Total,
                SaldoFinal = ahorro.Total + movimiento.Cantidad
            };
            ahorro.Depositos.Add(movimientoEntity);
            ahorro.Total = ahorro.Depositos.Sum(x => x.Cantidad) - ahorro.Retiros.Sum(x => x.Cantidad);
            await _repositorio.ActualizarAsync(ahorro);

            return Created("", movimiento);
        }

        /// <summary>
        /// Retirar
        /// </summary>
        /// <param name="id"></param>
        /// <param name="movimiento"></param>
        /// <returns></returns>
        [HttpPost("{id}/Retiros")]
        public async Task<IActionResult> Retirar(string id, [FromBody] MovimientoDtoIn movimiento)
        {
            Ahorro ahorro;
            Movimiento movimientoEntity;
            decimal total;

            ahorro = await _repositorio.ObtenerPorIdAsync(id.ToString());
            if (movimiento.Cantidad > ahorro.Total)
            {
                return StatusCode(428, new
                {
                    Mensaje = "No hay chivo"
                });
            }
            movimientoEntity = new Movimiento
            {
                Cantidad = movimiento.Cantidad,
                Concepto = movimiento.Concepto,
                FechaDeRegistro = DateTime.Now,
                //Id = string.IsNullOrEmpty(movimiento.Id) ? Guid.NewGuid().ToString() : movimiento.Id,
                Referencia = movimiento.Referencia,
                SaldoFinal = ahorro.Total - movimiento.Cantidad,
                SaldoInicial = ahorro.Total
            };
            ahorro.Retiros.Add(movimientoEntity);
            ahorro.Total = ahorro.Depositos.Sum(x => x.Cantidad) - ahorro.Retiros.Sum(x => x.Cantidad);
            await _repositorio.ActualizarAsync(ahorro);

            return Created("", movimiento);
        }

        /// <summary>
        /// Actualizar el valor o lo guarda en caso de no existir
        /// </summary>
        /// <param name="id"></param>
        /// <param name="otros"></param>
        /// <returns></returns>
        [HttpPost("{id}/Otros")]
        public async Task<IActionResult> AgregarOtros(string id, Dictionary<string, string> otros)
        {
            Ahorro ahorro;

            ahorro = await _repositorio.ObtenerPorIdAsync(id);
            foreach (var item in otros)
            {
                var otro = ahorro.Otros.Where(x => x.Key == item.Key).FirstOrDefault();
                if (otro.Key != null)
                {
                    ahorro.Otros[otro.Key] = item.Value;
                }
                else
                {
                    ahorro.Otros.Add(item.Key, item.Value);
                }
            }
            await _repositorio.ActualizarAsync(ahorro);

            return Accepted();
        }

        /// <summary>
        /// Obtener por otro
        /// </summary>
        /// <param name="otro"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        [HttpGet("Otros/{otro}/{valor}")]
        public async Task<IActionResult> ObtenerPorOtros(string otro, string valor)
        {
            Ahorro ahorro;

            ahorro = await _repositorio.ObtenerPorOtroAsync(otro, valor);
            if (ahorro == null)
                return NotFound();

            return Ok(ahorro);
        }
    }
}