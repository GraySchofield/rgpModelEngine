using UnityEngine;
using System.Collections.Generic;

namespace GSStorm.RPG.Engine
{
	[CreateAssetMenu(fileName = "Gear", menuName = "Template/Gear", order = 3)]
	public class GearTemplate : ItemTemplate
	{
		[Space]

		[Header ("Gear Values")]
		public GearType GearType;
		public GearBodyPosition BodyPosition;
        public int SocketCount;

		public int MinRequiredLevel;

        public List<AttributeTemplate> Attributes;
			
	}
}

