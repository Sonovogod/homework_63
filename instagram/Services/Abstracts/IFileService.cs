using instagram.Enums.File;

namespace instagram.Services.Abstracts;

public interface IFileService
{
    public bool FileValid(IFormFile uploadedFile, ImageType imageType);
    public string SaveImage(IFormFile uploadedFile, ImageType imageType);
}