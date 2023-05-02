using instagram.Enums.File;
using instagram.Extension;
using instagram.Models;
using instagram.Services.Abstracts;
using instagram.Services.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace instagram.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IFileService _fileService;
    private readonly SignInManager<User> _signInManager;

    public AccountController(IAccountService accountService, IFileService fileService, SignInManager<User> signInManager)
    {
        _accountService = accountService;
        _fileService = fileService;
        _signInManager = signInManager;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl)
    {
        return View(new UserLoginViewModel {ReturnUrl = returnUrl});
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(UserLoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            User? user = await _accountService.FindByEmailOrLoginAsync(model.EmailOrLogin);
            if (user is not null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

                if (signInResult.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    return RedirectToAction("Profile", new{userName = user.UserName});
                }

            }
            ModelState.AddModelError("incorrectAuthentication", "Некорректный логин и/или пароль");
        }
        return View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        UserRegisterViewModel model = new UserRegisterViewModel();
        return View(model);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(UserRegisterViewModel model, IFormFile uploadedFile)
    {
        if (ModelState.IsValid)
        {
            bool fileValid = _fileService.FileValid(uploadedFile, ImageType.Logo);
            if (fileValid)
            {
                string filePath = _fileService.SaveImage(uploadedFile, ImageType.Logo);
                model.Avatar = filePath;
                var result = await _accountService.Add(model);
                
                if (result.Succeeded)
                {
                    User? user = await _accountService.FindByEmailOrLoginAsync(model.Email);
                    await _signInManager.SignInAsync(user, true);
                    return RedirectToAction("Profile", "Account", new {userName = user.UserName});
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                ModelState.AddModelError("CreateUserError", "Ошибка при создании пользователя");
            }
            ModelState.AddModelError("incorrectLogo", "Ошибка загрузки, фото не соответсвует требованиям");
        }
        ModelState.AddModelError("incorrectRegistration", "Ошибка регистрации!");

        return View(model);
    }
    
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOff()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Profile(string userName)
    {
        User? user = await _accountService.FindByEmailOrLoginAsync(userName);
        var totalUserId = _accountService.FindByEmailOrLoginAsync(User.Identity.Name).Result.Id;
        if (user is not null)
        {
            UserProfileViewModel userView = user.MapToUserProfileViewModel();
            userView.Posts = user.Posts.OrderByDescending(x => x.DateOfCreate).ToList();
            ViewData.Add("totalUser", totalUserId);
            
            return View(userView);
        }
        
        return NotFound();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Follow(string? followerName)
    {
        if (!string.IsNullOrEmpty(followerName))
        {
            var totalUserId = _accountService.FindByEmailOrLoginAsync(User.Identity.Name).Result.Id;
            string userName = User.Identity.Name;
            if ( userName.ToLower() != followerName.ToLower() )
            {
                await _accountService.ToFollow(followerName, userName);
                return RedirectToAction("Profile", new { userName = followerName });
            }
            ViewData.Add("totalUser", totalUserId);
        }
        return NotFound();
    }
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetFollowers(string? userName)
    {
        if (!string.IsNullOrEmpty(userName))
        {
            var totalUserId = _accountService.FindByEmailOrLoginAsync(User.Identity.Name).Result.Id;
            List<User> users = await _accountService.GetFollowers(userName);
            UserSortResultViewModel model = new UserSortResultViewModel()
            {
                Users = users.MapToUserSortResultViewModel()
            };
            ViewData.Add("totalUser", totalUserId);
            return View(model);
        }
        return NotFound();
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetSubscribe(string? userName)
    {
        if (!string.IsNullOrEmpty(userName))
        {
            var totalUserId = _accountService.FindByEmailOrLoginAsync(User.Identity.Name).Result.Id;
            List<User> users = await _accountService.GetSubscribe(userName);
            UserSortResultViewModel model = new UserSortResultViewModel()
            {
                Users = users.MapToUserSortResultViewModel()
            };
            ViewData.Add("totalUser", totalUserId);
            return View(model);
        }
        return NotFound();
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Search(string key)
    {
        var totalUserId = _accountService.FindByEmailOrLoginAsync(User.Identity.Name).Result.Id;
        List<User> users = await _accountService.Search(key);
        UserResultProfile model = new UserResultProfile()
        {
            Users = users.MapToUserResultProfile()
        };
        ViewData.Add("totalUser", totalUserId);
        return View(model);
    }
}

