using instagram.Models;

namespace instagram.Services.ViewModels.Users;

public class UserMiddleViewModel
{
    public string Avatar { get; set; }
    public string UserName { get; set; }
    public string? UserInfo { get; set; }
    public List<UserFollower> Followers { get; set; } = new List<UserFollower>();
    public List<UserSubscription> Subscriptions { get; set; } = new List<UserSubscription>();
}