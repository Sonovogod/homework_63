using instagram.Models;
using instagram.Services.ViewModels.Users;
using Microsoft.AspNetCore.Identity;

namespace instagram.Services.Abstracts;

public interface IAccountService
{
    public bool UserNameUnique(string userName);
    public bool UserEmailUnique(string email);
    public Task<IdentityResult> Add(UserRegisterViewModel model);
    public Task<User?> FindByEmailOrLoginAsync(string key);
    Task<bool> ToFollow(string userName, string subscribeName);
    Task<List<User>> Search(string key);
    Task<List<User>> GetFollowers(string userName);
    Task<List<User>> GetSubscribe(string userName);
    public User GetByUserName(string key);
}