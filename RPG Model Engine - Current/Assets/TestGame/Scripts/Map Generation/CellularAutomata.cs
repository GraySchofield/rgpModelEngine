using UnityEngine;
using System.Collections;
using System;

public class CellularAutomata
{
    [Range(0, 100)]
    public int RandomFillPercent = 40;
    public string Seed;
    public bool UseRandomSeed = true;
    public int SmoothNumber = 5;
    public int PassThreshold = 5;
    public int WipeThreshold = 3;

    public void GenerateMap(int[,] map, int left, int right, int top, int bottom)
    {
        RandomFillMap(map, left, right, top, bottom);

        for (int i = 0; i < 5; i++)
        {
            SmoothMap(map, left, right, top, bottom);
        }
    }

    void RandomFillMap(int[,] map, int left, int right, int top, int bottom)
    {
        if (UseRandomSeed)
        {
            Seed = DateTime.Now.Millisecond.ToString();
        }

        System.Random pseudoRandom = new System.Random(Seed.GetHashCode());

        for (int x = left; x < right; x++)
        {
            for (int y = top; y < bottom; y++)
            {
                if (x == left || x == right - 1 || y == top || y == bottom - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (pseudoRandom.Next(0, 100) < RandomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap(int[,] map, int left, int right, int top, int bottom)
    {
        for (int x = left; x < right; x++)
        {
            for (int y = top; y < bottom; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y, map, left, right, top, bottom);

                if (neighbourWallTiles >= PassThreshold) { map[x, y] = 1; }
                else if (neighbourWallTiles <= WipeThreshold) { map[x, y] = 0; }
            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY, int[,] map, int left, int right, int top, int bottom)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= left && neighbourX < right && neighbourY >= top && neighbourY < bottom)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount += 1;
                }
            }
        }
        return wallCount;
    }
}
