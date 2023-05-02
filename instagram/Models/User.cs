using instagram.Enums.User;
using Microsoft.AspNetCore.Identity;

namespace instagram.Models;

public class User : IdentityUser
{
    public string Avatar { get; set; }
    public string? Name { get; set; }
    public string? UserInfo { get; set; }
    public Gender? Gender { get; set; }
    public List<Post> Posts { get; set; } = new List<Post>();
    public List<UserFollower> Followers { get; set; } = new List<UserFollower>();
    public List<UserSubscription> Subscriptions { get; set; } = new List<UserSubscription>();
    public DateTime DateOfCreate { get; set; }
}