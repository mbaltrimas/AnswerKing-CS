using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Answer.King.Api.OpenApi;

public class ValidationProblemDetailsSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(Microsoft.AspNetCore.Mvc.ValidationProblemDetails))
        {
            return;
        }

        var serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var exampleValidationProblemDetails = new ValidationProblemDetails
        {
            Errors = new Dictionary<string, string[]>
                { { "LineItems", new[] { "The LineItems field is required." } } },
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "One or more validation errors occurred.",
            Status = 400,
            TraceId = "00-f40e09a437a87f4ebcd2f39b128bb8f3-4b2ad798ac046140-00"
        };

        var example = JsonSerializer.Serialize(
            exampleValidationProblemDetails,
            typeof(ValidationProblemDetails),
            serializerOptions);

        schema.Example = new OpenApiString(example);

        schema.Properties["type"].Description =
            @"A URI reference [RFC3986] that identifies the problem type. This specification encourages that, when
            dereferenced, it provide human-readable documentation for the problem type
            (e.g., using HTML [W3C.REC-html5-20141028]).  When this member is not present, its value is assumed to be
            'about:blank'.";
        schema.Properties["type"].Nullable = false;

        schema.Properties["title"].Description =
            @"A short, human-readable summary of the problem type.It SHOULD NOT change from occurrence to occurrence
            of the problem, except for purposes of localization(e.g., using proactive content negotiation;
            see[RFC7231], Section 3.4).";
        schema.Properties["title"].Nullable = false;

        schema.Properties["status"].Description =
            "The HTTP status code([RFC7231], Section 6) generated by the origin server for this occurrence of the problem.";
        schema.Properties["status"].Nullable = false;

        schema.Properties["errors"].Description = "The validation errors.";
        schema.Properties["errors"].Nullable = false;

        schema.Properties["detail"].Description =
            "A human-readable explanation specific to this occurrence of the problem.";
        schema.Properties["detail"].Nullable = true;

        schema.Properties["instance"].Description =
            "A URI reference that identifies the specific occurrence of the problem. It may or may not yield further information if dereferenced.";
        schema.Properties["instance"].Nullable = true;

        var traceId = new OpenApiSchema
        {
            Type = "string",
            Description = "A unique identifier to represent this request in trace logs.",
            Nullable = false
        };

        schema.Properties.Add("traceId", traceId);
    }
}

internal class ValidationProblemDetails
{
    public Dictionary<string, string[]> Errors { get; init; } = new();

    public string? Type { get; set; }

    public string? Title { get; set; }

    public int Status { get; set; }

    public string? TraceId { get; set; }
}