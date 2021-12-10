using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCellularAutomata : MonoBehaviour, IMapGenerator
{

    [Range(0, 100)]
    public int RandomFillPercent;

    public int Width = 100;
    public int Height = 100;

    public string Seed;
    public bool UseRandomSeed = true;

    public int SmoothNumber = 5;
    public int PassThreshold = 5;
    public int WipeThreshold = 3;

    public int[,] Map { get; private set; }

    public void GenerateMap()
    {
        Map = new int[Width, Height];

        CellularAutomata ca = new CellularAutomata();
        ca.RandomFillPercent = RandomFillPercent;
        ca.Seed = Seed;
        ca.SmoothNumber = SmoothNumber;
        ca.PassThreshold = PassThreshold;
        ca.WipeThreshold = WipeThreshold;
        ca.UseRandomSeed = UseRandomSeed;

        ca.GenerateMap(Map, 0, Width, 0, Height);
    }
}
