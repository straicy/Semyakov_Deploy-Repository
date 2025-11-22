using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MyWebApi.Infrastructure
{
    public class MongoDBClient
    {
        private static IMongoDatabase? _database;
        private static MongoDBClient? _instance;
        private readonly IConfiguration _configuration;
        private readonly ILogger<MongoDBClient>? _logger;

        public static MongoDBClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .Build();
                    _instance = new MongoDBClient(configuration, null);
                }
                return _instance;
            }
        }

        public MongoDBClient(IConfiguration configuration, ILogger<MongoDBClient>? logger)
        {
            _configuration = configuration;
            _logger = logger;
            var connectionString = _configuration.GetConnectionString("MongoDB");
            _logger?.LogInformation("Initializing MongoDBClient with connection string: {ConnectionString}", connectionString);
            try
            {
                var client = new MongoClient(connectionString);
                _database = client.GetDatabase(_configuration["DatabaseName"]);
                _logger?.LogInformation("Connected to database: {DatabaseName}", _configuration["DatabaseName"]);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to connect to MongoDB");
                throw;
            }
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            if (_database == null)
            {
                _logger?.LogError("Database not initialized");
                throw new InvalidOperationException("Database not initialized");
            }
            return _database.GetCollection<T>(name);
        }
    }
}