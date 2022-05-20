//using Neo4j.Driver;

//namespace Roll_Call_System_Neo4j.Repository
//{
//    public class UserRepository
//    {
//        private readonly IDriver _driver;
//        public UserRepository(IDriver driver)
//        {
//            _driver = driver;
//        }

//        private static void WithDatabase(SessionConfigBuilder sessionConfigBuilder)
//        {
//            var neo4jVersion = System.Environment.GetEnvironmentVariable("4.4.0");
//            if (!neo4jVersion.StartsWith("4"))
//            {
//                return;
//            }

//            sessionConfigBuilder.WithDatabase(Database());
//        }
//        private static string Database()
//        {
//            return System.Environment.GetEnvironmentVariable("neo4j");
//        }
//    }
//}
