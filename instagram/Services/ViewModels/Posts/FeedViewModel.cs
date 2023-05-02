using instagram.Models;
using instagram.Services.ViewModels.Users;

namespace instagram.Services.ViewModels.Posts;

public class FeedViewModel
{
    public UserShortViewModel User { get; set; }
    public List<PostViewModel>? Posts { get; set; }
    public List<SubscribersViewModel>? Subscribers { get; set; }
}