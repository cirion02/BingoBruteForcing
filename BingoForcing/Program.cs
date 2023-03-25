// See https://aka.ms/new-console-template for more information

using BingoForcing.BoardGeneration;
using BingoForcing.GeneratorHelperFileGenerators;

LetterToObjectiveLookup.CreateLetterToObjectiveJson("lockout-3-1-2");

var temp = LetterToObjectiveLookup.ReadLetterToObjectiveJson("lockout-3-1-2");

int temp2 = 6;