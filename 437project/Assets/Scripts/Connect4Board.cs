using UnityEngine;

public class Connect4Board
{
    readonly int[] pointDist = { 0, 0, 1, 6, 1000 };
    const int centerPoint = 2;

    public static int CheckWin(int[,] b)
    {
        return 0;
    }

    private bool PlaceChip(int[,] b, int c, int type)
    {
        if (b[5][c] == 0)
        {
            for (int i = 0; i < 6; i++)
            {
                if (b[i][c] == 0)
                {
                    b[i][c] = type;
                    break;
                }
            }
            return true;
        } 
        else
        {
            return false;
        }
    }

    private int RowSweep(int[,] b, int r)
    {
        int[] chunk = { 0, 0, 0 };
        int total = 0;
        for (int i = 0; i < 3; i++)
        {
            chunk[b[r, i] + 1]++;
        }
        for (int i = 3; i < 7; i++)
        {
            chunk[b[r, i] + 1]++;
            total += EvalChunk(chunk);
            chunk[b[r, i - 3] + 1]--;
        }
        return total;
    }

    private int ColumnSweep(int[,] b, int c)
    {
        int[] chunk = { 0, 0, 0 };
        int total = 0;
        for (int i = 0; i < 3; i++)
        {
            chunk[b[i, c] + 1]++;

        }
        for (int i = 3; i < 6; i++)
        {
            chunk[b[i, c] + 1]++;
            total += EvalChunk(chunk);
            chunk[b[i - 3, c] + 1]--;
        }
    }

    private int EvalChunk(int[] chunk) {
        if (chunk[0] == 0)
        {
            return pointDist[chunk[2]];
        }
        else if (chunk[2] == 0)
        {
            return -pointDist[chunk[0]];
        }
        else
        {
            return 0;
        }
    }
}
