using DuckBank.Persistence.Interfaces;

namespace DuckBank.Persistence.Repositorios
{
    public class Repositorio : IRepositorio
    {
        public IAhorroRepositorio Ahorro { get; }

        public IClienteRepositorio Cliente { get; }

        public IUsuario Usuario { get; }

        public IContactoRepositorio Contacto { get; }

        public ITipoDeCuentaRepositorio TipoDeCuenta {  get; }

        public Repositorio(
            IClienteRepositorio clienteRepositorio
            , IAhorroRepositorio ahorroRepositorio
            , IUsuario usuario
            , IContactoRepositorio contacto
            , ITipoDeCuentaRepositorio tipoDeCuenta
        )
        {
            Ahorro = ahorroRepositorio;
            Cliente = clienteRepositorio;
            Usuario = usuario;
            Contacto = contacto;
            TipoDeCuenta = tipoDeCuenta;
        }
    }
}
