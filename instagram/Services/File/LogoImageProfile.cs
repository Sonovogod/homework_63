using instagram.Enums.File;
using instagram.Services.Abstracts;

namespace instagram.Services.File;

public class LogoImageProfile : IImageProfile
{
    public LogoImageProfile(IEnumerable<string> allowedExtensions)
    {
        AllowedExtensions = new List<string>(){".jpg", ".jpeg", ".png"};
    }
    public ImageType ImageType => ImageType.Logo;
    private const int mb = 1048576;
    public string Folder => "Logo";
    public int Width => 110;
    public int Height => 110;
    public int MaxSizeBytes => 10 * mb;
    public IEnumerable<string> AllowedExtensions { get; }
}