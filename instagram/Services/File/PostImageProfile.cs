using instagram.Enums.File;
using instagram.Services.Abstracts;

namespace instagram.Services.File;

public class PostImageProfile : IImageProfile
{
    public PostImageProfile(IEnumerable<string> allowedExtensions)
    {
        AllowedExtensions = new List<string>(){".jpg", ".jpeg", ".png"};
    }
    public ImageType ImageType => ImageType.Post;
    private const int mb = 1048576;
    public string Folder => "Post";
    public int Width => 1080;
    public int Height => 1080;
    public int MaxSizeBytes => 10 * mb;
    public IEnumerable<string> AllowedExtensions { get; }
}