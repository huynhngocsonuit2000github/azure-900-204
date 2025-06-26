# 1. Basic application

- [WIP]: using Azure Blob Storage in an ASP.NET Core Web API application to upload, download, list, and delete files.

1. Create Azure Storage Account
   - Create Storage Account
   - Go to this resource, and create Container, and change the access level to public
   - Get the Access Keys to use as a Connection string
2. Install .NET application and connect to this storage

# 2. Enhances

- Host a static website
- Implement audit feature

## 2.1. Host a static website
- We will host a static website with only files: index.html, style.css, script.js
- Step to do:
  - Enable **Static website** inside the Storage Account -> It will create the **Primary Endpoint** after saving
  - Then there is a container **$web** created
  - Go to the container and upload the **index.html, style.css, script.js** file

## 2.2. Implement audit feature
- Create container called auditlogs
- Create the .Net application and use AppendBlockAsync() method to append the log data into existing Container
  - dotnet add package Azure.Storage.Blobs
  - dotnet add package Azure.Storage.Blobs.Specialized
