using System.Text.Json.Serialization;

namespace My.Cloud.Functions;

public class Visitor
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string VisitorDate { get; set; }
}
