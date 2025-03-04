using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace DuckBank.Persistence.Repositorios
{
    public class BaseRepositorio
    {
        protected readonly IMongoDatabase _mongoDatabase;
        public BaseRepositorio(IConfiguration configurations)
        {
            var conectionString = configurations.GetConnectionString("MongoDb");
            var mongoClient = new MongoClient(conectionString);
            var nombreDeLaDb = conectionString.Split("/").Last().Split("?").First();
            _mongoDatabase = mongoClient.GetDatabase(nombreDeLaDb);
        }
    }
}
