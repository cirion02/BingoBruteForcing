using BingoForcing.BoardGeneration;
using Newtonsoft.Json;

namespace BingoForcing.GeneratorHelperFileGenerators;

public static class LetterToObjectiveLookup
{
    private const string ALPHABET = "abcdefghijklmn";
    
    public static void CreateLetterToObjectiveJson(string filename)
    {
        Objective[][] generator = GeneratorJsonParser.ParseJsonFile(@"GeneratorJsons/" + filename + ".json");

        Dictionary<char, string>[] result = new Dictionary<char, string>[25];

        for (int i = 0; i < 25; i++)
        {
            result[i] = new Dictionary<char, string>();
            for (int j = 0; j < generator[i].Length; j++)
            {
                result[i].Add(ALPHABET[j], generator[i][j].Name);
            }
        }

        string json = JsonConvert.SerializeObject(result);
        
        File.WriteAllText(@"GeneratorLookups/" + filename + ".json", json);
    }
    
    public static Dictionary<char, string>[] ReadLetterToObjectiveJson(string filename)
    {
        string text = File.ReadAllText(@"GeneratorLookups/" + filename + ".json");
        return JsonConvert.DeserializeObject<Dictionary<char, string>[]>(text) ?? Array.Empty<Dictionary<char, string>>();
    }
}