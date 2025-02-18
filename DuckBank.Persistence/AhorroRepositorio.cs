using DuckBank.Core.Entities;
using DuckBank.Core.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DuckBank.Persistence
{
    public class AhorroRepositorio: IAhorroRepositorio
    {
        private readonly IMongoCollection<Ahorro> _collection;

        public AhorroRepositorio(IConfiguration configurations)
        {
            var conectionString = configurations.GetConnectionString("MongoDb");
            var mongoClient = new MongoClient(conectionString);
            var nombreDeLaDb = conectionString.Split("/").Last().Split("?").First();
            var mongoDatabase = mongoClient.GetDatabase(nombreDeLaDb);
            _collection = mongoDatabase.GetCollection<Ahorro>("Ahorros");
        }

        public async Task ActualizarAsync(Ahorro ahorro)
        => await _collection.ReplaceOneAsync(a => a._id == ahorro._id, ahorro);


        public async Task<int> AgregarAsync(Ahorro item)
        {
            if (item.Id == 0)
                item.Id = await ObtenerId();
            await _collection.InsertOneAsync(item);

            return item.Id;
        }

        private async Task<int> ObtenerId()
        {
            var item = await
            _collection
            .Find(new BsonDocument()) // Puedes agregar filtros si es necesario
            .SortByDescending(r => r.Id) // Ordenar por fecha de forma descendente
            .FirstOrDefaultAsync();
            ;
            if (item == null)
                return 1;

            return item.Id + 1;
        }

        public async Task<List<Ahorro>> ObtenerAsync()
        {
            List<Ahorro> ahorros;
            // FilterDefinition<Ahorro> filter;

            // filter = Builders<Ahorro>.Filter.Eq("ClienteId", clienteId);
            ahorros = (await _collection.FindAsync(x => x.Estado == "Activo")).ToList();

            return ahorros;
        }

        public async Task<List<Ahorro>> ObtenerListaDeAhorrosPorClienteIdAsync(string clienteId)
        {
            List<Ahorro> ahorros;
            FilterDefinition<Ahorro> filter;

            filter = Builders<Ahorro>.Filter.Eq("ClienteId", clienteId);
            ahorros = (await _collection.FindAsync(filter)).ToList();

            return ahorros;
        }

        public async Task<Ahorro> ObtenerPorIdAsync(string idGuid)
        {
            Ahorro ahorro;
            int id;

            if (int.TryParse(idGuid, out id))
                ahorro = (await _collection.FindAsync<Ahorro>(x => x.Id == id)).FirstOrDefault();
            else
                ahorro = (await _collection.FindAsync<Ahorro>(x => x.Guid == idGuid)).FirstOrDefault();

            return ahorro;
        }

        public async Task<List<Ahorro>> GetAsync(PagerEntity pager)
        {
            List<Ahorro> entities;
            FilterDefinition<Ahorro> filter;

            if (string.IsNullOrEmpty(pager.Search))
                filter = Builders<Ahorro>.Filter.Where(_ => true);
            else
                filter = Builders<Ahorro>.Filter
                .Where(x => x.Nombre.ToLower().Contains(pager.Search.ToLower()));

            entities = await _collection.Find(filter)
                .Sort("{Id:1}")
                .Skip((pager.PageCurrent - 1) * pager.RecordsPerPage)
                .Limit(pager.RecordsPerPage)
                .ToListAsync();

            pager.TotalRecordsFiltered = entities.Count();
            pager.TotalRecords = (int)await _collection.CountDocumentsAsync(new BsonDocument());

            return entities;
        }

        public async Task<Ahorro> ObtenerPorOtroAsync(string otro, string valor)
        {
            Ahorro ahorro;

            ahorro = (await _collection.FindAsync(new BsonDocument($"Otros.{otro}", valor))).FirstOrDefault();

            return ahorro;
        }
    }

    public class PagerEntity
    {
        public int PageCurrent { get; set; } = 1;

        public int RecordsPerPage { get; set; } = 10;

        public string Search { get; set; }

        public string SortColumn { get; set; }

        public string SortColumnDir { get; set; }

        public int TotalRecords { get; set; }

        public int TotalRecordsFiltered { get; set; }

    }
}
