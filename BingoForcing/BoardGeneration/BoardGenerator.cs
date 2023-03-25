namespace BingoForcing.BoardGeneration;

public class BoardGenerator
{
    private Objective[][] _generator;

    private BoardObjective[] _currentBoard;

    private int[][] _lineCheckList = new int[25][];

    private List<int> _randomTable1;
    private List<int> _randomTable2;
    private int _randomSeedX;

    public BoardGenerator(Objective[][] generator)
    {
        _generator = generator;
        _lineCheckList[0] = new int[] {1, 2, 3, 4, 5, 10, 15, 20, 6, 12, 18, 24};
        _lineCheckList[1] = new int[] {0, 2, 3, 4, 6, 11, 16, 21};
        _lineCheckList[2] = new int[] {0, 1, 3, 4, 7, 12, 17, 22};
        _lineCheckList[3] = new int[] {0, 1, 2, 4, 8, 13, 18, 23};
        _lineCheckList[4] = new int[] {0, 1, 2, 3, 8, 12, 16, 20, 9, 14, 19, 24};
        _lineCheckList[5] = new int[] {0, 10, 15, 20, 6, 7, 8, 9};
        _lineCheckList[6] = new int[] {0, 12, 18, 24, 5, 7, 8, 9, 1, 11, 16, 21};
        _lineCheckList[7] = new int[] {5, 6, 8, 9, 2, 12, 17, 22};
        _lineCheckList[8] = new int[] {4, 12, 16, 20, 9, 7, 6, 5, 3, 13, 18, 23};
        _lineCheckList[9] = new int[] {4, 14, 19, 24, 8, 7, 6, 5};
        _lineCheckList[10] = new int[] {0, 5, 15, 20, 11, 12, 13, 14};
        _lineCheckList[11] = new int[] {1, 6, 16, 21, 10, 12, 13, 14};
        _lineCheckList[12] = new int[] {0, 6, 12, 18, 24, 20, 16, 8, 4, 2, 7, 17, 22, 10, 11, 13, 14};
        _lineCheckList[13] = new int[] {3, 8, 18, 23, 10, 11, 12, 14};
        _lineCheckList[14] = new int[] {4, 9, 19, 24, 10, 11, 12, 13};
        _lineCheckList[15] = new int[] {0, 5, 10, 20, 16, 17, 18, 19};
        _lineCheckList[16] = new int[] {15, 17, 18, 19, 1, 6, 11, 21, 20, 12, 8, 4};
        _lineCheckList[17] = new int[] {15, 16, 18, 19, 2, 7, 12, 22};
        _lineCheckList[18] = new int[] {15, 16, 17, 19, 23, 13, 8, 3, 24, 12, 6, 0};
        _lineCheckList[19] = new int[] {4, 9, 14, 24, 15, 16, 17, 18};
        _lineCheckList[20] = new int[] {0, 5, 10, 15, 16, 12, 8, 4, 21, 22, 23, 24};
        _lineCheckList[21] = new int[] {20, 22, 23, 24, 1, 6, 11, 16};
        _lineCheckList[22] = new int[] {2, 7, 12, 17, 20, 21, 23, 24};
        _lineCheckList[23] = new int[] {20, 21, 22, 24, 3, 8, 13, 18};
        _lineCheckList[24] = new int[] {0, 6, 12, 18, 20, 21, 22, 23, 19, 14, 9, 4};
    }

    public BoardObjective[] GenerateBoard(int seed)
    {
        string stringSeed = seed.ToString();
        SeedRandom seedRandom = new SeedRandom(stringSeed);

        _currentBoard = new BoardObjective[25];
        
        /*
        Filled = False
        Id = 0
        Name = null
        Synergy = 0
        Types = null 
        */
        
        PrecalcDifficultyValues(seed);
        
        for (int i = 0; i < 25; i++)
        {
	        int difficulty = Difficulty(i);
	        int objectiveCount = _generator[difficulty].Length;
	        int rng = (int)Math.Floor(objectiveCount * seedRandom.Random());
	        if (rng == objectiveCount)
	        {
		        rng--;
		        //TODO: Check if this does anything
	        }

	        BoardObjective currentObjective =
		        BoardObjective.FromObjective(_generator[difficulty][rng % objectiveCount]);
	        int synergy = CheckLine(i, currentObjective.Types);

	        currentObjective.AddBoardInfo(rng % objectiveCount, synergy);
	        BoardObjective minSynergyObjective = currentObjective;
	        
	        int j = 1;
	        do
	        {
		        currentObjective =
			        BoardObjective.FromObjective(_generator[difficulty][(j + rng) % objectiveCount]);
		        synergy = CheckLine(i, currentObjective.Types);
		        
		        if (synergy < minSynergyObjective.Synergy)
		        {
			        currentObjective.AddBoardInfo((j + rng) % objectiveCount, synergy);
			        minSynergyObjective = currentObjective;
		        }

		        j++;
	        } while ((synergy != 0) && (j < objectiveCount));

	        _currentBoard[i] = minSynergyObjective;
        }
        return _currentBoard;
    }


