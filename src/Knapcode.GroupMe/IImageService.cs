using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.GroupMe.Models;

namespace Knapcode.GroupMe
{
    public interface IImageService
    {
        Task<Image> UploadImageUrlAsync(string imageUrl, CancellationToken token);
        Task<Image> UploadStreamAsync(Stream stream, CancellationToken token);
    }
}