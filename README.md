# Cloud-Course

A check in web app.

## Description

This is a simple check in web application connected to Azure. 
It uses github pages as frontend and talks to an azure database via function app.
It registeres user with name and email, and the time they checked in according to UTC time. 

## Getting Started

### Dependencies

* .NET 8.0
* Windows 10/11

### How to run

 1. **Frontend**

    Visit my github pages site:
    [My cloud cours program](https://adrianokd.github.io/Cloud-Course)

 2.**Backend**
    
The site is already running and is connected to my Azure Functions backend.
No need to clone repoistory or configure any code to try it out.
    

### For Local Development (Optional)

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

## Help

Any advise for common problems or issues.

To have your frontend able to access your backend in azure you have to open cors in Function app settings and add a
"*" (without quotation marks). Only use the * when developing, when you have everything set up use your  https://<yourUserName>.github.io 
to have only your github pages communicate with the function app.


## Authors

Adrian Dahl

## License

This project is licensed under the MIT-License.

## Acknowledgments

Inspiration, code snippets, etc.
* [Readme-templet](https://gist.github.com/DomPizzie/7a5ff55ffa9081f2de27c315f5018afc)
* [Tutorial Azure/CosmosDB](https://learn.microsoft.com/en-us/azure/azure-functions/how-to-create-function-vs-code?pivots=programming-language-csharp)
* [W3School](https://www.w3schools.com)
