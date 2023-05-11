using BingoForcing.BoardGeneration;
using Newtonsoft.Json;

namespace BingoForcing.Helpers;

public static class LetterObjectiveLookup
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
    
    public static void CreateObjectiveToLetterJson(string filename)
    {
        Objective[][] generator = GeneratorJsonParser.ParseJsonFile(filename);

        Dictionary<string, char>[] result = new Dictionary<string, char>[25];

        for (int i = 0; i < 25; i++)
        {
            result[i] = new Dictionary<string, char>();
            for (int j = 0; j < generator[i].Length; j++)
            {
                result[i].Add(generator[i][j].Name, ALPHABET[j]);
            }
        }

        string json = JsonConvert.SerializeObject(result);
        
        File.WriteAllText(@"GeneratorLookups/" + filename + "-reverse.json", json);
    }
    
    public static Dictionary<string, char>[] ReadObjectiveToLetterJson(string filename)
    {
        if (!File.Exists(@"GeneratorLookups/" + filename + "-reverse.json"))
        {
            CreateObjectiveToLetterJson(filename);
        }
        
        string text = File.ReadAllText(@"GeneratorLookups/" + filename + "-reverse.json");
        return JsonConvert.DeserializeObject<Dictionary<string, char>[]>(text) ?? Array.Empty<Dictionary<string, char>>();
    }
}