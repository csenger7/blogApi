using BlogApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using BlogApi.Models.DTOs;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController
    {
        private readonly string ConnectionString = "Server=localhost;Database=blog;User Id=root;Password=";
        [HttpGet]
        public List<post> GetAllPost()
        {
            List<post> posts = new();
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
            }
            connector.Close();
            return posts;
        }

        [HttpPost]
        public object AddNewBlogger(AddBlogPost post)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var pst = new post
            {
                Title = post.Title,
                Content = post.Content,
                postTime = DateTime.Now,
                updateTime = DateTime.Now,
                blogId = post.blogId
            };

            var sql = $"INSERT INTO `blogpost`(`Title`, `Content`, `postTime`, `updateTime`, `blogId`) VALUES (@title,@content,@postTime,@updateTime,@blogId)";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@title", pst.Title);
            cmd.Parameters.AddWithValue("@content", pst.Content);
            cmd.Parameters.AddWithValue("@postTime", pst.postTime);
            cmd.Parameters.AddWithValue("@updateTime", pst.updateTime);
            cmd.Parameters.AddWithValue("@blogId", pst.blogId);
            cmd.ExecuteNonQuery();
            connector.Close();
            return pst;
        }
        [HttpPut]
        public object UpdatePost(int id, post post)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var sql = $"UPDATE `blogpost` SET `Title`=@title,`Content`=@content,`updateTime`=@updateT WHERE `Id`=@id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@title", post.Title);
            cmd.Parameters.AddWithValue("@content", post.Content);
            cmd.Parameters.AddWithValue("@updateT", post.updateTime);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            connector.Close();
            return post;
        }
        [HttpDelete]
        public object DeletePost(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var sql = $"DELETE FROM `blogpost` WHERE `Id`=@id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            connector.Close();
            return "Kitörölve";
        }
    }
}

