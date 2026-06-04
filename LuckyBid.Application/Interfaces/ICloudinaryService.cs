using System.IO;
using System.Threading.Tasks;

namespace LuckyBid.Application.Interfaces;

public interface ICloudinaryService
{
    Task<string> UploadImageAsync(Stream fileStream, string fileName);
    Task<bool> DeleteImageAsync(string publicId);
}
