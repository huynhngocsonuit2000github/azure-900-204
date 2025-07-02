# 1. Basic application

- Create a .NET 8 HTTP-triggered Azure Function, test locally, and deploy to Azure. Tech: .NET 8, Azure CLI, Azure Functions Core Tools, RunFromPackage


1. At local, install Azure Functions Core Tools
   - npm install -g azure-functions-core-tools@4 --unsafe-perm true
2. Create the Azure Function Project
   ``````
   func init HttpFunctionDemo --worker-runtime dotnet
   cd HttpFunctionDemo
   func new --name HelloFunction --template "HTTP trigger"
   ``````

   This creates:
   ``````
   HttpFunctionDemo/
   ├── HelloFunction.cs      👈 Your function code
   ├── host.json             👈 Runtime config
   └── local.settings.json   👈 App settings for local run
   ``````

3. Start the function and test in local side
4. Create the Function App in Azure
   - Go to Function Apps -> create
   - Type: Consumption
   - Chose region: West Europe to avoid issue **Free Trial subscription has 0 quota for Dynamic (Consumption Plan) Azure Functions in the region you're trying to deploy to.**
5. Upload the source code via VS code extension
   <!-- - Go to Environment Variable and add value WEBSITE_RUN_FROM_PACKAGE=1 -->
  <!-- 
  func init --worker-runtime dotnetIsolated --target-framework net8.0
  func new --name HelloWorld --template "HTTP trigger" --authlevel "Anonymous"
  func start # (optional: test locally)
  func azure functionapp publish my-function-app 
  -->
