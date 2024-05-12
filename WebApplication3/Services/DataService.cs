using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using WebApplication3.Services;
using WebApplication3.Controllers;
using WebApplication3.Interfaces;

namespace WebApplication3.Services
{
    public class DataService : IDataService
    {
        private readonly MySqlConnection _connection;

        public DataService(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
        }

        public async Task<User> GetUser(int id)
        {
            try
            {
                await _connection.OpenAsync();


                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"https://reqres.in/api/users/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var userData = JsonConvert.DeserializeObject<ExternalUserData>(json);

                    // Create a new user object
                    var user = new User
                    {
                        Id = userData.data.id,
                        Email = userData.data.email,
                        FirstName = userData.data.first_name,
                        LastName = userData.data.last_name,
                        Avatar = userData.data.avatar
                    };


                    using var cmd = new MySqlCommand("INSERT INTO users (Id, Email, FirstName, LastName, Avatar) VALUES (@Id, @Email, @FirstName, @LastName, @Avatar)", _connection);
                    cmd.Parameters.AddWithValue("@Id", user.Id);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@Avatar", user.Avatar);

                    await cmd.ExecuteNonQueryAsync();

                    return user;
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                _connection.Close();
            }
        }

    }

}
