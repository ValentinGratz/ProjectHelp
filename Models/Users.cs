using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ProjectHelp_Site.Models
{
    public class Users
    {
        
        public static string _Username { get; set; }
        public static string _Firstname { get; set; }
        public static string _Name { get; set; }
        public static string _Age { get; set; }
        public static string _Mail { get; set; }
        public static bool _IsConnected { get; set; }
        
        
        /*
         * @brief La méthode LoginAction permet de crée une connextion à partir d'un Username & d'un Password
         * @return Task<bool>
         */
        private static async Task<bool> LoginAction(string Username, string Password)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    using (MySqlConnection Connection = DbContext.GetConnection())
                    {
                        await Connection.OpenAsync();
                        MySqlCommand Cmd = new MySqlCommand("SELECT * FROM users where username = @username", Connection);
                        MySqlParameter UsernameParam = new MySqlParameter("@username", MySqlDbType.String, Username.Length);
                        UsernameParam.Value = Username;
                        Cmd.Parameters.Add(UsernameParam);
                        Cmd.Prepare();
                        using (var Reader = await Cmd.ExecuteReaderAsync())
                        {
                            while (await Reader.ReadAsync())
                            {                                
                                if (Password == Reader["password"].ToString())
                                {
                                    _Username = Username;
                                    _Firstname = Reader["firstname"].ToString();
                                    _Age = Reader["age"].ToString();
                                    _Name = Reader["name"].ToString();
                                    _Mail = Reader["mail"].ToString();
                                    return true;
                                }
                            }
                        }
                    }

                    return false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            });
        }
        
        /*
         * @brief La méthode Login permet de récupérer des informations d'un utilisateur de manière asynchrone
         * @return bool
         */
        public static bool Login(string Username, string Password)
        {
            Task<bool> ResultLogin = LoginAction(Username, Password);
            
            Task.WaitAll(ResultLogin);
            if (ResultLogin.Result)
            {
                _IsConnected = true;
                return true;
            }

            _IsConnected = false;
            return false;
        }
    }
}