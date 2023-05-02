using instagram.Models;

namespace instagram.Services.ViewModels.Users;

public class UserSearchViewModel
{
    public string Avatar { get; set; }
    public string UserName { get; set; }
    public string? UserInfo { get; set; }
    public List<UserFollower> Followers { get; set; } = new List<UserFollower>();
}