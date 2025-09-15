# Cloud-Course

A simple check-in web application built with Azure cloud services.

## Description

This is a simple check in web application connected to Azure.
It allows users to check in with their name and email.  
* Frontend: Hosted on Github Pages.
* Backend: Azure functions for serverless API.
* Database: Azure Cosmos DB, a NoSQL DB for storage.

## Getting Started

### Dependencies

* .NET 8.0 SDK
* Windows 10/11
* Azure Tools for VS Code

🚀### How to run

**Option 1: Try directly**

 1. **Frontend**

    Visit my github pages site:
    [My cloud cours program](https://adrianokd.github.io/Cloud-Course)

 2.**Backend**
    
The site is already running and is connected to my Azure Functions backend.
No need to clone repoistory or configure any code to try it out.
    

🪛### Option 2: Local Development 

If you want to run or modify the code locally, follow these steps:

1. Clone the repository:
   ```
   git clone https://github.com/AdrianOKD/Cloud-Course.git
   ```

2. Install .NET 8.0 SDK and Azure Tools for VS Code.

3. Set up your own Azure resources (Function App, Cosmos DB, etc.)
 ```
   Add your connection strings to `local.settings.json`.

   * Example:
{
  "IsEncrypted": false,
  "Values": {
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "AzureWebJobsStorage": "<your-azure-storage-connection-string>",
    "CosmosDbConnectionString": "<your-cosmos-db-connection-string>"
  },
  "Host": {
    "CORS": "*"
  }
}
 ```
4. Run the backend locally:
   ```
   func start
   ```
5. Open `docs/index.html` in your browser for the frontend.

---

🔧 Configuration
CORS Setup
For the frontend to communicate with your Azure Function App:

1. Navigate to your Function App in Azure Portal
2. Go to Settings → CORS
3. For development: Add *
4. For production: Add your GitHub Pages URL: https://yourusername.github.io

⚠️ Security Note: Only use * during development. Always specify exact origins in production.

## Authors

Adrian Dahl

## License

This project is licensed under the MIT-License.

## Acknowledgments
* [Readme-templet](https://gist.github.com/DomPizzie/7a5ff55ffa9081f2de27c315f5018afc)
* [Azure Functions Documentation](https://learn.microsoft.com/en-us/azure/azure-functions)
* [Tutorial Azure/CosmosDB](https://learn.microsoft.com/en-us/azure/azure-functions/how-to-create-function-vs-code?pivots=programming-language-csharp)
* [W3School](https://www.w3schools.com)
* [Azure Functions setup tutorial](https://www.youtube.com/watch?v=_9moXcR2Suo)
