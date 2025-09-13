using System.Text.Json.Serialization;

namespace My.Cloud.Functions;

public class Visitor
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Message { get; set; }
}
