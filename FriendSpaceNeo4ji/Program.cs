using MongoNeo4jIntegration.DAL.Concrete;
using MongoNeo4jIntegration.DAL.Interfaces;

namespace FriendSpaceNeo4ji
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Конфігурація підключень до баз даних
            const string mongoConnectionString = "mongodb://localhost:27017";
            const string mongoDatabaseName = "FriendSpace";
            const string neo4jUri = "bolt://localhost:7687";
            const string neo4jUsername = "neo4j";
            const string neo4jPassword = "password";

            // Ініціалізація реалізацій DAL
            IMongoDBRepository mongoDbRepository = new MongoDBRepository(mongoConnectionString, mongoDatabaseName);
            INeo4jRepository neo4jRepository = new Neo4jRepository(neo4jUri, neo4jUsername, neo4jPassword);

            // Ініціалізація бізнес-логіки (BLL)
            var userService = new UserService(mongoDbRepository, neo4jRepository);

            try
            {
                // **1. Додати користувача**
                Console.WriteLine("Додаємо користувача...");
                string userId1 = "user1";
                string userName1 = "johnny";
                string userEmail1 = "user@example.com";
                await userService.AddUserAsync(userId1, userName1, userEmail1);
                Console.WriteLine($"Користувач {userName1} доданий успішно!");

                // **2. Додати іншого користувача**
                Console.WriteLine("Додаємо іншого користувача...");
                string userId2 = "user2";
                string userName2 = "janed";
                string userEmail2 = "jane.do@example.com";
                await userService.AddUserAsync(userId2, userName2, userEmail2);
                Console.WriteLine($"Користувач {userName2} доданий успішно!");

                // **3. Створити пост для користувача**
                Console.WriteLine("Додаємо пост...");
                string postId = "post1";
                string postContent = "This is John's first post!";
                await userService.AddPostAsync(postId, userId1, postContent);
                Console.WriteLine($"Пост для {userName1} доданий успішно!");

                // **4. Лайкнути пост**
                Console.WriteLine("Ставимо лайк посту...");
                await userService.LikePostAsync(userId2, postId);
                Console.WriteLine($"{userName2} лайкнув пост {postId}!");

                // **5. Додати зв'язок між користувачами**
                Console.WriteLine("Додаємо зв'язок між користувачами...");
                await neo4jRepository.CreateRelationshipAsync(userId1, userId2, "FRIEND");
                Console.WriteLine($"{userName1} тепер друг {userName2}!");

                // **6. Знайти найкоротший шлях між користувачами**
                Console.WriteLine("Шукаємо найкоротший шлях між користувачами...");
                int? shortestPath = await neo4jRepository.GetShortestPathAsync(userId1, userId2);
                if (shortestPath.HasValue)
                {
                    Console.WriteLine($"Найкоротший шлях між {userName1} та {userName2}: {shortestPath.Value}");
                }
                else
                {
                    Console.WriteLine($"Шлях між {userName1} та {userName2} не знайдено.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
            finally
            {
                // Завершуємо роботу з Neo4j
                if (neo4jRepository is IDisposable disposableNeo4j)
                {
                    disposableNeo4j.Dispose();
                }

                Console.WriteLine("Програма завершила виконання.");
            }
        }

        private class UserService
        {
            private IMongoDBRepository mongoDbRepository;
            private INeo4jRepository neo4jRepository;

            public UserService(IMongoDBRepository mongoDbRepository, INeo4jRepository neo4jRepository)
            {
                this.mongoDbRepository = mongoDbRepository;
                this.neo4jRepository = neo4jRepository;
            }

            internal async Task AddPostAsync(string postId, string userId1, string postContent)
            {
                throw new NotImplementedException();
            }

            internal async Task AddUserAsync(string userId1, string userName1, string userEmail1)
            {
                throw new NotImplementedException();
            }

            internal async Task LikePostAsync(string userId2, string postId)
            {
                throw new NotImplementedException();
            }
        }
    }
}