    private int CheckLine(int i, string[] typesA)
    {
	    int synergy = 0;
	    for (int j = 0; j < _lineCheckList[i].Length; j++)
	    {
		    int otherSquareIndex = _lineCheckList[i][j];
		    if (_currentBoard[otherSquareIndex].Filled == false)
		    {
			    continue;
		    }

		    string[] typesB = _currentBoard[otherSquareIndex].Types;
		    for (int k = 0; k < typesA.Length; k++) {
			    for (int l = 0; l < typesB.Length; l++)
			    {
				    if (typesA[k] != typesB[l]) continue;
				    synergy++;
				    if (k == 0)
				    {
					    synergy++;
				    };
				    if (l == 0)
				    {
					    synergy++;
				    };
			    }
		    }
	    }

	    return synergy;
    }

    private void PrecalcDifficultyValues(int seed)
    {
	    //This function takes a space on the board between 1 and 25, and returns its difficulty score, also between 1 and 25.
	    //These should always form a magic square if seed remains the same

	    int firstThreeDigits = (seed / 1000) % 1000;
	    int lastThreeDigits = seed % 1000;

	    //Use last 3 digits to generate 4 random numbers. between 0-1, 0-2, 0-3, and 0-4 respectively
	    //Done by using relative prime factor modulo's. Except 2 and 4 which takes independent digits from a modulo 8
	    //3, 5, and 8 are co-prime so their modulo's are independent

	    int mod8 = lastThreeDigits % 8;

	    int random1 = mod8 % 2;		       //between 0-1    (this essentially takes the last bit of the 3 bit number) 
	    int random2 = lastThreeDigits % 3;  //between 0-2
	    int random3 = mod8 / 2;	           //between 0-3    (this essentially takes the first two bits of the 3 bit number) 
	    int random4 = lastThreeDigits % 5;  //between 0-4

	    //using the numbers generated above, we insert the numbers 0-4 into a list in a random order to create a shuffled table
	    _randomTable1 = new List<int>{0};
	    _randomTable1.Insert(random1, 1);
	    _randomTable1.Insert(random2, 2);
	    _randomTable1.Insert(random3, 3);
	    _randomTable1.Insert(random4, 4);


	    //Do the same this as above for a second table

	    mod8 = firstThreeDigits % 8;
	    random1 = mod8 % 2;
	    random2 = firstThreeDigits % 3;
	    random3 = mod8 / 2;
	    random4 = firstThreeDigits % 5;   

	    _randomTable2 = new List<int>{0};
	    _randomTable2.Insert(random1, 1);
	    _randomTable2.Insert(random2, 2);
	    _randomTable2.Insert(random3, 3);
	    _randomTable2.Insert(random4, 4);

	    //Since we use mod 3 5 and 8 for our random generation above, when dividing by 3*5*8=120 we have a new independent value.
	    //We use this to create one last value between 0 and 4
	    int lastDigitsSeed = lastThreeDigits / 120;
	    int firstDigitsSeed = firstThreeDigits / 120;
	    _randomSeedX = (lastDigitsSeed * 8 + firstDigitsSeed) % 5;
    }
    
    private int Difficulty(int i)
    {

	    int y = i / 5;
		//This represents the y position on the board, top row being a 0, next a 1. esc.

		int x = (i + _randomSeedX) % 5;
		//This represents the x position, modified by the randomSeedX. To make not every board use the same base for the magic square

		//since x is added to this value and then mod5'd, every distinct value of x returns a distinct value for index1.
		//and since every row has one objective of each x value, all objectives will have a different.
		//and since 3 and 5 do not share any common factors, every unique value of y will also give a unique value
		//so for index1, so each row also has 1 of each value.
		int index1 = (x + 3 * y) % 5;
		//Same is true for index2
		int index2 = (3 * x + y) % 5;
		

		//Now we use the tables generated earlier to get randomized difficulty scores. And since each row and column has every value 
		//for index1/index2 once, and the tables both contain every number once, each row and column will still have every value once.
		int groupOf5 = _randomTable1[index1];
		int withinGroup = _randomTable2[index2];

		//Now calculate the actual true difficulty of the space.
		int value = 5 * groupOf5 + withinGroup;

		//since every row/column has every value for groupOf5 and withinGroup once, the total for all of them will be the same.
		// 5 * (0,1,2,3,4) + (0,1,2,3,4)
		// (0,5,10,15,20) + (0,1,2,3,4) = 60
		
		return value;
	}
}