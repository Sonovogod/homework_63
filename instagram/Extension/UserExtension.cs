using instagram.Models;
using instagram.Services.ViewModels.Users;

namespace instagram.Extension;

public static class UserExtension
{
    public static User MapToUserModel(this UserRegisterViewModel model)
    {
        return new User()
        {
            UserName = model.UserName,
            Email = model.Email,
            Avatar = model.Avatar,
            Name = model.Name,
            UserInfo = model.UserInfo,
            PhoneNumber = model.PhoneNumber,
            Gender = model.Gender
        };
    }
    
    public static UserProfileViewModel MapToUserProfileViewModel(this User model)
    {
        return new UserProfileViewModel()
        {
            UserName = model.UserName,
            Email = model.Email,
            Avatar = model.Avatar,
            Name = model.Name,
            UserInfo = model.UserInfo,
            PhoneNumber = model.PhoneNumber,
            Posts = model.Posts,
            Followers = model.Followers,
            Subscriptions = model.Subscriptions
        };
    }
    public static UserShortViewModel MapToUserShortViewModel(this User model)
    {
        return new UserShortViewModel()
        {
            Avatar = model.Avatar,
            Id = model.Id,
            UserName = model.UserName
        };
    }

    public static List<SubscribersViewModel> MapToSubscribersViewModel(this IEnumerable<User> model)
    {
        List<SubscribersViewModel> subscribersViewModels = model.Select(x => new SubscribersViewModel()
        {
            Id = x.Id,
            Avatar = x.Avatar,
            UserName = x.UserName
        }).ToList();
        return subscribersViewModels;
    }
    
    public static List<UserSearchViewModel> MapToUserResultProfile(this IEnumerable<User> model)
    {
        List<UserSearchViewModel> UserResultProfile = model.Select(x => new UserSearchViewModel()
        {
            Avatar = x.Avatar,
            UserName = x.UserName,
            UserInfo = x.UserInfo,
            Followers = x.Followers
        }).ToList();
        return UserResultProfile;
    }
    public static List<UserMiddleViewModel> MapToUserSortResultViewModel(this IEnumerable<User> model)
    {
        List<UserMiddleViewModel> UserResultProfile = model.Select(x => new UserMiddleViewModel()
        {
            Avatar = x.Avatar,
            UserName = x.UserName,
            UserInfo = x.UserInfo,
            Followers = x.Followers,
            Subscriptions = x.Subscriptions
        }).ToList();
        return UserResultProfile;
    }
}