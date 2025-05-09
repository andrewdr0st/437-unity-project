using UnityEngine;
using System;

public class Connect4Board
{
    static readonly int[] pointDist = { 0, 0, 1, 10, 1000 };
    static readonly int[,] leftDiagPairs = { { 3, 0 }, { 4, 0 }, { 5, 0 }, { 5, 1 }, { 5, 2 }, { 5, 3 } };
    static readonly int[,] rightDiagPairs = { { 3, 6 }, { 4, 6 }, { 5, 6 }, { 5, 5 }, { 5, 4 }, { 5, 3 } };
    static readonly int[] diagCounts = { 4, 5, 6, 6, 5, 4 };
    const int centerPoint = 4;

    public static int[,] CreateBoard()
    {
        int[,] b = new int[6, 7];
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                b[i, j] = 0;
            }
        }
        return b;
    }

    public static int[,] CopyBoard(int[,] b)
    {
        int[,] n = new int[6, 7];
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                n[i, j] = b[i, j];
            }
        }
        return n;
    }

    public static int Evaluate(int[,] b)
    {
        int score = 0;
        for (int i = 0; i < 6; i++)
        {
            score += b[i, 3] * centerPoint;
            score += RowSweep(b, i);
            score += DiagonalSweepL(b, i);
            score += DiagonalSweepR(b, i);
            score += ColumnSweep(b, i);
        }
        score += ColumnSweep(b, 6);
        return score;
    }

    public static int MiniMax(int[,] b, int player, int depth, int alpha, int beta)
    {
        if (depth == 0 || CheckWin(b) != 0)
        {
            return Evaluate(b);
        }

        int bestScore = player == -1 ? 1000000 : -1000000;
        for (int i = 0; i < 7; i++)
        {
            if (PlaceChip(b, i, player)) {
                //bestScore = Math.Max(-MiniMax(b, -player, depth - 1, -beta, -alpha), bestScore);
                if (player == 1)
                {
                    bestScore = Math.Max(bestScore, MiniMax(b, -1, depth - 1, alpha, beta));
                } 
                else
                {
                    bestScore = Math.Min(bestScore, MiniMax(b, 1, depth - 1, alpha, beta));
                }
                    //alpha = Math.Max(alpha, bestScore);
                    PopChip(b, i);
                //if (alpha >= beta)
                //{
                //    break;
                //}
            }
        }
        return bestScore;
    }

    public static int CheckWin(int[,] b)
    {
        for (int i = 0; i < 6; i++)
        {
            int s = RowSweep(b, i);
            if (Math.Abs(s) > 500)
            {
                return Math.Sign(s);
            }
        }
        for (int i = 0; i < 7; i++)
        {
            int s = ColumnSweep(b, i);
            if (Math.Abs(s) > 500)
            {
                return Math.Sign(s);
            }
        }
        for (int i = 0; i < 6; i++)
        {
            int s = DiagonalSweepL(b, i);
            if (Math.Abs(s) > 500)
            {
                return Math.Sign(s);
            }
        }
        for (int i = 0; i < 6; i++)
        {
            int s = DiagonalSweepR(b, i);
            if (Math.Abs(s) > 500)
            {
                return Math.Sign(s);
            }
        }
        return 0;
    }

    public static bool PlaceChip(int[,] b, int c, int type)
    {
        if (b[5, c] == 0)
        {
            for (int i = 0; i < 6; i++)
            {
                if (b[i, c] == 0)
                {
                    b[i, c] = type;
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

    public static bool PopChip(int[,] b, int c)
    {
        for (int i = 5; i >= 0; i--)
        {
            if (b[i, c] != 0)
            {
                b[i, c] = 0;
                return true;
            }
        }
        return false;
    }

    private static int RowSweep(int[,] b, int r)
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

    private static int ColumnSweep(int[,] b, int c)
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
        return total;
    }

    private static int DiagonalSweepL(int[,] b, int d)
    {
        int[] chunk = { 0, 0, 0 };
        int total = 0;
        for (int i = 0; i < 3; i++)
        {
            chunk[b[leftDiagPairs[d, 0] - i, leftDiagPairs[d, 1] + i] + 1]++;
        }
        for (int i = 3; i < diagCounts[d]; i++)
        {
            chunk[b[leftDiagPairs[d, 0] - i, leftDiagPairs[d, 1] + i] + 1]++;
            total += EvalChunk(chunk);
            chunk[b[leftDiagPairs[d, 0] - i + 3, leftDiagPairs[d, 1] + i - 3] + 1]--;
        }
        return total;
    }

    private static int DiagonalSweepR(int[,] b, int d)
    {
        int[] chunk = { 0, 0, 0 };
        int total = 0;
        for (int i = 0; i < 3; i++)
        {
            chunk[b[rightDiagPairs[d, 0] - i, rightDiagPairs[d, 1] - i] + 1]++;
        }
        for (int i = 3; i < diagCounts[d]; i++)
        {
            chunk[b[rightDiagPairs[d, 0] - i, rightDiagPairs[d, 1] - i] + 1]++;
            total += EvalChunk(chunk);
            chunk[b[rightDiagPairs[d, 0] - i + 3, rightDiagPairs[d, 1] - i + 3] + 1]--;
        }
        return total;
    }

    private static int EvalChunk(int[] chunk) {
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
