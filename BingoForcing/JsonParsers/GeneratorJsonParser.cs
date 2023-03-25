using Newtonsoft.Json;

namespace BingoForcing.BoardGeneration;

public static class GeneratorJsonParser
{
    public static Objective[][] ParseJsonFile(string filename)
    {
        string text = File.ReadAllText(@"GeneratorJsons/" + filename + ".json");
        return JsonConvert.DeserializeObject<Objective[][]>(text) ?? Array.Empty<Objective[]>();
    }
}