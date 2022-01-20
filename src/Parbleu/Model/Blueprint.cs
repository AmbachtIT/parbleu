using System.Text.Json.Nodes;

namespace Parbleu.Model;

public class Blueprint
{
    public List<JsonObject> icons { get; set; }
    public List<JsonObject> entities { get; set; }
    public string item { get; set; }
    public string label { get; set; }
    public long version { get; set; }
}

public class BlueprintWrapper
{
    public Blueprint blueprint { get; set; }
}