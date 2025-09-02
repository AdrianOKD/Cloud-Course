using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace My.Cloud.Functions;

//Todo Read up on HttpResultsAttribute. Can ignore for now.
public class HttpCloud
{
    private readonly ILogger<HttpCloud> _logger;

    public HttpCloud(ILogger<HttpCloud> logger)
    {
        _logger = logger;
    }

    [Function("HttpCloud")]
    public MultiResponse Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
        FunctionContext executionContext
    )
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        string name = "";

        string email = "";

        var message = $"Welcome to Azure Functions!";

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        response.WriteStringAsync(message);

        // Return a response to both HTTP trigger and Azure Cosmos DB output binding.
        return new MultiResponse()
        {
            Document = new Visitor
            {
                Id = System.Guid.NewGuid().ToString(),
                Name = name,
                Email = email,
                Message = message,
            },
            HttpResponse = response,
        };
    }
}

public class MultiResponse
{
    [CosmosDBOutput(
        "my-database",
        "my-container",
        Connection = "CosmosDbConnectionString",
        CreateIfNotExists = true
    )]
    public Visitor Document { get; set; }
    public HttpResponseData HttpResponse { get; set; }
}

public class Visitor
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }
    public string Message { get; set; }
}
