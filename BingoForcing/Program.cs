// See https://aka.ms/new-console-template for more information

using BingoForcing.BoardGeneration;

Objective[][] generatorJson = GeneratorJsonParser.ParseJsonFile(@"../../../GeneratorJsons/lockout-3-1-2.json");

BoardGenerator generator = new BoardGenerator(generatorJson);

for (int i = 0; i < 1000000; i++)
{
    BoardObjective[] temp = generator.GenerateBoard(i);
} 