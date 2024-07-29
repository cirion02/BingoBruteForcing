using BingoForcing.Helpers;

namespace BingoForcing.DataGathering;

public static class RouteDominanceCalculations
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
    
    private static (HashSet<char>[,], string[]) GetObjectiveLetterLookup(string filename, string routeDataFilename)
    {
        Dictionary<string, char>[] objectiveLookup = LetterObjectiveLookup.ReadObjectiveToLetterJson(filename);
        
        using StreamReader sr = new StreamReader(@"Input/Routes/" + routeDataFilename + ".txt");

        List<string> routeNames = new();
        
        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            if (line == "") break;
            routeNames.Add(line);
        }

        HashSet<char>[,] result = new HashSet<char>[25, routeNames.Count*2];

        for (int i = 0; i < routeNames.Count*2; i++)
        {
            for (int j = 0; j < 25; j++)
            {
                result[j,i] = new HashSet<char>();
            }
        }

        int routeCounter = 0;
        
        while ((line = sr.ReadLine()) != null)
        {
            if (line == "")
            {
                routeCounter++;
                continue;
            }
            (int i, char value) = LookForLetter(objectiveLookup, line.Trim(' '));
            result[i, routeCounter].Add(value);
        }

        return (result, routeNames.ToArray());
    }

    private static (int[], int[]) GetBoardCounts(HashSet<char>[,] lookup, string board, int routes)
    {
        int[] hardCounts = new int[routes];
        int[] softCounts = new int[routes];
        for (int i = 0; i < 25; i++)
        {
            for (int j = 0; j < routes; j++)
            {
                if (lookup[i, j*2].Contains(board[i])) hardCounts[j]++;
                if (lookup[i, j*2+1].Contains(board[i])) softCounts[j]++;
            }
        }

        return (hardCounts, softCounts);
    }
    
    public static (int[,,,], int, string[]) GetRouteData(string filename, string routeDataFilename)
    {
        (HashSet<char>[,] lookup, string[] routeNames) = GetObjectiveLetterLookup(filename, routeDataFilename);

        int routes = routeNames.Length;

        int[,,,] result = new int[routes, routes, 25, 51];

        int closeBoards = 0;
        
        using StreamReader sr = new StreamReader(@"BoardsFiles/" + filename + ".txt");
        
        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            (int[] hardCounts, int[] softCounts) = GetBoardCounts(lookup, line, routes);
            int highestScore = -1;
            int secondHighestScore = -1;
            int highest = -1;
            int secondHighest = -1;

            for (int i = 0; i < routes; i++)
            {
                if (hardCounts[i] > highestScore || (hardCounts[i] == highestScore && softCounts[i] >= softCounts[highest]))
                {
                    secondHighestScore = highestScore;
                    secondHighest = highest;
                    highestScore = hardCounts[i];
                    highest = i;
                }
                else if (hardCounts[i] > secondHighestScore || (hardCounts[i] == secondHighestScore && softCounts[i] >= softCounts[secondHighest]))
                {
                    secondHighestScore = hardCounts[i];
                    secondHighest = i;
                }
            }

            if (highestScore <= secondHighestScore + 1)
            {
                closeBoards++;
                continue;
            }
            
            //Console.WriteLine($"{highest},{secondHighest}, {highest - secondHighest}, {25 + softCounts[highest] - softCounts[secondHighest]}");
            result[highest, secondHighest, highestScore - secondHighestScore,
                25 + softCounts[highest] - softCounts[secondHighest]]++;
        }

        return (result, closeBoards, routeNames);
    }
    
    public static void WriteObjectiveCounts(string filename, string routeDataFilename)
    {
        (int[,,,] results, int closeBoards, string[] routeNames) = GetRouteData(filename, routeDataFilename);
        
        Console.WriteLine("hi");

        using StreamWriter sw = new StreamWriter(@"Output/Routes/" + filename + "-" + routeDataFilename + ".txt");
        
        sw.WriteLine($"Close boards: {closeBoards}  =  {(float)closeBoards/10000}%");

        int routeCount = routeNames.Length;

        for (int route = 0; route < routeCount; route++)
        {
            sw.WriteLine($"Boards with strongest route being {routeNames[route]}:\n");
            int[] totals = new int[routeCount];
            for (int secondRoute = 0; secondRoute < routeCount; secondRoute++)
            {
                for (int i = 0; i < 25; i++)
                {
                    for (int j = 0; j < 51; j++)
                    {
                        totals[secondRoute] += results[route, secondRoute, i, j];
                    }
                }
            }
            sw.WriteLine($"Total: {totals.Sum()}  =  {(float)totals.Sum()/10000}%");
            sw.WriteLine("The second best route is:");

            for (int secondRoute = 0; secondRoute < routeCount; secondRoute++)
            {
                if (route == secondRoute) continue;
                sw.WriteLine($"  - {routeNames[secondRoute]} : {totals[secondRoute]}  =  {(float)totals[secondRoute]/10000}%");
            }
            int[] dominanceAmount = new int[25];
            for (int secondRoute = 0; secondRoute < routeCount; secondRoute++)
            {
                for (int i = 0; i < 25; i++)
                {
                    for (int j = 0; j < 51; j++)
                    {
                        dominanceAmount[i] += results[route, secondRoute, i, j];
                    }
                }
            }
            
            sw.WriteLine("\nThe amount it dominates by is is:");
            for (int dominance = 0; dominance < 25; dominance++)
            {
                if (dominanceAmount[dominance] == 0) continue;
                sw.WriteLine($"  - {dominance} : {dominanceAmount[dominance]}  =  {(float)dominanceAmount[dominance]/10000}%");
            }

            sw.WriteLine("\n\n");
        }
    }
}
