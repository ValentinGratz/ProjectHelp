using MySql.Data.MySqlClient;

namespace ProjectHelp_Site.Models
{
    public class DbContext
    {
        private static string _ConnectionString { get; set; }
        
        public DbContext(string ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(_ConnectionString);
        }

    }
}
          
          
        