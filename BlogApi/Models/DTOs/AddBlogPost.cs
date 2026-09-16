namespace BlogApi.Models.DTOs
{
    public class AddBlogPost
    {
        public string? Title { get; set; }

        public string? Content { get; set; }

        public DateTime? postTime { get; set; }

        public DateTime? updateTime { get; set; }

        public int blogId { get; set; }

    }
}
