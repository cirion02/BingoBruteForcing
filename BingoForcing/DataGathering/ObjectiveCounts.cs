using BingoForcing.BoardGeneration;

namespace BingoForcing.DataGathering;

public static class ObjectiveCounts
{
    //TODO: See if replacing this with ToInt32 is faster
    private static Dictionary<char, int> _alphabetLookup = new Dictionary<char, int>(){
        {'a', 0},
        {'b', 1},
        {'c', 2},
        {'d', 3},
        {'e', 4},
        {'f', 5},
        {'g', 6},
        {'h', 7},
        {'i', 8},
        {'j', 9},
        {'k', 10},
        {'l', 11},
        {'m', 12},
        {'n', 13},
    };
    
    public static int[][] GetObjectiveCounts(string filename)
    {
        int[][] counts = new int[25][];

        for (int i = 0; i < 25; i++)
        {
            counts[i] = new int[14];
        }

        using StreamReader sr = new StreamReader(@"BoardsFiles/" + filename + ".txt");
        
        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            for (int i = 0; i < 25; i++)
            {
                counts[i][_alphabetLookup[line[i]]]++;
            }
        }

        return counts;
    }

    public static void WriteObjectiveCounts(string filename)
    {
        int[][] counts = GetObjectiveCounts(filename);

        Objective[][] objectives = GeneratorJsonParser.ParseJsonFile(filename);
        
        using StreamWriter sw = new StreamWriter(@"Output/ObjectiveCounts/" + filename + ".txt");
        
        for (int i = 0; i < 25; i++)
        {
            sw.WriteLine($"Tier {i+1}:");
            for (int j = 0; j < objectives[i].Length; j++)
            {
                sw.WriteLine($"{objectives[i][j].Name}: {counts[i][j]}");
            }
            sw.WriteLine();
        }
    }
}