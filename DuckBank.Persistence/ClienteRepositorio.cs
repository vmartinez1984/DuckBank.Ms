using DuckBank.Core.Entities;
using DuckBank.Core.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DuckBank.Persistence
{
    public class ClienteRepositorio: IClienteRepositorio
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
            item.Id = (((int)await _collection.CountDocumentsAsync(_ => true)) + 1);
            await _collection.InsertOneAsync(item);

            return item.Id;
        }

        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            var clientes = await _collection.Find(_ => true).ToListAsync();

            return clientes;
        }

        public async Task ActualizarAsync(Cliente item) =>
            await _collection.ReplaceOneAsync(x => x._id == item._id, item);

        public async Task<Cliente> ObtenerPorIdAsync(string id)
        {
            return (await _collection.FindAsync(
            new BsonDocument("$or", new BsonArray
            {
                new BsonDocument("Id", id),
                new BsonDocument("EncodedKey",id)
            }))).FirstOrDefault();
        }

        public async Task<Cliente> ObtenerPorCorreoAsync(string correo)
        {
            return (await _collection.FindAsync(x => x.Correo == correo)).FirstOrDefault();
        }
    }
}