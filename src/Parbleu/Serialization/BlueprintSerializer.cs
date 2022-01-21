using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ionic.Zlib;
using Parbleu.Model;
using CompressionLevel = Ionic.Zlib.CompressionLevel;
using CompressionMode = System.IO.Compression.CompressionMode;
using DeflateStream = System.IO.Compression.DeflateStream;

namespace Parbleu.Serialization;

public static class BlueprintSerializer
{
    public static byte[] Base64DecodeBlueprintString(string encoded)
    {
        if (string.IsNullOrEmpty(encoded))
        {
            throw new ArgumentException(encoded);
        }

        if (!encoded.StartsWith("0"))
        {
            throw new FormatException("Blueprint string should start with '0'");
        }

        var bytes = Convert.FromBase64String(encoded.Substring(1));
        return bytes;
    }
    
    public static string Base64EncodeCompressed(byte[] compressed)
    {
        return "0" + Convert.ToBase64String(compressed);
    }
    
    /// <summary>
    /// Deflates the blueprint string
    /// </summary>
    /// <param name="encoded"></param>
    /// <returns>json</returns>
    public static string BlueprintStringToJson(string encoded)
    {
        var bytes = Base64DecodeBlueprintString(encoded).Skip(2).ToArray(); // Skip compression level (since it is explicitly specified as 9)
        using (var ms = new MemoryStream(bytes))
        {
            using (var stream = new DeflateStream(ms, CompressionMode.Decompress))
            using(var reader = new StreamReader(stream, encoding))
            {
                return reader.ReadToEnd();
            }
        }
    }

    public static Book BlueprintStringToBook(string encoded)
    {
        var json = BlueprintStringToJson(encoded);
        var wrapper = JsonSerializer.Deserialize<BookWrapper>(json);
        return wrapper?.blueprint_book;
    }
    
    public static Blueprint BlueprintStringToBlueprint(string encoded)
    {
        var json = BlueprintStringToJson(encoded);
        var wrapper = JsonSerializer.Deserialize<BlueprintWrapper>(json);
        return wrapper?.blueprint;
    }
    
    public static object BlueprintStringToObject(string encoded)
    {
        object result = BlueprintStringToBook(encoded);
        if (result == null)
        {
            result = BlueprintStringToBlueprint(encoded);
        }
        return result;
    }
    
    public static byte[] CompressJson(string json)
    {
        using (var ms = new MemoryStream())
        {
            using (var stream = new ZlibStream(ms, Ionic.Zlib.CompressionMode.Compress, CompressionLevel.Level9))
            using(var writer = new StreamWriter(stream, encoding))
            {
                writer.Write(json);
            }
            ms.Flush();
            var bytes = ms.ToArray();
            return bytes;
        }
    }
    
    public static string BookToJson(Book book)
    {
        var json = JsonSerializer.Serialize(new BookWrapper()
        {
            blueprint_book = book
        }, options);
        return json;
    }
    
    public static string BlueprintToJson(Blueprint blueprint)
    {
        var json = JsonSerializer.Serialize(new BlueprintWrapper()
        {
            blueprint = blueprint
        }, options);
        return json;
    }
    
    public static string ToJson(object obj)
    {
        if (obj is Blueprint blueprint)
        {
            return BlueprintToJson(blueprint);
        }
        
        if (obj is Book book)
        {
            return BookToJson(book);
        }
        
        return null;
    }
    
    public static byte[] Inflate(object obj)
    {
        var json = ToJson(obj);
        var bytes = CompressJson(json);
        return bytes;
    }
    
    public static string ToBlueprintString(object obj)
    {
        var json = ToJson(obj);
        var bytes = CompressJson(json);
        return Base64EncodeCompressed(bytes);
    }
    
    
    
    private static readonly JsonSerializerOptions options = new JsonSerializerOptions()
    {
        WriteIndented = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    private static readonly Encoding encoding = new UTF8Encoding(false);
}