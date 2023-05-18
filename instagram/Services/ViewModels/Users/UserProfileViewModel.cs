using System.ComponentModel.DataAnnotations;
using instagram.Models;
using instagram.Services.ViewModels.Posts;

namespace instagram.Services.ViewModels.Users;

public class UserProfileViewModel
{
    public string Avatar { get; set; }
    public string? Name { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string? UserInfo { get; set; }
    public List<Post> Posts { get; set; } = new List<Post>();
    public List<UserFollower> Followers { get; set; } = new List<UserFollower>();
    public List<UserSubscription> Subscriptions { get; set; } = new List<UserSubscription>();
    [MaxLength(2200, ErrorMessage = "Не более 2200 симоволов")]
    [Display(Name = "Описание")]
    public string? Content { get; set; }
}