using DuckBank.Core.Interfaces;
using DuckBank.Persistence.Entities;
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

        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            var clientes = await _collection.Find(_ => true).ToListAsync();

            return clientes;
        }

        public async Task ActualizarAsync(Cliente item) =>
            await _collection.ReplaceOneAsync(x => x._id == item._id, item);

        public async Task<Cliente> ObtenerPorIdAsync(string idEncodedKey)
        {
            if (int.TryParse(idEncodedKey, out int id))            
                return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
            
            return await _collection.Find(x=> x.EncodedKey == idEncodedKey).FirstOrDefaultAsync();
        }

        public async Task<Cliente> ObtenerPorCorreoAsync(string correo)
        {
            return (await _collection.FindAsync(x => x.Correo == correo)).FirstOrDefault();
        }

        public async Task<Cliente> ObtenerPorOtrosAsync(string llave, string valor) 
            //=> await _collection.Find(new BsonDocument($"Otros.{llave}", valor)).FirstOrDefaultAsync();
            => await _collection.Find(x=> x.Otros[llave] == valor).FirstOrDefaultAsync();
        
    }
}