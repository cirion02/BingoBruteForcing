namespace BingoForcing.BoardGeneration;

public class BoardGenerator
{
    private Objective[,] _generator;

    public BoardGenerator(Objective[,] generator)
    {
        _generator = generator;
    }
}