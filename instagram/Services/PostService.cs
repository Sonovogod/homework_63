using instagram.Extension;
using instagram.Models;
using instagram.Services.Abstracts;
using instagram.Services.ViewModels.Posts;
using Microsoft.EntityFrameworkCore;

namespace instagram.Services;

public class PostService : IPostService
{
    private readonly InstagramContext _db;
    private readonly IAccountService _accountService;

    public PostService(InstagramContext db, IAccountService accountService)
    {
        _db = db;
        _accountService = accountService;
    }

    public void Add(PostCreateViewModel model, string creatorId)
    {
        Post post = model.PostExtensionViewModel();
        post.DateOfCreate = DateTime.Now;
        post.CreatorId = creatorId;
        
        _db.Posts.Add(post);
        _db.SaveChanges();
    }

    public Post? GetPostById(int postId)
    {
        Post? post = _db.Posts
            .Include(x=> x.Comments)
            .ThenInclude(x=> x.Commentator)
            .Include(x=> x.Likes)
            .Include(x=> x.Creator)
            .FirstOrDefault(x => x.Id == postId);
        return post;
    }

    public void Like(string userName, int postId)
    {
        Like like = new Like();
            Post? post = GetPostById(postId);
            User? user = _accountService.GetByUserName(userName);
            if (post is not null && user is not null)
            {
                like = new Like()
                {
                    DateOfLiked = DateTime.Now,
                    PostId = post.Id,
                    UserId = user.Id
                };
                Like? existLike = post.Likes.FirstOrDefault(lx => lx.PostId == like.PostId && lx.UserId == like.UserId);
                if (existLike != null)
                    post.Likes.Remove(existLike);
                else
                    post.Likes.Add(like);
            }
            _db.SaveChanges();
    }

    public void AddComment(string modelComment, string modelUserName, int postId)
    {
        User? user = _accountService.GetByUserName(modelUserName);
        Post? post = GetPostById(postId);
        if (post is not null && user is not null)
        {
            Comment comment = new Comment()
            {
                CommentatorId = user.Id,
                Content = modelComment,
                DateOfCreat = DateTime.Now,
                PostId = post.Id
            };
             _db.Comments.Add(comment);
             _db.SaveChanges();
        }
    }

    public void DeletePost(int postId)
    {
        Post? post = GetPostById(postId);
        if (post is not null)
        {
            post.IsDelete = true;
            _db.Posts.Update(post);
            _db.SaveChanges();
        }
    }
    
    public void EditPost(int postId, string content)
    {
        Post? post = GetPostById(postId);
        if (post is not null)
        {
            post.Content = content;
            _db.Posts.Update(post);
            _db.SaveChanges();
        }
    }
}