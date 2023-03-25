using System.Diagnostics.CodeAnalysis;

namespace BingoForcing.BoardGeneration;

public static class BoardsFileGenerator
{
    public static void WriteBoardsToFile(string filename, int count, int min = 0)
    {
        Objective[][] generatorJson = GeneratorJsonParser.ParseJsonFile(filename);
        BoardGenerator generator = new BoardGenerator(generatorJson);

        using StreamWriter sw = new StreamWriter(@"BoardsFiles/" + filename + ".txt");
        
        for (int i = min; i < count + min; i++)
        {
            sw.WriteLine(StringRepresentationGenerator.BoardToString(generator.GenerateBoard(i)));
        }
    }
}