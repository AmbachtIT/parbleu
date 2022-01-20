using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using NUnit.Framework;
using Parbleu.Serialization;
using Parbleu.Test.SampleData;

namespace Parbleu.Test;

[TestFixture()]
public class TestSerialization
{
    
    [Test(), TestCaseSource(nameof(AllSampleData))]
    public void Exists(KeyValuePair<string, string> book)
    {
        Assert.IsNotEmpty(book.Key);
        Assert.IsNotEmpty(book.Value);
    }
    
    [Test(), TestCaseSource(nameof(AllSampleData))]
    public void Base64Decode(KeyValuePair<string, string> book)
    {
        var bytes = BlueprintSerializer.Base64DecodeBlueprintString(book.Value);
        Assert.NotNull(bytes);
    }
    
    [Test(), TestCaseSource(nameof(AllSampleData))]
    public void Deflate(KeyValuePair<string, string> book)
    {
        var json = BlueprintSerializer.DecompressBlueprintString(book.Value);
        Assert.NotNull(json);
    }
    
    [Test(), TestCaseSource(nameof(AllSampleData))]
    public void IsValidJson(KeyValuePair<string, string> book)
    {
        var json = BlueprintSerializer.DecompressBlueprintString(book.Value);
        var deserialized = JsonSerializer.Deserialize<JsonObject>(json);
    }
    
        
    [Test(), TestCaseSource(nameof(AllBooks))]
    public void IsValidBook(KeyValuePair<string, string> book)
    {
        var deserialized = BlueprintSerializer.BlueprintStringToBook(book.Value);
        Assert.NotNull(deserialized);
        Assert.Greater(deserialized.blueprints.Count, 0);
    }
    
    
    
    [Test(), TestCaseSource(nameof(AllSampleData))]
    public void TestRoundtripJson(KeyValuePair<string, string> book)
    {
        var json = BlueprintSerializer.DecompressBlueprintString(book.Value);
        var deserialized = BlueprintSerializer.BlueprintStringToObject(book.Value);
        var jsonSerialized = BlueprintSerializer.ToJson(deserialized);
        Assert.AreEqual(json, jsonSerialized);
    }
    
    [Test(), TestCaseSource(nameof(AllSampleData))]
    public void TestRoundtripInflateDeflate(KeyValuePair<string, string> book)
    {
        var bytes = BlueprintSerializer.Base64DecodeBlueprintString(book.Value);
        var deserialized = BlueprintSerializer.BlueprintStringToObject(book.Value);
        var inflated = BlueprintSerializer.Inflate(deserialized);
        CollectionAssert.AreEqual(bytes, inflated);
    }
    
    [Test(), TestCaseSource(nameof(AllSampleData))]
    public void TestRoundtripBlueprintString(KeyValuePair<string, string> book)
    {
        var obj = BlueprintSerializer.BlueprintStringToObject(book.Value);
        var recoded = BlueprintSerializer.ToBlueprintString(obj);
        Assert.AreEqual(book.Value, recoded);
    }
    public static IEnumerable<KeyValuePair<string, string>> AllBooks()
    {
        return SampleBooks.AllBooks();
    }
    
    public static IEnumerable<KeyValuePair<string, string>> AllBlueprints()
    {
        return SampleBooks.AllBlueprints();
    }

    public static IEnumerable<KeyValuePair<string, string>> AllSampleData()
    {
        return SampleBooks.AllFiles("");
    }

    
}