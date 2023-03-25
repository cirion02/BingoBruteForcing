namespace BingoForcing.BoardGeneration;

public static class BoardsFileGenerator
{
    public static void WriteBoardsToFile(string fileName, int count, int min = 0)
    {
        Objective[][] generatorJson = GeneratorJsonParser.ParseJsonFile(@"../../../GeneratorJsons/" + fileName + ".json");
        BoardGenerator generator = new BoardGenerator(generatorJson);
        
        using (StreamWriter sw = new StreamWriter(@"../../../BoardsFiles/" + fileName + ".txt"))
        {
            for (int i = min; i < count; i++)
            {
                sw.WriteLine(StringRepresentationGenerator.BoardToString(generator.GenerateBoard(i)));
            }
        }
    }
}