using System;

namespace GSStorm.RPG.Engine
{
    /// <summary>
    /// Class for any character, including NPCs, player and monsters.
    /// 
    /// Compared to CombatUnit, Character may have bag to carry items
    /// </summary>
	[Serializable]
    public class Character : CombatUnit
    {
        /// <summary>
        /// Bag
        /// </summary>
        /// <value>The bag.</value>
        public Bag Bag { get; set; }

        public override bool EquipGear(Gear gear, out Gear swappedGear)
        {
            if (!base.EquipGear(gear, out swappedGear)) { return false; }

            // Remove the item from the bag.
            // TODO: do we really want to remove the item from the bag?
            Bag.RemoveItem(gear);
            return true;
        }

        public override Gear UnequipGear(GearBodyPosition position)
        {
            Gear unequippedGear = base.UnequipGear(position);

            // Add the item to the bag.
            // TODO: do we really want to add them to the bag?
            if (unequippedGear != null)
            {
                Bag.AddItem(unequippedGear);
            }
            return unequippedGear;
        }
	}
}