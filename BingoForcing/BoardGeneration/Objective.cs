namespace BingoForcing.BoardGeneration;

public struct Objective
{
    public string Name { get; set; }
    public string[] Types { get; set; }
}

public struct IdObjective
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string[] Types { get; set; }
    public static IdObjective FromObjective(Objective objective, int id)
    {
        IdObjective idObjective = new();
        idObjective.Id = id;
        idObjective.Name = objective.Name;
        idObjective.Types = objective.Types;
        return idObjective;
    }
}

public struct BoardObjective
{
    public static BoardObjective FromIdObjective(IdObjective objective)
    {
        BoardObjective boardObjective = new();
        boardObjective.Filled = true;
        boardObjective.Id = objective.Id;
        boardObjective.Name = objective.Name;
        boardObjective.Types = objective.Types;

        return boardObjective;
    }

    public void AddBoardInfo(int synergy, int tier)
    {
        Synergy = synergy;
        Tier = tier;
    }
    
    public bool Filled { get; set; }
    public string Name { get; set; }
    public string[] Types { get; set; }
    public int Id { get; set; }
    public int Synergy { get; set; }

    public int Tier { get; set; }
}