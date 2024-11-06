using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text;
using Newtonsoft.Json;

namespace DuckBank.Api.Helpers
{
    /// <summary>
    /// Middleware para registrar las peticiones
    /// </summary>
    public class RequestResponseMiddleware
    {
        private RequestDelegate _next;
        private readonly RequestResponseRepository _requestRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="requestRepository"></param>
        public RequestResponseMiddleware(
            RequestDelegate next,
            RequestResponseRepository requestRepository
        )
        {
            _next = next;
            _requestRepository = requestRepository;
        }

        /// <summary>
        /// Aqui extraemos los datos
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task<RequestResponseEntity> AnalizeRequest(HttpContext context)
        {
            RequestResponseEntity requestDtoIn;

            using (StreamReader stream = new StreamReader(context.Request.Body))
            {
                string path;
                string queryString;
                string header;
                string body;
                string method;


                path = context.Request.Path;
                queryString = context.Request.QueryString.Value;
                header = JsonConvert.SerializeObject(context.Request.Headers).Replace("[", string.Empty).Replace("]", string.Empty);
                method = context.Request.Method;
                body = await stream.ReadToEndAsync();
                requestDtoIn = new RequestResponseEntity
                {
                    RequestBody = body,
                    RequestHeader = header,
                    RequestDateRegistration = DateTime.Now,
                    Path = path + queryString,
                    Method = method
                };

                byte[] bytes = Encoding.UTF8.GetBytes(body);
                context.Request.Body = new MemoryStream(bytes);
            }

            return requestDtoIn;
        }


        /// <summary>
        /// Aqui extraemos los datos y los registramos, response
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                //await AnalizeRequest(context);
                RequestResponseEntity requestDtoIn;

                requestDtoIn = await AnalizeRequest(context);
                // Store the original body stream for restoring the response body back to its original stream
                var originalBodyStream = context.Response.Body;

                // Create new memory stream for reading the response; Response body streams are write-only, therefore memory stream is needed here to read
                await using var memoryStream = new MemoryStream();
                context.Response.Body = memoryStream;

                // Call the next middleware
                await _next(context);

                // Set stream pointer position to 0 before reading
                memoryStream.Seek(0, SeekOrigin.Begin);

                // Read the body from the stream
                var responseBodyText = await new StreamReader(memoryStream).ReadToEndAsync();
                //set data response
                AnalizeResponse(context, requestDtoIn, responseBodyText);
                _ = _requestRepository.AgregarAsync(requestDtoIn);
                // Reset the position to 0 after reading
                memoryStream.Seek(0, SeekOrigin.Begin);

                // Do this last, that way you can ensure that the end results end up in the response.
                // (This resulting response may come either from the redirected route or other special routes if you have any redirection/re-execution involved in the middleware.)
                // This is very necessary. ASP.NET doesn't seem to like presenting the contents from the memory stream.
                // Therefore, the original stream provided by the ASP.NET Core engine needs to be swapped back.
                // Then write back from the previous memory stream to this original stream.
                // (The content is written in the memory stream at this point; it's just that the ASP.NET engine refuses to present the contents from the memory stream.)
                context.Response.Body = originalBodyStream;
                await context.Response.Body.WriteAsync(memoryStream.ToArray());
            }
            catch (Exception)
            {
                await _next(context);
            }
        }

        private static void AnalizeResponse(HttpContext context, RequestResponseEntity requestDtoIn, string responseBodyText)
        {
            requestDtoIn.ResponseBody = responseBodyText;
            requestDtoIn.ResponseHeader = JsonConvert.SerializeObject(context.Response.Headers).Replace("[", string.Empty).Replace("]", string.Empty);
            requestDtoIn.StatusCode = context.Response.StatusCode;
            requestDtoIn.ResponseDateRegistration = DateTime.Now;
            requestDtoIn.RequestId = context.TraceIdentifier;
        }
    }

    /// <summary>
    /// Repositorio para mongoDb Donde se registrara la peticion
    /// </summary>
    public class RequestResponseRepository
    {
        private readonly IMongoCollection<RequestResponseEntity> _collection;

        /// <summary>
        /// Colocamos la cadena de conexxion
        /// </summary>
        /// <param name="configurations"></param>
        public RequestResponseRepository(IConfiguration configurations)
        {
            string connetionString = configurations.GetSection("Serilog:WriteTo")
                .GetChildren()
                .FirstOrDefault(x => x["Name"] == "MongoDB")?["Args:databaseUrl"];
            var mongoClient = new MongoClient(connetionString);
            //Obtenermos el ultimo segmento para tener el nombre de la base de datos
            var mongoDatabase = mongoClient.GetDatabase(connetionString.Split('/').Last());
            _collection = mongoDatabase.GetCollection<RequestResponseEntity>("RequestResponse");
        }

        /// <summary>
        /// Se agregan los datos de la petición
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<string> AgregarAsync(RequestResponseEntity entity)
        {
            try
            {
                await _collection.InsertOneAsync(entity);

                return entity._id;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    /// <summary>
    /// Clase base para la colección
    /// </summary>
    public class RequestResponseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string RequestBody { get; internal set; }
        public string RequestHeader { get; internal set; }
        public DateTime RequestDateRegistration { get; internal set; }
        public string Path { get; internal set; }
        public string Method { get; internal set; }
        public string ResponseBody { get; internal set; }
        public string ResponseHeader { get; internal set; }
        public int StatusCode { get; internal set; }
        public DateTime ResponseDateRegistration { get; set; }
        public string RequestId { get; set; }
    }
}
