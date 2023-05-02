using System.ComponentModel.DataAnnotations;

namespace instagram.Services.ViewModels.Posts;

public class PostCreateViewModel
{
    [Display(Name = "Картинка")]
    public string? ImgPath { get; set; }
    [MaxLength(2200, ErrorMessage = "Не более 2200 симоволов")]
    [Display(Name = "Описание")]
    public string? Content { get; set; }
}