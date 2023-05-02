using instagram.Models;
using instagram.Services.ViewModels.Posts;

namespace instagram.Extension;

public static class PostExtension
{
    public static Post PostExtensionViewModel(this PostCreateViewModel model)
    {
        return new Post()
        {
            ImgPath = model.ImgPath,
            Content = model.Content
        };
    }

    public static List<Post> MapToSubscribePosts(this IEnumerable<User> model)
    {
        List<Post> posts = new List<Post>();
        foreach (var user in model)
        {
            posts.AddRange(user.Posts);
        }
        return posts.OrderByDescending(x => x.DateOfCreate).ToList();
    }

    public static List<PostViewModel> MapToPostViewModel(this IEnumerable<Post> model)
    {
        List<PostViewModel> postViewModel = model.Select(x => new PostViewModel()
        {
            Comments = x.Comments,
            Content = x.Content,
            DateOfCreate = x.DateOfCreate,
            Id = x.Id,
            ImgPath = x.ImgPath,
            Likes = x.Likes,
            CreatorId = x.CreatorId,
            Creator = x.Creator
        }).ToList();
        return postViewModel;
    }
    
    public static PostViewModel MapToPostViewModel(this Post model)
    {
        return new PostViewModel()
        {
            Comments = model.Comments,
            Content = model.Content,
            Creator = model.Creator,
            CreatorId = model.CreatorId,
            DateOfCreate = model.DateOfCreate,
            Id = model.Id,
            ImgPath = model.ImgPath,
            Likes = model.Likes
        };
    }
}