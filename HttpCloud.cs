using System.Net;
using System.Net.Mail;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace My.Cloud.Functions;

//EmailAddressAttribute
//MailAddress
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
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req
    )
    {
        string? body = await req.ReadAsStringAsync();

        JsonNode? data = JsonNode.Parse(body);
        string? name = data?["name"]?.ToString();

        string? email = data?["email"]?.ToString();

        //Todo om inget namn eller mail är givet, felmeddelande och hantera i frontend.

        var message = $"Welcome to Azure Functions!";

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await response.WriteStringAsync(message);
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

    [HttpResult]
    public HttpResponseData HttpResponse { get; set; }
}
