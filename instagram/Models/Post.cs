namespace instagram.Models;

public class Post
{
    public int Id { get; set; }
    public DateTime DateOfCreate { get; set; }
    public string ImgPath { get; set; }
    public string? Content { get; set; }
    public bool IsDelete { get; set; }
    public List<Like> Likes { get; set; } = new List<Like>();
    public List<Comment> Comments { get; set; } = new List<Comment>();
    
    public string CreatorId { get; set; }
    public User Creator { get; set; }
}