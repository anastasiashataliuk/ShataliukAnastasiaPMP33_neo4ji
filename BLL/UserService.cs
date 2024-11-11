using MongoNeo4jIntegration.DAL.Interfaces;
using System.Threading.Tasks;

namespace MongoNeo4jIntegration.BLL
{
    public class UserService
    {
        private readonly IMongoDBRepository _mongoDbRepository;
        private readonly INeo4jRepository _neo4jRepository;

        public UserService(IMongoDBRepository mongoDbRepository, INeo4jRepository neo4jRepository)
        {
            _mongoDbRepository = mongoDbRepository;
            _neo4jRepository = neo4jRepository;
        }

        public async Task AddUserAsync(string id, string name, string email)
        {
            await _mongoDbRepository.AddUserAsync(id, name, email);
            await _neo4jRepository.AddUserNodeAsync(id, name);
        }

        public async Task AddPostAsync(string postId, string userId, string content)
        {
            await _mongoDbRepository.AddPostAsync(postId, userId, content);
            await _neo4jRepository.AddPostNodeAsync(postId, content);
        }

        public async Task LikePostAsync(string userId, string postId)
        {
            await _mongoDbRepository.LikePostAsync(postId);
            await _neo4jRepository.LikePostAsync(userId, postId);
        }
    }
}
