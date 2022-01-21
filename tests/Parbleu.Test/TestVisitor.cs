using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using NUnit.Framework;
using Parbleu.DynamicJson;
using Parbleu.Serialization;

namespace Parbleu.Test;

[TestFixture()]
public class TestVisitor
{

    [Test(), TestCaseSource(nameof(TestJsonValues))]
    public void Roundtrip(string json)
    {
        var visitor = new Visitor();
        var deserialized = JsonSerializer.Deserialize<JsonNode>(json);
        var result = visitor.Visit(deserialized);
        var serialized = Serialize(result);
        Assert.AreEqual(json, serialized);
    }

    public static IEnumerable<string> TestJsonValues()
    {
        yield return "{\"value\":\"v\"}";
        yield return "{\"value\":1}";
        yield return "1";
        yield return "\"str\"";
        yield return "\"1\"";

        foreach (var bp in SampleData.SampleBooks.AllFiles(""))
        {
            var json = BlueprintSerializer.BlueprintStringToJson(bp.Value);
            yield return json;
        }
    }

    private string Serialize(JsonNode node)
    {
        return JsonSerializer.Serialize(node, options);
    }
    
    private static readonly JsonSerializerOptions options = new JsonSerializerOptions()
    {
        WriteIndented = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
}