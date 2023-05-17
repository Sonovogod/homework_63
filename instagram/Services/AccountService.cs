using System.Text.RegularExpressions;
using instagram.Extension;
using instagram.Models;
using instagram.Services.Abstracts;
using instagram.Services.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace instagram.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<User> _db;
    private readonly InstagramContext _instagramContext;

    public AccountService(UserManager<User> db, InstagramContext instagramContext)
    {
        _db = db;
        _instagramContext = instagramContext;
    }

    public bool UserNameUnique(string userName)
        => !_db.Users.Any(x => x.UserName != null && x.UserName.ToLower().Equals(userName.ToLower()));

    public bool UserEmailUnique(string email)
        => !_db.Users.Any(x => x.Email != null && x.Email.ToLower().Equals(email.ToLower()));

    public async Task<IdentityResult> Add(UserRegisterViewModel model)
    {
        User user = model.MapToUserModel();
        user.UserName = model.UserName.ToLower();
        user.DateOfCreate = DateTime.Now;
        IdentityResult result = await _db.CreateAsync(user, model.Password);
        return result;
    }

    public async Task<User?> FindByEmailOrLoginAsync(string? key)
    {
        User? user = new User();
        if (key is null)
            return null;

        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        bool isMail = Regex.IsMatch(key, pattern);

        if (isMail)
            user = await _instagramContext.Users
                .Include(x=> x.Posts)
                .Include(x => x.Subscriptions)
                .Include(x => x.Followers)
                .FirstOrDefaultAsync(x => x.NormalizedEmail != null && x.NormalizedEmail.Equals(key.ToUpper()));
        else
            user = await _instagramContext.Users
                .Include(x => x.Subscriptions)
                .Include(x => x.Followers)
                .Include(x=> x.Posts)
                .FirstOrDefaultAsync(x => x.NormalizedUserName != null && x.NormalizedUserName.Equals(key.ToUpper()));

        return user;
    }

    public User GetByUserName(string key)
    {
        User? user = _instagramContext.Users
            .Include(x => x.Subscriptions)
            .Include(x => x.Followers)
            .Include(x=> x.Posts)
            .FirstOrDefault(x => x.NormalizedUserName != null && x.NormalizedUserName.Equals(key.ToUpper()));
        return user;
    }

    public async Task<bool> ToFollow(string userName, string subscribeName)
    {
        User? user = _instagramContext.Users
            .FirstOrDefault(x => x.UserName != null && x.UserName.Equals(userName));
        User? subscriptionUser = _instagramContext.Users
            .FirstOrDefault(x => x.UserName != null && x.UserName.Equals(subscribeName));

        if (user != null && subscriptionUser != null)
        {
            UserSubscription? existingSubscription = await _instagramContext.UserSubscriptions
                .FirstOrDefaultAsync(x => x.UserId == subscriptionUser.Id && x.SubscriptionId == user.Id);
            UserFollower? existingFollower = await _instagramContext.UserFollowers
                .FirstOrDefaultAsync(x => x.FollowerId == subscriptionUser.Id && x.UserId == user.Id);
            
            if (existingSubscription == null && existingFollower == null)
            {
                UserSubscription newSubscription = new UserSubscription()
                {
                    UserId = subscriptionUser.Id,
                    SubscriptionId = user.Id,
                    DateOfSubscription = DateTime.Now
                };
                UserFollower newFollower = new UserFollower()
                {
                    UserId = user.Id,
                    FollowerId = subscriptionUser.Id,
                    DateOfFollowing = DateTime.Now
                };
                await _instagramContext.UserSubscriptions.AddAsync(newSubscription);
                await _instagramContext.UserFollowers.AddAsync(newFollower);
                await _instagramContext.SaveChangesAsync();
                return true;
            }
            if (existingSubscription != null && existingFollower != null)
            {
                _instagramContext.UserSubscriptions.Remove(existingSubscription);
                _instagramContext.UserFollowers.Remove(existingFollower);
            }

            await _instagramContext.SaveChangesAsync();
        }
        return false;
    }

    public async Task<List<User>> Search(string key)
    {
        List<User> users = await _instagramContext.Users
            .Include(x => x.Subscriptions)
            .Include(x => x.Followers)
            .Include(x => x.Posts)
            .Where(x=> (x.UserName != null && EF.Functions.Like(x.UserName.ToLower(), $"%{key.ToLower()}%")) || 
                            (x.Email != null && EF.Functions.Like(x.Email.ToLower(), $"%{key.ToLower()}%")) ||
                            (x.Name != null && EF.Functions.Like(x.Name.ToLower(), $"%{key.ToLower()}%")))
            .ToListAsync();
        return users;
    }

    public async Task<List<User>> GetFollowers(string userName)
    {
        User? user = await _instagramContext.Users.FirstOrDefaultAsync(x =>
            x.NormalizedUserName != null && x.NormalizedUserName.Equals(userName.ToUpper()));
        List<User> followers = new List<User>();
        
        if (user is not null)
        {
            followers = _instagramContext.Users
                .Include(x => x.Followers)
                .Where(x=>x.Subscriptions.Any(x=> x.SubscriptionId == user.Id)).ToList();
        }
        return followers;
    }

    public async Task<List<User>> GetSubscribe(string userName)
    {
        User? user = await _instagramContext.Users.FirstOrDefaultAsync(x =>
            x.NormalizedUserName != null && x.NormalizedUserName.Equals(userName.ToUpper()));
        List<User> subscribe = new List<User>();
        
        if (user is not null)
        {
            subscribe = _instagramContext.Users
                .Include(x => x.Subscriptions)
                .Include(x => x.Posts)
                .ThenInclude(p => p.Likes)
                .Include(x => x.Posts)
                .ThenInclude(x=> x.Comments)
                .Include(x => x.Followers)
                .Where(x=>x.Followers.Any(x=> x.FollowerId == user.Id))
                .ToList();
        }
        return subscribe;
    }
}