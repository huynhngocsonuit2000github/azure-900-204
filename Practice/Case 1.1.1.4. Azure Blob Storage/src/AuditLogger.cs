using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Blobs;
using System.Text;
using Newtonsoft.Json;

public class AuditLogger
{
    private readonly string _connStr;
    private readonly string _containerName = "auditlogs";

    public AuditLogger(string connStr)
    {
        _connStr = connStr;
    }

    public async Task LogAsync(object auditData)
    {
        string blobName = $"logs-{DateTime.UtcNow:yyyy-MM-dd}.log";
        var containerClient = new BlobContainerClient(_connStr, _containerName);
        await containerClient.CreateIfNotExistsAsync();

        var appendBlobClient = containerClient.GetAppendBlobClient(blobName);

        if (!await appendBlobClient.ExistsAsync())
        {
            await appendBlobClient.CreateAsync();
        }

        var json = JsonConvert.SerializeObject(auditData) + Environment.NewLine;
        var bytes = Encoding.UTF8.GetBytes(json);
        using var stream = new MemoryStream(bytes);
        await appendBlobClient.AppendBlockAsync(stream);
    }
}
