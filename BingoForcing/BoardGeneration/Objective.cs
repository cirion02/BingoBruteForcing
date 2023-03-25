namespace BingoForcing.BoardGeneration;

public struct Objective
{
    public string Name { get; set; }
    public string[] Types { get; set; }
}

public struct BoardObjective
{
    public static BoardObjective FromObjective(Objective objective)
    {
        BoardObjective boardObjective = new();
        boardObjective.Filled = true;
        boardObjective.Name = objective.Name;
        boardObjective.Types = objective.Types;

        return boardObjective;
    }

    public void AddBoardInfo(int id, int synergy)
    {
        Id = id;
        Synergy = synergy;
    }
    
    public bool Filled { get; set; }
    public string Name { get; set; }
    public string[] Types { get; set; }
    public int Id { get; set; }
    public int Synergy { get; set; }
}