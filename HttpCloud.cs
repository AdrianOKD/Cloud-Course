using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json.Nodes;
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
                _logger.LogWarning("Body was null");
                throw new ArgumentNullException("The body is null");
            }
            JsonNode? data = JsonNode.Parse(body);
            if (data == null)
            {
                _logger.LogWarning("Data was null");
                throw new ArgumentNullException("data is null");
            }
            //extracts and validate required fields in visitor.
            string? name = data?["name"]?.ToString();
            if (name == null)
            {
                _logger.LogWarning("Name was null");
                throw new ArgumentNullException("name is required");
            }

            string? email = data?["email"]?.ToString();

            if (email == null)
            {
                _logger.LogWarning("Email was null");
                throw new ArgumentNullException("email is required");
            }
            //Validate emails format.
            var emailChecker = new EmailAddressAttribute();
            bool isValid = emailChecker.IsValid(email);
            if (!isValid)
            {
                _logger.LogWarning("Email was not in valid format");
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
                    VisitorDate = dateTime,
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
