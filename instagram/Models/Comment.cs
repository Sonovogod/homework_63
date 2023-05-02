namespace instagram.Models;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime DateOfCreat { get; set; }

    public string CommentatorId { get; set; }
    public User Commentator { get; set; }
    public int PostId { get; set; }
    public Post Post { get; set; }
}