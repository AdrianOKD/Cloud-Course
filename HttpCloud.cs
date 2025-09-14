using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace My.Cloud.Functions;

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
        try
        {
            string? body = await req.ReadAsStringAsync();
            if (body == null)
            {
                throw new ArgumentNullException("The body is null");
            }
            JsonNode? data = JsonNode.Parse(body);
            if (data == null)
            {
                throw new ArgumentNullException();
            }
            //extracts and validate required fields in visitor.
            string? name = data?["name"]?.ToString();
            if (name == null)
            {
                throw new ArgumentNullException("name is required");
            }

            string? email = data?["email"]?.ToString();

            if (email == null)
            {
                throw new ArgumentNullException("email is required");
            }
            //Validate emails format.
            var emailChecker = new EmailAddressAttribute();
            bool isValid = emailChecker.IsValid(email);
            if (!isValid)
            {
                throw new ArgumentException("Email format is not valid");
            }

            //adds time and date for easier sorting in db

            string dateTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

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
                    Time = dateTime,
                    Message = message,
                },
                HttpResponse = response,
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error processing request");
            var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await errorResponse.WriteStringAsync($"Error: {e.Message}");
            return new MultiResponse() { Document = null!, HttpResponse = errorResponse };
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
        public required HttpResponseData HttpResponse { get; set; }
    }
}
