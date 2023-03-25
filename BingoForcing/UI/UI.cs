using BingoForcing.BoardGeneration;

namespace BingoForcing.UI;

public static class UI
{
    private static string? _currentFilename;
    
    public static void Main()
    {
        Console.WriteLine("Bingo Brute Force v1.idk by Cirion02\n");

        bool done = false;

        while (!done)
        {
            done = AwaitCommand();
        }
    }

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
            default:
                Console.WriteLine("Unknown command, type help for list of commands.");
                return false;
        }
    }

    private static void Help()
    {
        Console.WriteLine("Help: Shows this list");
        Console.WriteLine("Quit: Quits the application");
        Console.WriteLine("SetFileName: Sets the file name of the files that will be read and written too,\n" +
                          "changing this between commands could have unintended consequences.");
        Console.WriteLine("GenBoards: Generates the list of boards with the current gen (This takes long)");
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
        if (!File.Exists(@"../../../GeneratorJsons/" + filename + ".json"))
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
        if (File.Exists(@"../../../BoardsFiles/" + _currentFilename + ".txt"))
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
}