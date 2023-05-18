using instagram.Models;
using instagram.Services.ViewModels.Posts;

namespace instagram.Services.Abstracts;

public interface IPostService
{
    void Add(PostCreateViewModel model, string creatorId);
    Post? GetPostById(int postId);
    void Like(string userName, int postId);
    void AddComment(string modelComment, string modelUserName, int postId);
    public void DeletePost(int postId);
    void EditPost(int postId, string content);
}