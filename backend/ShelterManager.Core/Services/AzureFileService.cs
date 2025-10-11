using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using ShelterManager.Core.Services.Abstractions;

namespace ShelterManager.Core.Services;

public class AzureFileService : IFileService
{
    private const string ConnectionStringName = "AzureStorage";
    
    private readonly BlobServiceClient _blobServiceClient;
    
    public AzureFileService(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringName);
        
        _blobServiceClient = new BlobServiceClient(connectionString);
    }

    public async Task<Stream?> GetFileStreamAsync(string path)
    {
        var container = SplitContainer(path);
        if (container is null)
        {
            throw new ArgumentException("Cannot find container for path");
        }
     
        var containerClient = _blobServiceClient.GetBlobContainerClient(container);
        var blobClient = containerClient.GetBlobClient(RemoveContainer(path, container));

        if (!await blobClient.ExistsAsync())
        {
            return null;
        }
        
        var stream = await blobClient.OpenReadAsync();
        
        return stream;
    }

    public async Task UploadFileAsync(Stream stream, string path, Dictionary<string, string>? metadata = null)
    {
        var container = SplitContainer(path);
        if (container is null)
        {
            throw new ArgumentException("Cannot find container for path");
        }
        
        var containerClient = _blobServiceClient.GetBlobContainerClient(container);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
        
        var blobClient = containerClient.GetBlobClient(RemoveContainer(path, container));
        
        await blobClient.UploadAsync(stream, true);
        if (metadata is not null)
        {
            await blobClient.SetMetadataAsync(metadata);
        }
    }

    public async Task<bool> DeleteFileAsync(string path)
    {
        var container = SplitContainer(path);
        if (container is null)
        {
            throw new ArgumentException("Cannot find container for path");
        }
        
        var containerClient = _blobServiceClient.GetBlobContainerClient(container);
        var blobClient = containerClient.GetBlobClient(RemoveContainer(path, container));
        
        var result = await blobClient.DeleteIfExistsAsync();
        
        return result.Value;
    }

    public async Task<ICollection<IFileService.FileInfo>> ListFilesInFolderAsync(string path)
    {
        var container = SplitContainer(path);
        if (container is null)
        {
            throw new ArgumentException("Cannot find container for path");
        }
        
        var containerClient = _blobServiceClient.GetBlobContainerClient(container);
        var items = new List<IFileService.FileInfo>();
        
        await foreach (var blob in containerClient.GetBlobsByHierarchyAsync(prefix: RemoveContainer(path, container), delimiter: "/"))
        {
            items.Add(new IFileService.FileInfo
            {
                Name = blob.IsBlob ? blob.Blob.Name : blob.Prefix,
                IsFile = blob.IsBlob,
                IsDirectory = blob.IsPrefix
            });
        }
        
        return items;
    }
    
    private static string? SplitContainer(string path)
    {
        var index = path.IndexOf('/');
        return index <= 0 ? null : path[..index];
    }
    
    private static string RemoveContainer(string path, string container)
    {
        return path[(container.Length + 1)..];
    }
}