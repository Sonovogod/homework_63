using instagram.Enums.File;
using instagram.Services.Abstracts;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace instagram.Services.File;

public class FileService : IFileService
{
    private readonly IEnumerable<IImageProfile> _imageProfiles;
    private readonly IWebHostEnvironment _hostEnvironment;

    public FileService(IEnumerable<IImageProfile> imageProfiles, IWebHostEnvironment hostEnvironment)
    {
        _imageProfiles = imageProfiles;
        _hostEnvironment = hostEnvironment;
    }

    public bool FileValid(IFormFile uploadedFile, ImageType imageType)
    {
        try
        {
            if (uploadedFile.Length == 0)
                throw new Exception();
            var imageProfile = _imageProfiles.FirstOrDefault(profile => profile.ImageType == imageType);
            var fileExtension = Path.GetExtension(uploadedFile.FileName);
            Image image = Image.Load(uploadedFile.OpenReadStream());
            
            if (imageProfile == null)
                throw new Exception();
            if (!imageProfile.AllowedExtensions.Any(x=> x.Equals(fileExtension.ToLower())))
                throw new Exception();
            if (uploadedFile.Length > imageProfile.MaxSizeBytes)
                throw new Exception();
            if (image.Width < (imageProfile.Width / 2) || image.Height < (imageProfile.Height / 2))
                throw new Exception();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    public string SaveImage(IFormFile uploadedFile, ImageType imageType)
    {
        var imageProfile = _imageProfiles.FirstOrDefault(profile => profile.ImageType == imageType);
        var fileExtension = Path.GetExtension(uploadedFile.FileName);
        Image image = Image.Load(uploadedFile.OpenReadStream());
        var folderPath = Path.Combine(_hostEnvironment.WebRootPath, imageProfile.Folder);

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
        var fileName = $"{Path.GetFileNameWithoutExtension(Path.GetRandomFileName())}{fileExtension}";
        string filePath = Path.Combine(folderPath, fileName);

        var resizeOption = new ResizeOptions()
        {
            Mode = ResizeMode.Crop,
            Size = new Size(imageProfile.Width, imageProfile.Height)
        };
        image.Mutate(action => action.Resize(resizeOption));

        image.Save(filePath, new JpegEncoder() { Quality = 100 });
        return Path.Combine(@$"\{imageProfile.Folder}", fileName);
    }
}