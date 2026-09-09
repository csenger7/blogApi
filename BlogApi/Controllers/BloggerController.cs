using BlogApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using BlogApi.Models.DTOs;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloggerController : ControllerBase
    {
        private readonly string ConnectionString = "Server=localhost;Database=blog;User Id=root;Password=";
        [HttpGet]
        public List<Blogger> GetAllBloggers()
        {
            List<Blogger> bloggers = new();
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = "SELECT * FROM blogger";
            var cmd = new MySqlCommand(sql, connector);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var blogger = new Blogger
                {
                    Id = dr.GetInt32("Id"),
                    Name = dr.GetString("Name"),
                    Email = dr.GetString("Email"),
                    Age = dr.GetInt32("Age"),
                    Password = dr.GetString("Password"),
                    RegistrationTime = dr.GetDateTime("RegistrationTime")
                };
                bloggers.Add(blogger);
            }
            connector.Close();
            return bloggers;
        }
        [HttpPost]
        public object AddNewBlogger(AddBloggerDTOs blogger)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var blg = new Blogger
            {
                Name = blogger.Name,
                Email = blogger.Email,
                Age = blogger.Age,
                Password = blogger.Password,
                RegistrationTime = DateTime.Now
            };

            var sql = $"INSERT INTO `blogger`(`Name`, `Email`, `Age`, `Password`, `RegistrationTime`) VALUES (@name,@email,@age,@password,@registrationTime)";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@name", blg.Name);
            cmd.Parameters.AddWithValue("@email", blg.Email);
            cmd.Parameters.AddWithValue("@age", blg.Age);
            cmd.Parameters.AddWithValue("@password", blg.Password);
            cmd.Parameters.AddWithValue("@registrationTime", blg.RegistrationTime);
            cmd.ExecuteNonQuery();
            connector.Close();
            return blg;
        }
        [HttpPut]
        public object UpdateBlogger(int id, Blogger blogger)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var sql = $"UPDATE `blogger` SET `Name`=@name,`Email`=@email,`Age`=@age,`Password`=@password WHERE `Id`=@id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@name", blogger.Name);
            cmd.Parameters.AddWithValue("@email", blogger.Email);
            cmd.Parameters.AddWithValue("@age", blogger.Age);
            cmd.Parameters.AddWithValue("@password", blogger.Password);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            connector.Close();
            return blogger;
        }
        [HttpDelete]
        public object DeleteBlogger(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var sql = $"DELETE FROM `blogger` WHERE `Id`=@id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            connector.Close();
            return null;
        }
    }
}