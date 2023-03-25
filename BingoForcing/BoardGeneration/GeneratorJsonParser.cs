using Newtonsoft.Json;

namespace BingoForcing.BoardGeneration;

public static class GeneratorJsonParser
{
    public static Objective[][] ParseJsonFile(string fileName)
    {
        string text = File.ReadAllText(fileName);
        return JsonConvert.DeserializeObject<Objective[][]>(text) ?? Array.Empty<Objective[]>();
    }
}