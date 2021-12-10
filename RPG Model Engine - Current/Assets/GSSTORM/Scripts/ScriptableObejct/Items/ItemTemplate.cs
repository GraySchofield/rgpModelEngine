using UnityEngine;
using System.Collections;

/// <summary>
/// Base template class for create items,
/// templates for stuffs like gear, etc
/// should derive from this calss
/// </summary>

namespace GSStorm.RPG.Engine
{
	[CreateAssetMenu(fileName = "Item", menuName = "Template/Item", order = 2)]
	public class ItemTemplate : BaseTemplate
	{
		[Space]
	
		[Header("Item Values")]
		public int Level;
        public int MaxLevel;
		public RarityType Rarity;
		public string Description;
		[Range(0,1)]
		public float Vitality = 1;
		public int Tier;
		public int Weight;
		public CategoryType Category;


        /// <summary>
        /// Tells us if this item is countable
        /// </summary>
        public bool IsCountable;
        public int MaxStackCount;


        /// <summary>
        /// The image of the item when drop 
        /// on the map
        /// </summary>
        public Texture2D MapIcon;


    }

}

