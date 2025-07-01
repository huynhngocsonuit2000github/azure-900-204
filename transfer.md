Create a .NET 8 HTTP-triggered Azure Function, test locally, and deploy to Azure
💡 Tech: .NET 8, Azure CLI, Azure Functions Core Tools, RunFromPackage

- Install Azure Function extension on the VS Code
- Install the dotnet web
- publish the code into the Azure Function
  func init --worker-runtime dotnetIsolated --target-framework net8.0
  func new --name HelloWorld --template "HTTP trigger" --authlevel "Anonymous"
  func start # (optional: test locally)
  func azure functionapp publish my-function-app
