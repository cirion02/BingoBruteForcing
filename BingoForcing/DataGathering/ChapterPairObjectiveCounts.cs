using BingoForcing.Helpers;

namespace BingoForcing.DataGathering;

public static class ChapterPairObjectiveCounts
{
    private static (Dictionary<(char, int), int>, List<string>) CreateLetterLookup(string filename)
    {
        using StreamReader sr = new StreamReader(@"Input/ObjectivesPerChapter/" + filename + ".txt");

        Dictionary<string, char>[] objectiveLookup = LetterObjectiveLookup.ReadObjectiveToLetterJson(filename);
        
        List<string> chapterNames = new();
        Dictionary<(char, int), int> chapterLookup = new();
        
        string? line;
        bool chapterNameNext = true;
        int chapterNumber = 0;
        while ((line = sr.ReadLine()) != null)
        {
            if (chapterNameNext)
            {
                chapterNames.Add(line);
                chapterNameNext = false;
                continue;
            }

            if (line == "")
            {
                chapterNameNext = true;
                chapterNumber++;
                continue;
            }

            bool objectiveFound = false;

            for (int i = 0; i < 25; i++)
            {
                if (objectiveLookup[i].TryGetValue(line, out char value))
                {
                    chapterLookup.Add((value,i), chapterNumber);
                    objectiveFound = true;
                    break;
                }
            }

            if (!objectiveFound) throw new ArgumentException($"Objective {line} was not found in the generator.");
        }

        return (chapterLookup, chapterNames);
    }

    public static (int[], int[,,], List<string>) GetObjectivePairCounts(string filename)
    {
        (Dictionary<(char, int), int> chapterLookup, List<string> chapterNames) = CreateLetterLookup(filename);

        int[] result = new int[25];

        int chapterCount = chapterNames.Count;

        int[,,] resultPerChapterPair = new int[chapterCount, chapterCount, 25];
        
        using StreamReader sr = new StreamReader(@"BoardsFiles/" + filename + ".txt");

        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            int[] chapterAmounts = new int[chapterCount];
            
            for (int i = 0; i < 25; i++)
            {
                if (chapterLookup.TryGetValue((line[i], i), out int value))
                {
                    chapterAmounts[value]++;
                }
            }

            int maxValue = 0;

            for (int i = 0; i < chapterCount; i++)
            {
                for (int j = i + 1; j < chapterCount; j++)
                {
                    int value = chapterAmounts[i] + chapterAmounts[j];

                    resultPerChapterPair[i, j, value]++;

                    if (maxValue < value) maxValue = value;
                }
            }

            result[maxValue]++;
        }

        return (result, resultPerChapterPair, chapterNames);
    }

    public static void WriteObjectiveFromListOnBoardCount(string filename)
    {
        (int[] result, int[,,] resultPerChapterPair, List<string> chapterNames) = GetObjectivePairCounts(filename);
        
        using StreamWriter sw = new StreamWriter(@"Output/ObjectivesPerChapter/" + filename + ".txt");
        
        // For just printing totals without chapters
        
        sw.WriteLine("Max objectives in two chapters:");
        
        int highestValue = 0;

        for (int i = result.Length - 1; i >= 0; i--)
        {
            if (result[i] != 0)
            {
                highestValue = i;
                break;
            }
        }
        
        for (int i = 0; i <= highestValue; i++)
        {
            sw.WriteLine($"{i}: {result[i]}");
        }
        
        sw.WriteLine("\n");
        
        // For printing per chapter pair

        int chapterCount = chapterNames.Count;

        for (int i = 0; i < chapterCount; i++)
        {
            for (int j = i + 1; j < chapterCount; j++)
            {
                sw.WriteLine($"{chapterNames[i]} + {chapterNames[j]}");
                
                highestValue = 0;

                for (int k = 24; k >= 0; k--)
                {
                    if (resultPerChapterPair[i,j,k] != 0)
                    {
                        highestValue = k;
                        break;
                    }
                }
        
                for (int k = 0; k <= highestValue; k++)
                {
                    sw.WriteLine($"{k}: {resultPerChapterPair[i,j,k]}");
                }
                
                sw.WriteLine();
            }
        }

    }
    
}