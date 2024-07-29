using BingoForcing.Helpers;

namespace BingoForcing.DataGathering;

public class FindBoardsFromObjectives
{
    private static (int, char) LookForLetter(Dictionary<string, char>[] objectiveLookup, string objectiveName)
    {
        for (int i = 0; i < 25; i++)
        {
            if (objectiveLookup[i].TryGetValue(objectiveName, out char value))
            {
                return (i, value);
            }
        }
        throw new ArgumentException($"Objective {objectiveName} does not appear in the given generator.");
    }
    
    private static (HashSet<char>[], int) GetObjectiveLetterLookupAndCount(string filename, string objectiveListFilename)
    {
        Dictionary<string, char>[] objectiveLookup = LetterObjectiveLookup.ReadObjectiveToLetterJson(filename);
        
        HashSet<char>[] result = new HashSet<char>[25];

        for (int i = 0; i < 25; i++)
        {
            result[i] = new HashSet<char>();
        }
        
        using StreamReader sr = new StreamReader(@"Input/ObjectivesFromBoard/" + objectiveListFilename + ".txt");

        int count = 0;
        
        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            count++;
            (int i, char value) = LookForLetter(objectiveLookup, line.Trim(' '));
            result[i].Add(value);
        }

        return (result, count);
    }
    
    public static List<int> GetBoardsFromObjectives(string filename, string objectiveListFilename)
    {
        (HashSet<char>[] lookup, int count) = GetObjectiveLetterLookupAndCount(filename, objectiveListFilename);
        
        Console.WriteLine(count);

        using StreamReader sr = new StreamReader(@"BoardsFiles/" + filename + ".txt");
        
        List<int> result = new List<int>();

        int seed = 0;
        
        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            int temp = 0;
            for (int i = 0; i < 25; i++)
            {
                if (lookup[i].Contains(line[i])) temp++;
            }

            if (temp == count)
            {
                result.Add(seed);
            }

            seed++;
        }

        return result;
    }
    
    public static void WriteBoardsFromObjectives(string filename, string objectiveListFilename)
    {
        List<int> result = GetBoardsFromObjectives(filename, objectiveListFilename);

        using StreamWriter sw = new StreamWriter(@"Output/BoardFromObjectives/" + filename + "-" + objectiveListFilename + ".txt");
        
        sw.WriteLine("Possible Seeds:");
        
        for (int i = 0; i < result.Count; i++)
        {
            sw.WriteLine($"{result[i]}");
        }
    }
}