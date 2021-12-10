using System;

/// <summary>
/// A map point records all the map data information
/// on a single point in the map coordinate
/// </summary>
namespace GSStorm.RPG.Engine
{
	public enum MapTileType{
		SKY,
		NORMAL_LAND,
		GRASSS_LAND,
		MUD_LAND,
		RIVER
		//...
	}

	[Serializable]
	public class MapPoint
	{
		/// <summary>
		/// Whether you can stand on that point at all
		/// </summary>
		public bool IsWalkable;

		/// <summary>
		/// The geometry type of that map point
		/// </summary>
		public MapTileType TileType;

		public int GetWalkableValue(){
			if (IsWalkable)
				return 0;
			else
				return 1;
		}

		public void SetWalkableValue(int value){
			if (value == 0) {
				IsWalkable = true;
			}

			if (value == 1) {
				IsWalkable = false;
			}
		}
			
	}
}

