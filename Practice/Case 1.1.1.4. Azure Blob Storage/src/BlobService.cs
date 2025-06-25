using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;


public class BlobService
{
    private readonly BlobContainerClient _containerClient;

    public BlobService(IConfiguration config)
    {
        var connStr = config["AzureBlobStorage:ConnectionString"];
        var containerName = config["AzureBlobStorage:ContainerName"];

        _containerClient = new BlobContainerClient(connStr, containerName);
        _containerClient.CreateIfNotExists(PublicAccessType.Blob);
    }

    public async Task UploadFileAsync(string fileName, Stream content)
    {
        var blobClient = _containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(content, overwrite: true);
    }

    public async Task DownloadFileAsync(string fileName, string savePath)
    {
        var blobClient = _containerClient.GetBlobClient(fileName);
        await blobClient.DownloadToAsync(savePath);
    }

    public async Task<List<string>> ListFilesAsync()
    {
        var names = new List<string>();
        await foreach (var blob in _containerClient.GetBlobsAsync())
        {
            names.Add(blob.Name);
        }
        return names;
    }

    public async Task DeleteFileAsync(string fileName)
    {
        var blobClient = _containerClient.GetBlobClient(fileName);
        await blobClient.DeleteIfExistsAsync();
    }
}
