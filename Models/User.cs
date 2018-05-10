using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ProjectHelp_Site.Models
{
    public class User
    {
        public int ID { get; set; }
        public string _Username { get; set; }
        public int _Level { get; set; }
        

        private async Task<bool> LoginAction(string Username, string Password)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    using (MySqlConnection conn = DbContext.GetConnection())
                    {
                        await conn.OpenAsync();
                        MySqlCommand cmd = new MySqlCommand("SELECT * FROM users where username = @username", conn);
                        MySqlParameter usernameParam = new MySqlParameter("@username", MySqlDbType.String, Username.Length);
                        usernameParam.Value = Username;
                        cmd.Parameters.Add(usernameParam);
                        cmd.Prepare();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                if (Password == reader["password"].ToString())
                                {
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
    }
}