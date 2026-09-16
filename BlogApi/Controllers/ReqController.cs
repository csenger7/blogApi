using BlogApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReqController : ControllerBase
    {
        private readonly string ConnectionString = "Server=localhost;Database=blog;User Id=root;Password=";
        [HttpGet]
        public object GetABlogger(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            List<Blogger> bloggers = new();
            connector.Open();
            var sql = $"SELECT `Name`, `Email` FROM blogger WHERE `Id`=@id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var blogger = new Blogger
                {
                    Name = dr.GetString("Name"),
                    Email = dr.GetString("Email"),
                };
                bloggers.Add(blogger);
            }
            connector.Close();
            return bloggers;
        }
        [HttpGet("BloggerPost")]
        public object GetABloggerPost(string name)
        {
            var connector = new MySqlConnection(ConnectionString);
            int idf = 0;
            List<post> posts = new();
            connector.Open();
            var sql = $"SELECT `Id` FROM blogger WHERE `Name`=@name";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@name", name);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                idf = dr.GetInt32("Id");
            }
            connector.Close();
            connector.Open();
            var sql2 = $"SELECT `Title`, `Content` FROM blogpost WHERE `blogId`=@blogId";
            var cmd2 = new MySqlCommand(sql2, connector);
            cmd2.Parameters.AddWithValue("@blogId", idf);
            var dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                var post = new post
                {
                    Title = dr2.GetString("Title"),
                    Content = dr2.GetString("Content"),
                };
                posts.Add(post);
            }

            connector.Close();
            return posts;
        }

        [HttpGet("Darab")]
        public object GetAllPost()
        {
            List<post> posts = new();
            int darab = 0;
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = "SELECT * FROM blogpost";
            var cmd = new MySqlCommand(sql, connector);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var post = new post
                {
                    Id = dr.GetInt32("Id"),
                    Title = dr.GetString("Title"),
                    Content = dr.GetString("Content"),
                    postTime = dr.GetDateTime("postTime"),
                    updateTime = dr.GetDateTime("updateTime"),
                    blogId = dr.GetInt32("blogId")
                };
                posts.Add(post);
                darab++;
            }
            connector.Close();
            return "Ennyi darab post van:" + darab;
        }
        [HttpGet("Darab2")]
        public object GetAllPostBlog(int id)
        {
            List<post> posts = new();
            int darab = 0;
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = "SELECT * FROM blogpost WHERE `blogId`=@blogId";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@blogId", id);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var post = new post
                {
                    Id = dr.GetInt32("Id"),
                    Title = dr.GetString("Title"),
                    Content = dr.GetString("Content"),
                    postTime = dr.GetDateTime("postTime"),
                    updateTime = dr.GetDateTime("updateTime"),
                    blogId = dr.GetInt32("blogId")
                };
                posts.Add(post);
                darab++;
            }
            connector.Close();
            return "Ennyi postja van a keresett bloggernek:" + darab;
        }
    }
}
