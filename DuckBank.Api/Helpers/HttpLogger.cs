﻿using Microsoft.Extensions.Http.Logging;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DuckBank.Api.Helpers
{
    public class HttpLogger : IHttpClientLogger
    {
        private readonly ILogger<HttpLogger> _logger;
        private readonly HttpLoggerRepository _repository;

        public HttpLogger(ILogger<HttpLogger> logger, HttpLoggerRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository;
        }

        public object? LogRequestStart(HttpRequestMessage request)
        {
            //_logger.LogInformation(
            //    "Sending '{Request.Method}' to '{Request.Host}{Request.Path}'",
            //    request.Method,
            //    request.RequestUri?.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped),
            //    request.RequestUri!.PathAndQuery);

            return null;
        }

        public void LogRequestStop(
            object? context, HttpRequestMessage request, HttpResponseMessage response, TimeSpan elapsed)
        {
            //_logger.LogInformation(
            //    "Received '{Response.StatusCodeInt} {Response.StatusCodeString}' after {Response.ElapsedMilliseconds}ms",
            //    (int)response.StatusCode,
            //    response.StatusCode,
            //    elapsed.TotalMilliseconds.ToString("F1"));
            HttpLoggerEntity httpLoggerEntity;

            httpLoggerEntity = new HttpLoggerEntity
            {
                FechaDeRegistro = DateTime.Now,
                RequestBody = request.Content is null ? null : request.Content.ReadAsStringAsync().Result,
                RequestUrl = request.RequestUri.ToString(),
                RequestHeaders = request.Content is null ? null : request.Content?.Headers.ToString().Trim(),
                ResponseBody = response.Content is null ? null : response.Content?.ReadAsStringAsync().Result,
                ResponseHeaders = response.Content is null || response.Content?.Headers is null ? null : response.Content?.Headers?.ToString().Trim(),
                StatusCode = (int)response.StatusCode,
                TiempoDeRespuesta = elapsed.TotalMilliseconds,
            };
            Task.Run(async () =>
            {
                await _repository.AgregarAsync(httpLoggerEntity);
            });
        }

        public void LogRequestFailed(
            object? context,
            HttpRequestMessage request,
            HttpResponseMessage? response,
            Exception exception,
            TimeSpan elapsed)
        {
            _logger.LogError(
                exception,
                "Request towards '{Request.Host}{Request.Path}' failed after {Response.ElapsedMilliseconds}ms",
                request.RequestUri?.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped),
                request.RequestUri!.PathAndQuery,
                elapsed.TotalMilliseconds.ToString("F1"));
        }
    }

    public class HttpLoggerRepository
    {
        private readonly IMongoCollection<HttpLoggerEntity> _collection;
        public HttpLoggerRepository(IConfiguration configurations)
        {
            var mongoClient = new MongoClient(
                configurations.GetConnectionString("mongoDbLogs")
            );
            //var nombreDeLaDb = configurations.GetSection("DuckBankAhorros").Value;
            var mongoDatabase = mongoClient.GetDatabase("DuckBank_Logs");
            _collection = mongoDatabase.GetCollection<HttpLoggerEntity>("Http");
        }

        public async Task<string> AgregarAsync(HttpLoggerEntity entity)
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

    public class HttpLoggerEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string RequestHeaders { get; set; }
        public string RequestUrl { get; set; }
        public string RequestBody { get; set; }

        public string ResponseBody { get; set; }
        public string ResponseHeaders { get; set; }

        public int StatusCode { get; set; }

        public DateTime FechaDeRegistro { get; set; }
        public double TiempoDeRespuesta { get; internal set; }
    }
}
