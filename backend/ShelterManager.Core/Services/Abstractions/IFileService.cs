namespace ShelterManager.Core.Services.Abstractions;

public interface IFileService
{
    Task<Stream?> GetFileStreamAsync(string path);
    Task UploadFileAsync(Stream stream, string path, Dictionary<string, string>? metadata = null);
    Task<bool> DeleteFileAsync(string path);
    Task<ICollection<FileInfo>> ListFilesInFolderAsync(string path);

    public sealed record FileInfo
    {
        public required string Name { get; init; }
        public bool IsFile { get; init; }
        public bool IsDirectory { get; init; }
    }
}