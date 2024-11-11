using MongoDB.Bson;
using System.Threading.Tasks;

namespace MongoNeo4jIntegration.DAL.Interfaces
{
    public interface IMongoDBRepository
    {
        Task AddUserAsync(string id, string name, string email);
        Task<BsonDocument> GetUserAsync(string id);
        Task AddPostAsync(string postId, string userId, string content);
        Task LikePostAsync(string postId);
    }
}
