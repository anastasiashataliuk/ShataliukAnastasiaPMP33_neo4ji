using MongoDB.Bson;
using MongoDB.Driver;
using MongoNeo4jIntegration.DAL.Interfaces;
using System.Threading.Tasks;

namespace MongoNeo4jIntegration.DAL.Concrete
{
    public class MongoDBRepository : IMongoDBRepository
    {
        private readonly IMongoDatabase _database;

        public MongoDBRepository(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public async Task AddUserAsync(string id, string name, string email)
        {
            var collection = _database.GetCollection<BsonDocument>("users");
            var document = new BsonDocument
            {
                { "id", id },
                { "name", name },
                { "email", email }
            };
            await collection.InsertOneAsync(document);
        }

        public async Task<BsonDocument> GetUserAsync(string id)
        {
            var collection = _database.GetCollection<BsonDocument>("users");
            return await collection.Find(new BsonDocument("id", id)).FirstOrDefaultAsync();
        }

        public async Task AddPostAsync(string postId, string userId, string content)
        {
            var collection = _database.GetCollection<BsonDocument>("posts");
            var document = new BsonDocument
            {
                { "postId", postId },
                { "userId", userId },
                { "content", content },
                { "likes", 0 }
            };
            await collection.InsertOneAsync(document);
        }

        public async Task LikePostAsync(string postId)
        {
            var collection = _database.GetCollection<BsonDocument>("posts");
            var filter = Builders<BsonDocument>.Filter.Eq("postId", postId);
            var update = Builders<BsonDocument>.Update.Inc("likes", 1);
            await collection.UpdateOneAsync(filter, update);
        }
    }
}
