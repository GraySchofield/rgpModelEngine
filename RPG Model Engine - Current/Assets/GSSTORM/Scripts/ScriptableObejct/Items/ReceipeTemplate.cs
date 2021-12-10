using UnityEngine;
using System.Collections.Generic;

namespace GSStorm.RPG.Engine
{
	[CreateAssetMenu(fileName = "Receipe", menuName = "Template/Receipe", order = 4)]
	public class ReceipeTemplate : ItemTemplate
	{
		[Space]

		[Header ("Receipe Values")]
		public List<ReceipeUnit> ReceipeCost;

		public List<ReceipeUnit> ReceipeProduction;

	}
}

