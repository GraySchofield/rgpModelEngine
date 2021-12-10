using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GSStorm.RPG.Engine;
using UnityEditor;

[RequireComponent(typeof(SimpleCellularAutomata))]
[RequireComponent(typeof(TinyKeep))]

public class MapManager : MonoBehaviour
{
    public static SimpleCellularAutomata SimpleCellularAutomata { get; private set; }
    public static TinyKeep TinyKeep { get; private set; }

    public MapGeneratorType GeneratorType = MapGeneratorType.SimpleCellularAutomata;

    private IMapGenerator _generator;

    private void Awake()
    {
        SimpleCellularAutomata = GetComponent<SimpleCellularAutomata>();
        TinyKeep = GetComponent<TinyKeep>();

        if (GeneratorType == MapGeneratorType.SimpleCellularAutomata)
        {
            _generator = SimpleCellularAutomata;
        } else if (GeneratorType == MapGeneratorType.TinyKeep)
        {
            _generator = TinyKeep;
        }
    }

    private void Start()
    {
        _generator.GenerateMap();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _generator.GenerateMap();
        }
    }

    private void OnDrawGizmos()
    {
        if (_generator != null && _generator.Map != null)
        {
            int width = _generator.Map.GetLength(0);
            int height = _generator.Map.GetLength(1);
            int[,] map = _generator.Map;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width / 2 + x + 0.5f, -height / 2 + y + 0.5f, 0);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
}
