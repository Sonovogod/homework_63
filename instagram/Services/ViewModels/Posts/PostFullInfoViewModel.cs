using System.ComponentModel.DataAnnotations;

namespace instagram.Services.ViewModels.Posts;

public class PostFullInfoViewModel
{
    public PostViewModel? PostViewModel { get; set; }
    [MaxLength(200, ErrorMessage = "Не более 200 символов")]
    [Required(ErrorMessage = "Нельзя отправить пустой коммент")]
    public string Comment { get; set; }
    [Required(ErrorMessage = "Пользователь не найден")]
    public string UserName { get; set; }
}