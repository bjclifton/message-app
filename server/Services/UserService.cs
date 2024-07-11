using server.Models;
using server.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Diagnostics.Eventing.Reader;

namespace server.Services
{
  public class UserService : IUserService
  {
    private readonly IMongoCollection<User> _usersCollection;
    public UserService(IOptions<MongoDbSettings> settings)
    {
      var client = new MongoClient(settings.Value.ConnectionString);
      var database = client.GetDatabase(settings.Value.DatabaseName);
      _usersCollection = database.GetCollection<User>("Users");
    }

    public async Task<List<User>> GetAll() =>
      await _usersCollection.Find(user => true).ToListAsync();

    public async Task<User> GetByID(string id) =>
      await _usersCollection.Find<User>(user => user.Id == id).FirstOrDefaultAsync();

    public async Task<User> GetByUsername(string username) =>
      await _usersCollection.Find<User>(user => user.Username == username).FirstOrDefaultAsync();

    public async Task<User> GetByEmail(string email) =>
      await _usersCollection.Find<User>(user => user.Email == email).FirstOrDefaultAsync();

    public async Task Create(User newUser) =>
      await _usersCollection.InsertOneAsync(newUser);

    public async Task Update(string id, User userIn) =>
      await _usersCollection.ReplaceOneAsync(user => user.Id == id, userIn);

    public async Task Remove(string id) =>
      await _usersCollection.DeleteOneAsync(user => user.Id == id);
  }

}

