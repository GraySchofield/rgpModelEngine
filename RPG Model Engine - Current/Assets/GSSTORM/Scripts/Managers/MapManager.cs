using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GSStorm.RPG.Game;

namespace GSStorm.RPG.Engine{
    
	public enum TileValue{
		GROUND,
		WATER
	}

	public class MapManager : Singleton<MapManager>, IManager{
		#region Map content Generation Test
		//Testing Code, Map data generation should be done in MapGenerator Factory
		int[,] _map;
		int _width = 30;
		int _height = 30;
		int _randomFillPercent = 45;
		public int[,] MapData {
			get{
				return _map;
			}
			set{
				_map = value;
			}
		}

		void SmoothMap(){
			for (int x = 0; x < _width; x++) {
				for (int y = 0; y < _height; y++) {
					int neighbourWallTiles = GetSurroundingWallCount (x, y);

					if (neighbourWallTiles > 4) {
						_map [x, y] = 1;
					}else if (neighbourWallTiles < 4){
						_map [x, y] = 0;
					}
				}
			}
		}

		int GetSurroundingWallCount(int girdX, int gridY){
			int wallCount = 0;
			for (int x = girdX - 1 ; x <= girdX + 1; x++) {
				for (int y = gridY - 1; y <= gridY + 1; y++) {
					if (x >= 0 && x < _width && y >= 0 && y < _height) {
						if (x != girdX || y != gridY) {
							wallCount += _map [x, y];
						}
					} else {
						wallCount++;
					}
				}
			}

			return wallCount;
		}
		#endregion

		#region public Properties
		public MapFactoryTypeMesh MeshFactory {
			get;
			private set;
		}

		public MapFactoryTypeSprite SpriteFactory{
			get;
			private set;
		}


		public bool IsUsingMesh {
			get;
			set;
		}

		#endregion

		public ManagerStatus Status { get; private set;}

		public IEnumerator PreLaunch() { 
			Debug.Log ("Map Manager : pre launch ...");
			//Testing Code
			Status = ManagerStatus.PreLaunching;

			_map = new int[_width,_height];

			string seed = Time.time.ToString ();
			System.Random psdoRandom = new System.Random (seed.GetHashCode());

			for (int x = 0; x < _width; x++) {
				for (int y = 0; y < _height; y++) {
					_map [x, y] = (psdoRandom.Next (0, 100) < _randomFillPercent) ? 1 : 0;
				}
			}

			for (int i = 0; i < 1; i++) {
				SmoothMap ();
			}
				
			//Create the map factories
			MeshFactory = new MapFactoryTypeMesh();
			SpriteFactory = new MapFactoryTypeSprite();

			Debug.Log ("Map Manager : pre launch finished...");
			Status = ManagerStatus.StarLaunching;
			yield return null;

		}

		public IEnumerator StartLaunch() { 
			Debug.Log ("Map Manager : start launch ...");

			if (IsUsingMesh) {
				yield return CoreGameController.Current.StartCoroutine (MeshFactory.GenerateMesh(MapData, 1f));
			} else {
				yield return CoreGameController.Current.StartCoroutine (SpriteFactory.GenerateSpriteMap(MapData, 1f));
			}
				
			Debug.Log ("Map Manager :launch finished...");

			Status = ManagerStatus.Started;

			yield return null; 
		}


		public IEnumerator PostLaunch() { yield return null; }

	

	}
}