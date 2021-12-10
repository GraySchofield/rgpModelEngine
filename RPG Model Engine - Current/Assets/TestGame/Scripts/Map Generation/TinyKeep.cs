using UnityEngine;
using System.Collections;
using System;

public class TinyKeep : MonoBehaviour, IMapGenerator
{
    public int[,] Map { get; private set; }

    public string Seed;
    public bool UseRandomSeed = true;

    public Sprite BasicSprite;
	public GameObject RoomTile;

    private System.Random _pseudoRandom = new System.Random();

    public void GenerateMap()
    {
        for(int i = 0; i < 50; i++)
        { 
            Vector2 pos = GetRandomPointInCircle();

			GameObject newCube = Instantiate (RoomTile, pos, Quaternion.identity); 
            int width = _pseudoRandom.Next(10, 100);
            int height = width + _pseudoRandom.Next(-10, 10);
            Vector3 scale = new Vector3(width, height, 10);
            newCube.transform.localScale = scale;

            if (width > 75)
            {
				newCube.transform.Find ("RoomContent").GetComponent<SpriteRenderer> ().color = Color.red;
            }

        }
    }

    public void Setup()
    {
        if (UseRandomSeed)
        {
            Seed = DateTime.Now.Millisecond.ToString();
        }

        _pseudoRandom = new System.Random(Seed.GetHashCode());
    }

    private Vector2 GetRandomPointInCircle()
    {
        double radius = 200;

        double t = 2 * Math.PI * _pseudoRandom.NextDouble();
        double u = _pseudoRandom.NextDouble() + _pseudoRandom.NextDouble();
        double r;
        if (u > 1) r = 2 - u; else r = u;
        return new Vector2((float)(radius * r * Math.Cos(t)), (float)(radius * r * Math.Sin(t)));
    }

    private void OnDrawGizmos()
    {
    }
}
