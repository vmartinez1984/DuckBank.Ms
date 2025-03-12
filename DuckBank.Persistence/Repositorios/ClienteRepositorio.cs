using DuckBank.Persistence.Entities;
using DuckBank.Persistence.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DuckBank.Persistence.Repositorios
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        private readonly IMongoCollection<Cliente> _collection;

        /// <summary>
        /// Configuracion de la mongodb en el constructor
        /// </summary>
        /// <param name="configurations"></param>
        public ClienteRepositorio(IConfiguration configurations)
        {
            var conectionString = configurations.GetConnectionString("MongoDb");
            var mongoClient = new MongoClient(conectionString);
            var nombreDeLaDb = conectionString.Split("/").Last().Split("?").First();
            var mongoDatabase = mongoClient.GetDatabase(nombreDeLaDb);
            _collection = mongoDatabase.GetCollection<Cliente>("Clientes");
        }

        public async Task<int> AgregarAsync(Cliente item)
        {
            item.Id = (int)await _collection.CountDocumentsAsync(_ => true) + 1;
            await _collection.InsertOneAsync(item);

            return item.Id;
        }

        public async Task<Paginado<Cliente>> ObtenerTodosAsync(int numeroDePagina, int numeroDeRegistrosPorPagina, string textoABuscar)
        {
            List<Cliente> clientes;
            FilterDefinition<Cliente> filter;

            if (string.IsNullOrEmpty(textoABuscar))
                filter = Builders<Cliente>.Filter.Where(_ => true);
            else
            {
                textoABuscar = textoABuscar.ToLower();
                filter = Builders<Cliente>.Filter
                    .Where(
                        x => 
                        x.Nombre.ToLower().Contains(textoABuscar) ||
                        x.PrimerApellido.ToLower().Contains(textoABuscar) ||
                        x.EncodedKey.ToLower().Contains(textoABuscar)
                    );
            }
            clientes = await _collection.Find(filter)
            //.Sort("{MuseoId:1}")
            .Skip((numeroDePagina - 1) * numeroDeRegistrosPorPagina)
            .Limit(numeroDeRegistrosPorPagina)
            .ToListAsync();

            var paginado = new Paginado<Cliente>
            {
                TotalDeRegistrosFiltrados = (int)await _collection.Find(filter).CountDocumentsAsync(),
                TotalDeRegistros = (int)await _collection.CountDocumentsAsync(_ => true),
                Lista = clientes
            };

            return paginado;
        }



        public async Task ActualizarAsync(Cliente item) =>
            await _collection.ReplaceOneAsync(x => x._id == item._id, item);

        public async Task<Cliente> ObtenerPorIdAsync(string idEncodedKey)
        {
            if (int.TryParse(idEncodedKey, out int id))
                return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

            return await _collection.Find(x => x.EncodedKey == idEncodedKey).FirstOrDefaultAsync();
        }

        public async Task<Cliente> ObtenerPorCorreoAsync(string correo)
        {
            return (await _collection.FindAsync(x => x.Correo == correo)).FirstOrDefault();
        }

        public async Task<Cliente> ObtenerPorOtrosAsync(string llave, string valor)
            //=> await _collection.Find(new BsonDocument($"Otros.{llave}", valor)).FirstOrDefaultAsync();
            => await _collection.Find(x => x.Otros[llave] == valor).FirstOrDefaultAsync();

    }
}