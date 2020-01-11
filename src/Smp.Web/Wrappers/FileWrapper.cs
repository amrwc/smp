using System.Threading.Tasks;
using System.IO;

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