using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Parbleu.Test.SampleData;

public static class SampleBooks
{
    public static IEnumerable<KeyValuePair<string, string>> AllBlueprints()
    {
        return AllFiles(".blueprint");
    }
    public static IEnumerable<KeyValuePair<string, string>> AllBooks()
    {
        return AllFiles(".book");
    }
    
    public static IEnumerable<KeyValuePair<string, string>> AllFiles(string extension)
    {
        var type = typeof(SampleBooks);
        var assembly = type.Assembly;
        var ns = typeof(SampleBooks).Namespace + ".";
        foreach(var name in 
                assembly
                    .GetManifestResourceNames()
                    .Where(n => n.StartsWith(ns))
                    .Where(n => n.EndsWith(extension)))
        {
            using (var reader = new StreamReader(assembly.GetManifestResourceStream(name)))
            {
                var value = reader.ReadToEnd();
                yield return new KeyValuePair<string, string>(name.Replace(ns, ""), value);
            }
        }
    }

    
}