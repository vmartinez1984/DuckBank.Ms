using DuckBank.Core.Dtos;
using DuckBank.Persistence.Entities;

namespace DuckBank.BusinessLayer.Helpers
{
    public static class Mapeador
    {
        public static List<ClienteDto> ToDtos(this List<Cliente> entites) =>
            entites.Select(x => new ClienteDto
            {
                Correo = x.Correo,
                Direccion = x.Direccion.ToDto(),
                EncodedKey = x.EncodedKey,
                Id = x.Id,
                Nombre = x.Nombre,
                Otros = x.Otros,
                PrimerApellido = x.PrimerApellido,
                SegundoApellido = x.SegundoApellido,
                Telefono = x.Telefono

            }).ToList();

        public static DireccionDto ToDto(this Direccion entity) => new DireccionDto
        {
            Alcaldia = entity.Alcaldia,
            CalleYNumero = entity.CalleYNumero,
            CodigoPostal = entity.CodigoPostal,
            Colonia = entity.Colonia,
            CoordenadasGps = entity.CoordenadasGps,
            Estado = entity.Estado,
            Referencia = entity.Referencia
        };

        public static Direccion ToEntity(this DireccionDto dto)
        => new Direccion
        {
            Alcaldia = dto.Alcaldia,
            CalleYNumero = dto.CalleYNumero,
            CodigoPostal = dto.CodigoPostal,
            Colonia = dto.Colonia,
            CoordenadasGps = dto.CoordenadasGps,
            Estado = dto.Estado,
            Referencia = dto.Referencia
        };

        public static Cliente ToEntity(this ClienteDto dto) => new Cliente
        {
            EncodedKey = dto.EncodedKey,
            Nombre = dto.Nombre,
            PrimerApellido = dto.PrimerApellido,
            SegundoApellido = dto.SegundoApellido,
            Telefono = dto.Telefono,
            Correo = dto.Correo,
            EstaActivo = true,
            Otros = dto.Otros,
            Direccion = dto.Direccion.ToEntity()
        };

        public static ClienteDto ToDto(this Cliente entity) => entity is null? null : new ClienteDto
        {
            Correo = entity.Correo,
            Direccion = entity.Direccion.ToDto(),
            EncodedKey = entity.EncodedKey,
            Id = entity.Id,
            Nombre = entity.Nombre,
            Otros = entity.Otros,
            PrimerApellido = entity.PrimerApellido,
            SegundoApellido = entity.SegundoApellido,
            Telefono = entity.Telefono
        };

        public static Cliente ToEntity(this ClienteDtoIn dto) => new Cliente
        {
            EncodedKey = dto.EncodedKey,
            Nombre = dto.Nombre,
            PrimerApellido = dto.PrimerApellido,
            SegundoApellido = dto.SegundoApellido,
            Telefono = dto.Telefono,
            Correo = dto.Correo,
            EstaActivo = true,
            Otros = dto.Otros,
            Direccion = dto.Direccion.ToEntity()
        };

        public static MovimientoDto ToDto(this Movimiento entity) => entity is null ? null : new MovimientoDto
        {
            Cantidad = entity.Cantidad,
            Concepto = entity.Concepto,
            FechaDeRegistro = entity.FechaDeRegistro,
            Id = entity.Id,
            EncodedKey = entity.EncodedKey,
            SaldoFinal = entity.SaldoFinal,
            SaldoInicial = entity.SaldoInicial
        };

        public static List<MovimientoDto> ToDtos(this List<Movimiento> entities)
        {
            var list = entities.Select(x => x.ToDto()).ToList();

            return list.OrderBy(x => x.Id).ToList();
        }

        public static List<AhorroDto> ToDtos(this List<Ahorro> entities) => entities.Select(x => new AhorroDto
        {
             ClienteEncodedKey = x.ClienteEncodedKey,
             Estado = x.Estado,
             FechaDeRegistro = x.FechaDeRegistro,
             Guid = x.Guid,
             Id = x.Id,
             Interes = x.Interes,
             Nombre = x.Nombre,
             Otros = x.Otros,
             Total = x.Total
        }).ToList();

    }
}
