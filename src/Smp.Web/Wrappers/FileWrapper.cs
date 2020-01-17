using System.IO;
using System.Threading.Tasks;

namespace Smp.Web.Wrappers
{
    public interface IFileWrapper
    {
        string ReadAllText(string path);
        Task<string> ReadAllTextAsync(string path);
    }

    public class FileWrapper : IFileWrapper
    {
        public string ReadAllText(string path)
            => File.ReadAllText(path);

        public async Task<string> ReadAllTextAsync(string path)
            => await File.ReadAllTextAsync(path);
    }
}