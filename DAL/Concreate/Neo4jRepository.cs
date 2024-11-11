using Neo4j.Driver;
using MongoNeo4jIntegration.DAL.Interfaces;
using System.Threading.Tasks;

namespace MongoNeo4jIntegration.DAL.Concrete
{
    public class Neo4jRepository : INeo4jRepository, IDisposable
    {
        private readonly IDriver _driver;

        public Neo4jRepository(string uri, string username, string password)
        {
            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(username, password));
        }

        public async Task AddUserNodeAsync(string id, string name)
        {
            string query = @"
                CREATE (:User {id: $id, name: $name})
            ";

            await using var session = _driver.AsyncSession();
            await session.RunAsync(query, new { id, name });
        }

        public async Task AddPostNodeAsync(string postId, string content)
        {
            string query = @"
                CREATE (:Post {postId: $postId, content: $content})
            ";

            await using var session = _driver.AsyncSession();
            await session.RunAsync(query, new { postId, content });
        }

        public async Task CreateRelationshipAsync(string userId1, string userId2, string relationshipType)
        {
            string query = @"
                MATCH (u1:User {id: $id1}), (u2:User {id: $id2})
                CREATE (u1)-[:" + relationshipType + @"]->(u2)
            ";

            await using var session = _driver.AsyncSession();
            await session.RunAsync(query, new { id1 = userId1, id2 = userId2 });
        }

        public async Task LikePostAsync(string userId, string postId)
        {
            string query = @"
                MATCH (u:User {id: $userId}), (p:Post {postId: $postId})
                CREATE (u)-[:LIKES]->(p)
            ";

            await using var session = _driver.AsyncSession();
            await session.RunAsync(query, new { userId, postId });
        }

        public async Task<int?> GetShortestPathAsync(string userId1, string userId2)
        {
            string query = @"
        MATCH p = shortestPath((u1:User {id: $id1})-[*]-(u2:User {id: $id2}))
        RETURN length(p) AS distance
    ";

            await using var session = _driver.AsyncSession();
            var result = await session.RunAsync(query, new { id1 = userId1, id2 = userId2 });

            // Отримуємо всі результати у список
            var records = await result.ToListAsync();

            // Перевіряємо, чи є записи і отримуємо відстань
            if (records.Any())
            {
                var record = records.First();
                return record["distance"].As<int?>();
            }

            return null;  // Якщо не знайдено шляху
        }





        public void Dispose()
        {
            _driver?.Dispose();
        }
    }
}
