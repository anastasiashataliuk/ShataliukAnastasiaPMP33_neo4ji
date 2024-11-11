using System.Threading.Tasks;

namespace MongoNeo4jIntegration.DAL.Interfaces
{
    public interface INeo4jRepository
    {
        Task AddUserNodeAsync(string id, string name);
        Task AddPostNodeAsync(string postId, string content);
        Task CreateRelationshipAsync(string userId1, string userId2, string relationshipType);
        Task LikePostAsync(string userId, string postId);
        Task<int?> GetShortestPathAsync(string userId1, string userId2);
    }
}
