using instagram.Models;

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
}