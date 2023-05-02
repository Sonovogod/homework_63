using System.ComponentModel.DataAnnotations;
using instagram.Enums.User;
using Microsoft.AspNetCore.Mvc;

namespace instagram.Services.ViewModels.Users;

public class UserRegisterViewModel
{
    [Display(Name = "Логин")]
    [StringLength(30, MinimumLength = 1, ErrorMessage = "Минимальное количество знаков: 1, Максимальное - 30")]
    [Required(ErrorMessage = "Поле не можен быть пустым")]
    [RegularExpression(@"^[^@А-Яа-я]*$", ErrorMessage = "Не допускается использование кириллицы и знака '@' в логине")]
    [Remote("CheckUniqueName", "AccountValidation", ErrorMessage = "Такое имя уже есть", AdditionalFields = "UserName")]
    public string UserName { get; set; }
    
    [Display(Name = "Эл. почта")]
    [Required(ErrorMessage = "Поле не можен быть пустым")]
    [EmailAddress (ErrorMessage = "Некорректный адрес")]
    [RegularExpression(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$", ErrorMessage = "Неверный формат ввода")]
    [Remote("CheckUniqueEmail", "AccountValidation", ErrorMessage = "Такая почта уже зарегистрирована", AdditionalFields = "Email")]
    public string Email { get; set; }
    
    [Display(Name = "Фото профиля")]
    public string? Avatar { get; set; }
    
    [Display(Name = "Пароль")]
    [Required(ErrorMessage = "Поле не может быть пустым")]
    [MinLength(5, ErrorMessage = "Минимальная длина поля должна быть не менее 5 символов.")]
    public string Password { get; set; }
    
    [Display(Name = "Подтверждение пароля")]
    [Required(ErrorMessage = "Поле не можен быть пустым")]
    [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; }
    
    [Display(Name = "Ваше имя (необязательно)")]
    [StringLength(30, ErrorMessage = "Максимальное кол-во знаков 30 символов")]
    public string? Name { get; set; }
    
    [Display(Name = "Информация о пользователе")]
    [StringLength(30, ErrorMessage = "Максимальное кол-во знаков 30 символов")]
    public string? UserInfo { get; set; }
    
    [Display(Name = "Номер телефона")]
    [StringLength(16, ErrorMessage = "Максимальное кол-во знаков 16 символов")]
    [RegularExpression(@"^[+]?[0-9]*$", ErrorMessage = "Неверный формат телефона")]
    public string? PhoneNumber { get; set; }
    
    [Display(Name = "Пол")]
    public Gender? Gender { get; set; }
    public string? ReturnUrl { get; set; }
}