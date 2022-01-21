using System.Text.Json.Nodes;

namespace Parbleu.DynamicJson;

public class Visitor
{
    public JsonNode Visit(JsonNode node) => node switch
    {
        JsonObject obj => VisitObject(obj),
        JsonArray arr => VisitArray(arr),
        JsonValue value => VisitValue(value),
        _ => throw new InvalidOperationException()
    };

    protected virtual JsonNode VisitObject(JsonObject obj)
    {
        var result = new JsonObject();
        foreach (var kv in obj)
        {
            result.Add(kv.Key, Visit(kv.Value));
        }

        return result;
    }
    
    protected virtual  JsonNode VisitArray(JsonArray array)
    {
        var result = new JsonArray();
        foreach (var node in array)
        {
            result.Add(Visit(node));
        }
        return result;
    }
    
    protected virtual JsonNode VisitValue(JsonValue value)
    {
        if (value.TryGetValue(out string str))
        {
            return JsonValue.Create(str);
        }
        if (value.TryGetValue(out decimal dbl))
        {
            return JsonValue.Create(dbl);
        }
        if (value.TryGetValue(out int intValue))
        {
            return JsonValue.Create(intValue);
        }
        if (value.TryGetValue(out DateTime date))
        {
            return JsonValue.Create(date);
        }
        if (value.TryGetValue(out bool boolValue))
        {
            return JsonValue.Create(boolValue);
        }

        throw new InvalidOperationException();
    }

    
    
}