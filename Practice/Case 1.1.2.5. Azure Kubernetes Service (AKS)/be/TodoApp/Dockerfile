# Use official .NET SDK to build (force AMD64)
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY . ./

# Build explicitly for linux-x64 to avoid exec format errors
RUN dotnet publish -c Release -r linux-x64 --self-contained false -o out

# Use ASP.NET Runtime to run (force AMD64)
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .

# Listen on port 5000
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

ENTRYPOINT ["dotnet", "TodoApp.dll"]
