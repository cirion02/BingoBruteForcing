using System.Runtime.InteropServices;

namespace BingoForcing.BoardGeneration;

public class RandomListHolder
{
    private int[] _internalList;
    private int _i;
    private int _j;

    public RandomListHolder(int[] seedList)
    {
        int inputLength = seedList.Length;

        _internalList = new int[256];

        for (int i = 0; i < 256; i++)
        {
            _internalList[i] = i;
        }

        int temp2 = 0;
        
        for (int i = 0; i < 256; i++)
        {
            int temp1 = _internalList[i];
            temp2 = (temp1 + temp2 + seedList[i % inputLength]) % 256;
            _internalList[i] = _internalList[temp2];
            _internalList[temp2] = temp1;
        }

        _i = 0;
        _j = 0;

        GenerateRandomNum(256);
    }

    public double GenerateRandomNum(int inputNum)
    {
        int d = (_i + 1) % 256;
        int e = _internalList[d];
        int f = (_j + e) % 256;
        int h = _internalList[f];

        _internalList[d] = h;
        _internalList[f] = e;

        double i = _internalList[(e + h) % 256];

        inputNum--;

        while (inputNum != 0)
        {
            d = (d + 1) % 256;
            e = _internalList[d];
            f = (f + e) % 256;
            h = _internalList[f];
            _internalList[d] = h;
            _internalList[f] = e;
            i = i * 256 + _internalList[(e + h) % 256];

            inputNum--;
        }

        _i = d;
        _j = f;

        return i;
    }
}

public class SeedRandom
{
    private RandomListHolder _randomListHolder;
    
    public SeedRandom(string seedString)
    {
        _randomListHolder = new RandomListHolder(SeedToUTF16List(seedString));
    }

    private int[] SeedToUTF16List(string seedString)
    {
        int[] result = new int[seedString.Length];
        for (int i = 0; i < seedString.Length; i++)
        {
            result[i] = int.Parse(seedString[i].ToString()) + 48;
        }
        return result;
    }

    public double Random()
    {
        double c = _randomListHolder.GenerateRandomNum(6);
        double d = 281474976710656;
        double b = 0;

        while (c < 4503599627370496)
        {
            c = (c + b) * 256;
            // this desyncs with native nodejs code, since nodejs has a rounding error,
            // it however does not seem to actually effect the outcome of the function
            d *= 256;
            b = _randomListHolder.GenerateRandomNum(1);
        }

        while (c > 9007199254740992)
        {
            c /= 2;
            d /= 2;
            b = (double) ((uint)b >> 1);
        }
        
        return (c + b) / d;
    }
}

