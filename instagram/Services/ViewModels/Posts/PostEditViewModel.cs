using System.ComponentModel.DataAnnotations;

namespace instagram.Services.ViewModels.Posts;

public class PostEditViewModel
{
    [MaxLength(2200, ErrorMessage = "Не более 2200 симоволов")]
    [Display(Name = "Описание")]
    public string? Content { get; set; }

    public int PostId { get; set; }
    public string? PostOwner { get; set; }
}