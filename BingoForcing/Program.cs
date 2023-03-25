// See https://aka.ms/new-console-template for more information

using BingoForcing.BoardGeneration;
/*
SeedRandom seedRandom = new SeedRandom("0");
Console.WriteLine(seedRandom.Random());
Console.WriteLine(seedRandom.Random());
Console.WriteLine(seedRandom.Random());
Console.WriteLine();
seedRandom = new SeedRandom("1234");
Console.WriteLine(seedRandom.Random());
Console.WriteLine(seedRandom.Random());
Console.WriteLine(seedRandom.Random());
Console.WriteLine();*/
SeedRandom seedRandom = new SeedRandom("12345");
for (int i = 0; i < 25; i++)
{
    Console.WriteLine(seedRandom.Random());
}
