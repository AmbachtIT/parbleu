using System.Text.Json.Nodes;

namespace Parbleu.Model;

public class Book
{
    public List<JsonObject> blueprints { get; set; }

    public string item { get; set; }

    public string label { get; set; }

    public JsonObject label_color { get; set; }

    public int active_index { get; set; }

    public long version { get; set; }
}

public class BookWrapper
{
    public Book blueprint_book { get; set; }
}