using instagram.Enums.File;
using instagram.Extension;
using instagram.Models;
using instagram.Services.Abstracts;
using instagram.Services.ViewModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace instagram.Controllers;

public class PostsController : Controller
{
    private readonly IFileService _fileService;
    private readonly IPostService _postService;
    private readonly IAccountService _accountService;

    public PostsController(IFileService fileService, IPostService postService, IAccountService accountService)
    {
        _fileService = fileService;
        _postService = postService;
        _accountService = accountService;
    }

    [HttpGet]
    [Authorize]
    public IActionResult AddPost()
    {
        PostCreateViewModel model = new PostCreateViewModel();
        return View(model);
    }
    
    [HttpPost]
    [Authorize]
    public IActionResult AddPost(PostCreateViewModel model, IFormFile uploadedFile)
    {
        bool fileValid = _fileService.FileValid(uploadedFile, ImageType.Post);
        if (ModelState.IsValid && fileValid)
        {
            string filePath = _fileService.SaveImage(uploadedFile, ImageType.Post);
            string creatorId = _accountService.FindByEmailOrLoginAsync(User.Identity.Name).Result.Id;
            model.ImgPath = filePath;
            _postService.Add(model, creatorId);
            return RedirectToAction("Profile", "Account", new {userName = User.Identity.Name});
        }

        if (!fileValid)
            ModelState.AddModelError("imegeError", $"Ошибка загрузки картинки, одна из сторон меньше необходиомй или неподходящее расширение");
        else
            ModelState.AddModelError("modelError", "Ошибка при создании поста");
        
        return View(model);
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Feed()
    {
       User? totalUser = await _accountService.FindByEmailOrLoginAsync(User.Identity.Name);
        if (totalUser != null)
        {
            try
            {
                List<User> userSubscribers = await _accountService.GetSubscribe(totalUser.UserName);
                List<Post> subscribesPosts = userSubscribers.MapToSubscribePosts();

                FeedViewModel model = new FeedViewModel()
                {
                    Posts = subscribesPosts.MapToPostViewModel(),
                    User = totalUser.MapToUserShortViewModel(),
                    Subscribers = userSubscribers.MapToSubscribersViewModel()
                };
                return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }
        }
        return NotFound();
    }

    [HttpGet]
    [Authorize]
    public IActionResult FullPost(int postId)
    {
        Post? post = _postService.GetPostById(postId);
        if (post is null)
            return NotFound();
        
        PostViewModel postViewModel = post.MapToPostViewModel();
        PostFullInfoViewModel model = new PostFullInfoViewModel()
        {
            PostViewModel = postViewModel
        };
        return View(model);
    }

    [HttpGet]
    [Authorize]
    public IActionResult Like(string userName, int postId, string route = "FullPost")
    {
        if (User.Identity.Name.ToUpper().Equals(userName.ToUpper()))
        {
            _postService.Like(userName, postId);
            return RedirectToAction(route, "Posts", new { postId = postId });
        }
        return NotFound();
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public IActionResult Comment(PostFullInfoViewModel model)
    {
        string comment = model.Comment;
        string userName = model.UserName;
        int postId = model.PostViewModel.Id;
        if (User.Identity.Name.ToUpper().Equals(model.UserName.ToUpper()))
        {
            _postService.AddComment(comment, userName, postId);
            return RedirectToAction("FullPost", "Posts", new { postId = model.PostViewModel.Id });
        }

        return NotFound();
    }
}