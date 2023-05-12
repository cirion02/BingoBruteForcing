using BingoForcing.Helpers;

namespace BingoForcing.DataGathering;

public static class ObjectiveFromListOnBoardCount
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
    
    private static HashSet<char>[] GetObjectiveLetterLookup(string filename, string objectiveListFilename)
    {
        Dictionary<string, char>[] objectiveLookup = LetterObjectiveLookup.ReadObjectiveToLetterJson(filename);

        HashSet<char>[] result = new HashSet<char>[25];

        for (int i = 0; i < 25; i++)
        {
            result[i] = new HashSet<char>();
        }
        
        using StreamReader sr = new StreamReader(@"Input/ObjectiveLists/" + objectiveListFilename + ".txt");
        
        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            (int i, char value) = LookForLetter(objectiveLookup, line.Trim(' '));
            result[i].Add(value);
        }

        return result;
    }
    
    public static int[] GetObjectiveFromListOnBoardCount(string filename, string objectiveListFilename)
    {
        HashSet<char>[] lookup = GetObjectiveLetterLookup(filename, objectiveListFilename);

        using StreamReader sr = new StreamReader(@"BoardsFiles/" + filename + ".txt");
        
        int[] result = new int[25];
        
        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            int temp = 0;
            for (int i = 0; i < 25; i++)
            {
                if (lookup[i].Contains(line[i])) temp++;
            }

            result[temp]++;
        }

        return result;
    }
    
    public static void WriteObjectiveFromListOnBoardCount(string filename, string objectiveListFilename)
    {
        int[] result = GetObjectiveFromListOnBoardCount(filename, objectiveListFilename);

        int highestValue = 0;

        for (int i = result.Length - 1; i >= 0; i--)
        {
            if (result[i] != 0)
            {
                highestValue = i;
                break;
            }
        }
        
        using StreamWriter sw = new StreamWriter(@"Output/ObjectiveFromListOnBoardCount/" + filename + "-" + objectiveListFilename + ".txt");
        
        sw.WriteLine("Objective Counts:");
        
        for (int i = 0; i <= highestValue; i++)
        {
            sw.WriteLine($"{i}: {result[i]}");
        }
    }
}