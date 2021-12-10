using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GSStorm.RPG.Engine;
using UnityEditor;

public class MapGenerator : MonoBehaviour {
    [Range(0, 100)]
    public int RandomFillPercent;

    public int Width;
    public int Height;

    public string Seed;
    public bool UseRandomSeed;

    private int[,] _map;

    private void Start()
    {
        GenerateMap();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GenerateMap();
        }
    }

    void GenerateMap()
    {
        _map = new int[Width, Height];
        RandomFillMap();

        for (int i = 0; i < 5; i++)
        {
            SmoothMap();
        }

        MapTemplate asset = ScriptableObject.CreateInstance<MapTemplate>();
        AssetDatabase.CreateAsset(asset, "Assets/GSSTORM/Resources/GSSTormGameContent/Maps/" + "map-" + Seed + ".asset");
        asset.Seed = Seed;
       // asset._tiles = _map;
        AssetDatabase.SaveAssets();
    }

    void RandomFillMap()
    {
        if (UseRandomSeed)
        {
            Seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(Seed.GetHashCode());

        for (int x = 0; x < Width; x++)
        {
            for(int y = 0; y < Height; y++)
            {
                if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
                {
                    _map[x, y] = 1;
                }
                else
                {
                    _map[x, y] = (pseudoRandom.Next(0, 100) < RandomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);
                
                if (neighbourWallTiles > 4) { _map[x, y] = 1; }
                else if (neighbourWallTiles < 4) { _map[x, y] = 0;  }
            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for(int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for(int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if(neighbourX >= 0 && neighbourX < Width && neighbourY >= 0 && neighbourY < Height)
                {
                    if(neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += _map[neighbourX, neighbourY];
                    }
                } else
                {
                    wallCount += 1;
                }
            }
        }
        return wallCount;
    }

    private void OnDrawGizmos()
    {
        if(_map != null)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Gizmos.color = (_map[x, y] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-Width / 2 + x + 0.5f, -Height / 2 + y + 0.5f, 0);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
}
