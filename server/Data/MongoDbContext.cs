using Microsoft.Extensions.Options;
using MongoDB.Driver;
using server.Models;

namespace server.Data
{
  public class MongoDbContext
  {
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
      var client = new MongoClient(settings.Value.ConnectionString);
      _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    public IMongoCollection<Message> Messages => _database.GetCollection<Message>("Messages");
    public IMongoCollection<Conversation> Conversations => _database.GetCollection<Conversation>("Conversations");
    
  }

  public class MongoDbSettings
  {
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
  }
}

