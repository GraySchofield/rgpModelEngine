using UnityEngine;
using System.Collections.Generic;
using System;
/// <summary>
/// Template for a map.
/// 
/// Beta version...
/// </summary>

namespace GSStorm.RPG.Engine
{
    [CreateAssetMenu(fileName = "map", menuName = "Template/Map")]
	public class MapTemplate : BaseTemplate
    {
        public string Seed;
   
        public int Width = 30;
        public int Height = 30;
        public float FillPercent = 50f;

		//Only public variables are serializebale in Scriptable object 
		public MapPoint[] _tiles;

		public MapPoint this[int x, int y]
        {
            get
            {	
				if (0 <= x &&  x < Width && 0 <= y && y < Height) {
					if ((y * Width + x) < _tiles.Length) {
						return _tiles [y * Width + x];
					} 
				} 
				Debug.LogError("Index is out of bound !");
				return null;
			}
            set
            {
				if (0 <= x && x < Width && 0 <= y && y < Height) {
					if ((y * Width + x) < _tiles.Length) {
						_tiles [y * Width + x] = value;
						return;
					}
				}
				Debug.LogError("Index is out of bound !");
            }
        }

        public void GenerateMapData(string mapName)
        {
			Name = mapName;

			_tiles = new MapPoint [Width * Height];

            System.Random psdoRandom;

            if (Seed != null)
            {
                psdoRandom = new System.Random(Seed.GetHashCode());
                //Have a seed, use it to random
            }
            else
            {
                psdoRandom = new System.Random();
                //No seed
            }

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {

					MapPoint point = new MapPoint ();
					if (psdoRandom.Next (0, 100) >= FillPercent) {
						point.IsWalkable = true;
						point.TileType = MapTileType.NORMAL_LAND;
					} else {
						point.IsWalkable = false;
						point.TileType = MapTileType.SKY;
					}
					this [x, y] = point;

	             }
            }

            //TODO: this data should come from a 
            //map generation module, we are just using the following
            //lines for testing purpose
            //Celluar Automata
            for (int i = 0; i < 5; i++)
            {
                SmoothMap();
            }

            
        }

        void SmoothMap()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int neighbourWallTiles = GetSurroundingWallCount(x, y);

                    if (neighbourWallTiles > 4)
                    {
						this[x, y].SetWalkableValue(1);
                    }
                    else if (neighbourWallTiles < 4)
                    {
						this[x, y].SetWalkableValue(0);

                    }
                }
            }
        }

        int GetSurroundingWallCount(int girdX, int gridY)
        {
            int wallCount = 0;
            for (int x = girdX - 1; x <= girdX + 1; x++)
            {
                for (int y = gridY - 1; y <= gridY + 1; y++)
                {
                    if (x >= 0 && x < Width && y >= 0 && y < Height)
                    {
                        if (x != girdX || y != gridY)
                        {
							wallCount += this[x, y].GetWalkableValue();
                        }
                    }
                    else
                    {
                        wallCount++;
                    }
                }
            }

            return wallCount;
        }

    }

}
