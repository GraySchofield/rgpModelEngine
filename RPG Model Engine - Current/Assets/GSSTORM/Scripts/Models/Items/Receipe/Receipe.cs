using System;
using System.Collections.Generic;

namespace GSStorm.RPG.Engine
{
	[Serializable]
	public class Receipe : Item
	{
		public List<ReceipeUnit> ReceipeCost {
			get;
			private set;
		}

		public List<ReceipeUnit> ReceipeProduction{
			get;
			private set;
		}

		public Receipe(){
			ReceipeCost = new List<ReceipeUnit> ();
			ReceipeProduction = new List<ReceipeUnit> ();
		}


		/// <summary>
		/// Use this receipe to do the production
		/// </summary>
		public bool Produce (Player player){
			//1. Check if the player has the require materials

			// Player.CheckProductivity(ReceipeCost)

			// 2. Generate the production, add to player's bag
			foreach (var ru in ReceipeProduction) {
				//player.Bag.AddItem (ItemFactory.Current.Create(ru.TypeId, ru.Amount));
			}

			return true;
		}
	}
}

