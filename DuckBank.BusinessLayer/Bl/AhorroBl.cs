using DuckBank.Core.Dtos;
using DuckBank.Core.Entities;
using DuckBank.Core.Interfaces.Repositories;

namespace DuckBank.BusinessLayer.Bl
{
    public class AhorroBl
    {
        private readonly IAhorroRepositorio _repositorio;
        public AhorroBl(IAhorroRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<int> AgregarAsync(AhorroDtoIn ahorroDtoIn)
        {
            int id;

            id = await _repositorio.AgregarAsync(new Ahorro
            {
                Guid = string.IsNullOrEmpty(ahorroDtoIn.Guid) ? Guid.NewGuid().ToString() : ahorroDtoIn.Guid.ToString(),
                Nombre = ahorroDtoIn.Nombre,
                ClienteEncodedKey = ahorroDtoIn.ClienteEncodedKey,
                Otros = ahorroDtoIn.Otros,
                Interes = ahorroDtoIn.Interes,
                Estado = "Activo"
            });

            return id;
        }

        public async Task<object> DepositarAsync(string idGuid, MovimientoDtoIn movimiento)
        {
            Ahorro ahorro;
            Movimiento movimientoEntity;
            Movimiento movimientoExistente;

            ahorro = await _repositorio.ObtenerPorIdAsync(idGuid);
            if (string.IsNullOrEmpty(movimiento.EncodedKey))
                movimiento.EncodedKey = Guid.NewGuid().ToString();
            movimientoExistente = ahorro.Depositos.Where(x => x.EncodedKey == movimiento.EncodedKey).FirstOrDefault();
            if (movimientoExistente is null)
            {
                movimientoEntity = new Movimiento
                {
                    Cantidad = movimiento.Cantidad,
                    Concepto = movimiento.Concepto,
                    FechaDeRegistro = DateTime.Now,
                    EncodedKey = movimiento.EncodedKey,
                    SaldoInicial = ahorro.Total,
                    SaldoFinal = ahorro.Total + movimiento.Cantidad,
                    Id = ahorro.Depositos.Count()
                };
                ahorro.Depositos.Add(movimientoEntity);
                ahorro.Total = ahorro.Depositos.Sum(x => x.Cantidad) - ahorro.Retiros.Sum(x => x.Cantidad);
                await _repositorio.ActualizarAsync(ahorro);

                return new IdDto
                {
                    Id = movimientoEntity.Id,
                    EncodedKey = movimientoEntity.EncodedKey
                };
            }
            else
            {
                return new IdDto
                {
                    Id = movimientoExistente.Id,
                    EncodedKey = movimientoExistente.EncodedKey
                };
            }
        }

        public async Task<List<AhorroDto>> ObtenerAhorrosPorClienteIdAsync(string clienteId)
        => (await _repositorio.ObtenerListaDeAhorrosPorClienteIdAsync(clienteId))
                .Select(x => new AhorroDto
                {
                    ClienteEncodedKey = x.ClienteEncodedKey,
                    Guid = x.Guid,
                    Id = x.Id,
                    Interes = x.Interes,
                    Nombre = x.Nombre,
                    Otros = x.Otros,
                    Estado = x.Estado,
                    Total = x.Total,
                    FechaDeRegistro = x.FechaDeRegistro
                })
                .ToList();

        public object ObtenerMovimientoAsync(string encodedKey)
        {
            throw new NotImplementedException();
        }

        public async Task<AhorroDto> ObtenerPorIdAsync(string idGuid)
        {
            AhorroDto ahorroDto;
            Ahorro ahorro;

            ahorro = await _repositorio.ObtenerPorIdAsync(idGuid);

            ahorroDto = ahorro is null ? null : new AhorroDto
            {
                Id = ahorro.Id,
                Nombre = ahorro.Nombre,
                Total = ahorro.Total,
                Guid = ahorro.Guid,
                ClienteEncodedKey = ahorro.ClienteEncodedKey,
                Interes = ahorro.Interes,
                Otros = ahorro.Otros
            };

            return ahorroDto;
        }

        //public async Task<AhorroDto> ObtenerAhorroConDetallePorIdAsync(string guid)
        //{
        //    AhorroConDetalleDto ahorroDto;
        //    Ahorro ahorro;

        //    ahorro = await _repositorio.ObtenerPorIdAsync(idGuid);
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
        //        //FechaDeRegistro = ahorro.FechaDeRegistro,
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

        //    return ahorroDto;
        //}
    }
}
