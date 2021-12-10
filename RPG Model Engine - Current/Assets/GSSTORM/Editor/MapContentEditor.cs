using System;
using UnityEngine;
using UnityEditor;

namespace GSStorm.RPG.Engine
{

	public class MapContentEditor : EditorWindow
	{
		#region Private Properties
		private MapTemplate _currentMap;
		private int _currentIndexX = -1;
		private int _currentIndexY = -1;
		#endregion

		public MapTemplate CurrentMap {
			get{
				return _currentMap;
			}
			set{
				_currentMap = value;
			}
		}

		void OnGUI(){
			//Draw the map editor
			GUILayout.BeginHorizontal();

			GUILayout.BeginVertical ();
			if (_currentMap != null) {
				DrawMapUI(_currentMap);
			}
			GUILayout.EndVertical();

			GUILayout.BeginVertical ();
			EditorGUILayout.LabelField ("Map Point Content",EditorStyles.boldLabel);

			if (_currentIndexX >= 0 && _currentIndexY >= 0) {
				DrawMapPointDetail ();
			}

			GUILayout.EndVertical ();

			GUILayout.EndHorizontal ();

			if (GUI.changed)
			{
				if (_currentMap != null)
				{
					EditorUtility.SetDirty(_currentMap);
				}
			}
		}


		private void DrawMapPointDetail(){
			_currentMap [_currentIndexX, _currentIndexY].IsWalkable = EditorGUILayout.Toggle ("Is Walkable", _currentMap [_currentIndexX, _currentIndexY].IsWalkable);
			_currentMap [_currentIndexX, _currentIndexY].TileType = (MapTileType)EditorGUILayout.EnumPopup ("Ground Type", _currentMap [_currentIndexX, _currentIndexY].TileType);
		}

		private void DrawMapUI(MapTemplate map)
		{
			if (map != null)
			{
				EditorGUILayout.LabelField("Map Content", EditorStyles.boldLabel);
				GUILayout.Space(10);
				map.Name = EditorGUILayout.TextField("Map Name", map.Name);

				map.Seed = EditorGUILayout.TextField("Seed", map.Seed);
				map.Width = EditorGUILayout.IntField("Width", map.Width);
				map.Height = EditorGUILayout.IntField("Height", map.Height);
				map.FillPercent = EditorGUILayout.FloatField("Fill Percent", map.FillPercent);

				GUILayout.Space(10);

				GUI.color = Color.green;
				if (GUILayout.Button("GENERATE MAP CONTENT"))
				{
					map.GenerateMapData(map.Name);
				}
				GUI.color = Color.white;

				//Draw the actual map UI
				GUILayout.BeginHorizontal(GUILayout.Width(200));

				for (int x = 0; x < map.Width; x++)
				{
					GUILayout.BeginVertical(GUILayout.Height(200));
					for (int y = 0; y < map.Height; y++)
					{
						if (map [x, y] == null)
							return;

						if (!map[x, y].IsWalkable)
						{
							GUI.color = Color.gray;
							if (x == _currentIndexX && y == _currentIndexY)
								GUI.color = Color.green;

							if (GUILayout.Button ("", GUILayout.Width (15), GUILayout.Height (15))){
								//TODO: show some UI in editor for us to edit the mapcontent
								_currentIndexX = x;
								_currentIndexY = y;
							}
						}
						else
						{
							GUI.color = Color.white;
							if (x == _currentIndexX && y == _currentIndexY)
								GUI.color = Color.green;
							
							if (GUILayout.Button ("", GUILayout.Width (15), GUILayout.Height (15))) {
								_currentIndexX = x;
								_currentIndexY = y;
							}
						}

						GUI.color = Color.white;
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndHorizontal();

			}
		}

	}
}

