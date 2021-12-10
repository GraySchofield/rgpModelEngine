using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

namespace GSStorm.RPG.Engine{
	public class MapFactoryTypeSprite
	{
		#region Private Properties
		private readonly string _prefabRoot = "Prefabs/";
		#endregion

		#region Public Properties
		/// <summary>
		/// maps the tile value to the name of 
		/// the prefab of the tile
		/// </summary>
		/// <value>The tile set map.</value>
		public Dictionary<TileValue, GameObject> TileSetMap {
			get;
			private set;
		}
	
		#endregion

		public IEnumerator GenerateSpriteMap(int[,] map, float squareSize){
			//Set up the tile set map
			TileSetMap = new Dictionary<TileValue, GameObject>();

			foreach (TileValue value in Enum.GetValues(typeof(TileValue))) {
				GameObject prefab = null;
				switch (value) {
				case TileValue.GROUND:
					prefab = Resources.Load(_prefabRoot + "Ground", typeof(GameObject)) as GameObject;

					Debug.Log ("Loaded prefab ground : " + prefab);

					break;
				case TileValue.WATER:
					prefab = Resources.Load(_prefabRoot + "Water", typeof(GameObject)) as GameObject;
					Debug.Log ("Loaded prefab ground : " + prefab);

					break;
				}
				TileSetMap.Add (value, prefab);
			}

			//Instantiate the Tiles
			int xCount = map.GetLength (0);
			int yCount = map.GetLength (1);

			float mapWidth = xCount * squareSize;
			float mapHeight = yCount * squareSize;

			GameObject mapObject = new GameObject();
			mapObject.name = "MapObject";

			for (int x = 0; x < xCount; x++) {
				for (int y = 0; y < yCount; y++) {
					GameObject tile = TileSetMap [map [x, y] == 1 ? TileValue.GROUND : TileValue.WATER];
					Vector3 pos = new Vector3( - mapWidth/2 + x * squareSize + squareSize/2 , -mapHeight/2 + y * squareSize + squareSize/2 , 0 );
					 //Debug.Log ("Instantiate Sprite at : " + x + ", " + y);
					GameObject resultTile = GameObject.Instantiate (tile, pos, Quaternion.identity);
					resultTile.transform.SetParent (mapObject.transform);
					resultTile.transform.localScale = new Vector3 (squareSize / resultTile.GetComponent<SpriteRenderer> ().sprite.bounds.size.x,
						squareSize / resultTile.GetComponent<SpriteRenderer> ().sprite.bounds.size.y, 1);

				}
			}

			yield return null;
		}
	}
}

