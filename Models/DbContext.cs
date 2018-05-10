using MySql.Data.MySqlClient;

namespace ProjectHelp_Site.Models
{
    public class DbContext
    {
        public static string _ConnectionString { get; set; }
        
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(_ConnectionString);
        }

    }
}
          
          
        