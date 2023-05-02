using instagram.Models;

namespace instagram.Services.ViewModels.Posts;

public class PostViewModel
{
    public int Id { get; set; }
    public DateTime DateOfCreate { get; set; }
    public string ImgPath { get; set; }
    public string? Content { get; set; }
    public List<Like>? Likes { get; set; }
    public List<Comment>? Comments { get; set; }
    public string CreatorId { get; set; }
    public User Creator { get; set; }
}