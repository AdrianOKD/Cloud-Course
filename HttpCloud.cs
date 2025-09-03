using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Google.Protobuf;
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
    public async Task<MultiResponse> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req
    )
    {
        //string body = await new StreamReader(req.Body).ReadToEndAsync();
        string body = await req.ReadAsStringAsync();
        _logger.LogInformation("Bärjar läsa body");

        _logger.LogInformation("C# HTTP trigger function processed a request.");

        JsonNode data = JsonNode.Parse(body);
        _logger.LogInformation("parsa body.");

        string? name = data?["name"]?.ToString();

        _logger.LogInformation("Hämta namn.");

        string? email = data?["email"]?.ToString();
        _logger.LogInformation("hämta email");
        _logger.LogInformation($"Name: '{name}', Email: '{email}'");
        //Todo om inget namn eller mail är givet, felmeddelande och hantera i frontend.

        var message = $"Welcome to Azure Functions!";

        var response = req.CreateResponse(HttpStatusCode.OK);
        _logger.LogInformation("hämta response");
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        response.WriteStringAsync(message);

        // Return a response to both HTTP trigger and Azure Cosmos DB output binding.
        return new MultiResponse()
        {
            Document = new Visitor
            {
                id = System.Guid.NewGuid().ToString(),
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
    public string id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
}
