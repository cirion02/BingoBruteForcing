using System.Diagnostics.CodeAnalysis;
using BingoForcing.BoardGeneration;
using BingoForcing.DataGathering;

namespace BingoForcing.UI;

public static class UI
{
    private static string? _currentFilename;
    
    public static void Main()
    {
        Console.WriteLine("Bingo Brute Force v0.2 by Cirion02\n");

        bool done = false;

        while (!done)
        {
            done = AwaitCommand();
        }
    }
        
    [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
    private static bool AwaitCommand()
    {
        Console.WriteLine("\nAwaiting command, type help for list of commands, type quit to exit program");

        if (_currentFilename == null)
        {
            Console.WriteLine("WARNING: No filename is currently selected, most commands will not work without them. Set one using setFileName");
        }
        
        Console.Write("> ");
        string input = (Console.ReadLine() ?? "");
        string[] inputs = input.Split(" ");
        string command = inputs[0].ToLower();
        string arg = inputs.Length > 1 ? inputs[1] : "";

        switch (command)
        {
            case "help":
                Help();
                return false;
            case "quit":
                Quit();
                return true;
            case "setfilename":
                SetFileName(arg);
                return false;
            case "genboards": 
                GenBoards();
                return false;
            case "objectivecounts":
                GenObjectiveCounts();
                return false;
            case "generatorhelp":
                GeneratorHelp();
                return false;
            case "objectivesfromlist":
                GenObjectivesFromList();
                return false;
            case "objectivesperchapter":
                GenObjectivePerChapter();
                return false;
            case "routedominance":
                GenRouteDominanceData();
                return false;
            case "findboards":
                FindBoardFromObjectives();
                return false;
            case "filteredcounts":
                GenFilteredCounts();
                return false;
            default:
                Console.WriteLine("Unknown command, type help for list of commands.");
                return false;
        }
    }

    private static void Help()
    {
        Console.WriteLine("Help: Shows this list");
        Console.WriteLine("GeneratorHelp: Explains how to add your own generators to the program");
        Console.WriteLine("Quit: Quits the application");
        Console.WriteLine("SetFileName: Sets the file name of the files that will be read and written too,\n" +
                          "changing this between commands could have unintended consequences");
        Console.WriteLine("GenBoards: Generates the list of boards with the current gen");
        Console.WriteLine("ObjectiveCounts: Writes how many times each objective shows up into the output folder");
        Console.WriteLine("ObjectivesFromList: Takes a lot of objective names, generates a list of how many boards\n" +
                          "have a specific amount of those objectives.");
        Console.WriteLine("ObjectivesPerChapter: Makes a list of how many times each pair of chapters shows up on boards.\n" +
                          "Also calculates what the highest two chapter synergy on each board.");
        Console.WriteLine("RouteDominance: Generates data on how often a given route is dominant over others.");
        Console.WriteLine("FindBoards: Finds the board containing a specific set of objectives.");
        Console.WriteLine("FilteredCounts: Checks how often a list of objectives appears alongside a specific objective.");
    }
    
    private static void Quit()
    {
        Console.WriteLine("Exiting...");
    }
    
    private static void SetFileName(string arg)
    {
        string filename;
        if (arg == "")
        {
            Console.WriteLine("Enter filename (this is the base filename of your json file, so lockout1.json would be 'lockout1'");
            Console.Write("> ");
            filename = (Console.ReadLine() ?? "").ToLower();
        }
        else
        {
            filename = arg;
        }
        if (!File.Exists(@"GeneratorJsons/" + filename + ".json"))
        {
            Console.WriteLine($"No generator was found at GeneratorJsons/{filename}\nPlease try again");
        }
        else
        {
            Console.WriteLine($"Succesfully set generator filename to {filename}");
            _currentFilename = filename;
        }
    }

    private static void GenBoards()
    {
        if (_currentFilename == null)
        {
            Console.WriteLine("No filename is currently selected, set use using SetFileName and then try again.");
            return;
        }
        if (File.Exists(@"BoardsFiles/" + _currentFilename + ".txt"))
        {
            Console.WriteLine($"Boards for {_currentFilename} have already been generated before, are you sure you want to do it again. Type Y to continue.");
            Console.Write("> ");
            string input = (Console.ReadLine() ?? "").ToLower();
            if (input != "y" && input != "yes")
            {
                Console.WriteLine("Boardfile generation cancelled.");
                return;
            }
            Console.WriteLine("Continuing");
        }
        BoardsFileGenerator.WriteBoardsToFile(_currentFilename, 1000000);
    }
    
    private static void GenObjectiveCounts()
    {
        if (_currentFilename == null)
        {
            Console.WriteLine("No filename is currently selected, set use using SetFileName and then try again.");
            return;
        }
        if (!File.Exists(@"BoardsFiles/" + _currentFilename + ".txt"))
        {
            Console.WriteLine($"Boards for {_currentFilename} don't exist, create them and try again.");
            return;
        }
        if (!File.Exists(@"GeneratorJsons/" + _currentFilename + ".json"))
        {
            Console.WriteLine($"Generator for {_currentFilename} don't exist, create it and try again.");
            return;
        }
        ObjectiveCounts.WriteObjectiveCounts(_currentFilename);
    }
    
    private static void GenObjectivesFromList()
    {
        if (_currentFilename == null)
        {
            Console.WriteLine("No filename is currently selected, set use using SetFileName and then try again.");
            return;
        }
        if (!File.Exists(@"BoardsFiles/" + _currentFilename + ".txt"))
        {
            Console.WriteLine($"Boards for {_currentFilename} don't exist, create them and try again.");
            return;
        }
        if (!File.Exists(@"GeneratorJsons/" + _currentFilename + ".json"))
        {
            Console.WriteLine($"Generator for {_currentFilename} don't exist, create it and try again.");
            return;
        }
        Console.WriteLine($"What's the filename for the list of objective names");
        Console.Write("> ");
        string input = (Console.ReadLine() ?? "").ToLower();
        if (!File.Exists(@"Input/ObjectiveLists/" + input + ".txt"))
        {
            Console.WriteLine($"No file named {input} found in Input/ObjectiveLists, try again");
            return;
        }
        ObjectiveFromListOnBoardCount.WriteObjectiveFromListOnBoardCount(_currentFilename, input);
    }
    
    private static void FindBoardFromObjectives()
    {
        if (_currentFilename == null)
        {
            Console.WriteLine("No filename is currently selected, set use using SetFileName and then try again.");
            return;
        }
        if (!File.Exists(@"BoardsFiles/" + _currentFilename + ".txt"))
        {
            Console.WriteLine($"Boards for {_currentFilename} don't exist, create them and try again.");
            return;
        }
        if (!File.Exists(@"GeneratorJsons/" + _currentFilename + ".json"))
        {
            Console.WriteLine($"Generator for {_currentFilename} don't exist, create it and try again.");
            return;
        }
        Console.WriteLine($"What's the filename for the list of objective names");
        Console.Write("> ");
        string input = (Console.ReadLine() ?? "").ToLower();
        if (!File.Exists(@"Input/ObjectivesFromBoard/" + input + ".txt"))
        {
            Console.WriteLine($"No file named {input} found in Input/ObjectivesFromBoard, try again");
            return;
        }
        FindBoardsFromObjectives.WriteBoardsFromObjectives(_currentFilename, input);
    }
    
    private static void GenObjectivePerChapter()
    {
        if (_currentFilename == null)
        {
            Console.WriteLine("No filename is currently selected, set use using SetFileName and then try again.");
            return;
        }
        if (!File.Exists(@"BoardsFiles/" + _currentFilename + ".txt"))
        {
            Console.WriteLine($"Boards for {_currentFilename} don't exist, create them and try again.");
            return;
        }
        if (!File.Exists(@"GeneratorJsons/" + _currentFilename + ".json"))
        {
            Console.WriteLine($"Generator for {_currentFilename} don't exist, create it and try again.");
            return;
        }
        ChapterPairObjectiveCounts.WriteObjectiveFromListOnBoardCount(_currentFilename);
    }

    private static void GeneratorHelp()
    {
        Console.WriteLine("To add your own generator, put the json file into the GeneratorJsons folder.\n" +
                          "From there you can read it in this terminal by using SetFileName.");
    }
    
    private static void GenRouteDominanceData()
    {
        if (_currentFilename == null)
        {
            Console.WriteLine("No filename is currently selected, set use using SetFileName and then try again.");
            return;
        }
        if (!File.Exists(@"BoardsFiles/" + _currentFilename + ".txt"))
        {
            Console.WriteLine($"Boards for {_currentFilename} don't exist, create them and try again.");
            return;
        }
        if (!File.Exists(@"GeneratorJsons/" + _currentFilename + ".json"))
        {
            Console.WriteLine($"Generator for {_currentFilename} don't exist, create it and try again.");
            return;
        }
        Console.WriteLine($"What's the filename for the routes");
        Console.Write("> ");
        string input = (Console.ReadLine() ?? "").ToLower();
        if (!File.Exists(@"Input/Routes/" + input + ".txt"))
        {
            Console.WriteLine($"No file named {input} found in Input/Routes, try again");
            return;
        }
        RouteDominanceCalculations.WriteObjectiveCounts(_currentFilename, input);
    }
    
    private static void GenFilteredCounts()
    {
        if (_currentFilename == null)
        {
            Console.WriteLine("No filename is currently selected, set use using SetFileName and then try again.");
            return;
        }
        if (!File.Exists(@"BoardsFiles/" + _currentFilename + ".txt"))
        {
            Console.WriteLine($"Boards for {_currentFilename} don't exist, create them and try again.");
            return;
        }
        if (!File.Exists(@"GeneratorJsons/" + _currentFilename + ".json"))
        {
            Console.WriteLine($"Generator for {_currentFilename} don't exist, create it and try again.");
            return;
        }
        Console.WriteLine($"What's the filename for the routes");
        Console.Write("> ");
        string input = (Console.ReadLine() ?? "").ToLower();
        if (!File.Exists(@"Input/FilteredCounts/" + input + ".txt"))
        {
            Console.WriteLine($"No file named {input} found in Input/FilteredCounts, try again");
            return;
        }
        FilteredCounts.WriteFilteredCounts(_currentFilename, input);
    }
}