namespace BingoForcing.BoardGeneration;

public static class StringRepresentationGenerator
{
    private const string ALPHABET = "abcdefghijklmn";
    
    public static string BoardToString(BoardObjective[] board)
    {
        char[] result = new char[25];

        foreach (BoardObjective o in board)
        {
            result[o.Tier] = ALPHABET[o.Id];
        }

        return new string(result);
    }
}