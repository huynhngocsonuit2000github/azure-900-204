using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var blobService = new BlobService(config);

// ✅ Step 1: Create a local test file
string localFilePath = "verify-blob-3.txt";
File.WriteAllText(localFilePath, "My name is Son Huynh from icon -- 3");

// ✅ Step 2: Upload to Azure Blob
using var stream = File.OpenRead(localFilePath);
await blobService.UploadFileAsync(localFilePath, stream);
Console.WriteLine($"✅ Uploaded '{localFilePath}'");

// ✅ Step 3: List blobs in the container
var files = await blobService.ListFilesAsync();
Console.WriteLine("📂 Blobs in container:");
files.ForEach(name => Console.WriteLine(" - " + name));
