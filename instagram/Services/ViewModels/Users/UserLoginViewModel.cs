using System.ComponentModel.DataAnnotations;

namespace instagram.Services.ViewModels.Users;

public class UserLoginViewModel
{
    [Display(Name = "Эл. адрес или логин пользователя")]
    [Required(ErrorMessage = "Поле не может быть пустым")]
    public string EmailOrLogin { get; set; }
    [Display(Name = "Пароль")]
    [Required(ErrorMessage = "Поле не может быть пустым")]
    public string Password { get; set; }
    public string? ReturnUrl { get; set; }
}