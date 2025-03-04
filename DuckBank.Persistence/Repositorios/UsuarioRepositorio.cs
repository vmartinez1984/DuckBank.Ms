using DuckBank.Persistence.Entities;
using DuckBank.Persistence.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace DuckBank.Persistence.Repositorios
{
    public class UsuarioRepositorio : BaseRepositorio, IUsuario
    {
        private readonly IMongoCollection<Usuario> _collection;

        public UsuarioRepositorio(IConfiguration configurations) : base(configurations)
        {
            _collection = _mongoDatabase.GetCollection<Usuario>("Usuarios");
            _=Inicializar();
        }

        private async Task Inicializar()
        {
            if (await _collection.CountDocumentsAsync(_ => true) == 0)
            {
                _collection.InsertOne(new Usuario
                {
                    Contraseña = "123456",
                    EncodedKey = "MABV841205",
                    EstaActivo = true,
                    Id = 1,
                    Nombre = "Víctor",
                    PrimerApellido = "Martinez",
                    SegundoApellido = "Bravo",
                    Correo = "ahal_tocob@hotmail.com",
                    Role = new Role
                    {
                        EstaActivo = true,
                        Nombre = "Operador",
                        Descripcion = "Resolver incidentes"
                    }
                });

                _collection.InsertOne(new Usuario
                {
                    Contraseña = "123456",
                    EncodedKey = "MABV8412056TA",
                    EstaActivo = true,
                    Id = 1,
                    Nombre = "Víctor",
                    PrimerApellido = "Martinez",
                    SegundoApellido = "Bravo",
                    Correo = "superior.viktor@gmail.com",
                    Role = new Role
                    {
                        EstaActivo = true,
                        Nombre = "Administrador",
                        Descripcion = "Admin de usuarios"
                    }
                });
            }
        }

        public async Task<Usuario> ObtenerPorIdAsync(string usuarioEncodedKey)
        {
            int id;

            if (int.TryParse(usuarioEncodedKey, out id))
                return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
            else
                return await _collection.Find(x => x.EncodedKey == usuarioEncodedKey).FirstOrDefaultAsync();
        }

        public Task<Usuario> ObtenerPorCorreoAsync(string correo) => _collection.Find(x => x.Correo == correo).FirstOrDefaultAsync();
    }
}
