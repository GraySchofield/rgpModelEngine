using System.Collections;
using System.Collections.Generic;
using System;

namespace GSStorm.RPG.Engine
{
    /// <summary>
    /// Gift set for player.
    /// models the collection of gift on a player
    /// 
    /// </summary>
    public class GiftSet
	{
        private Dictionary<string, Gift> _gifts;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:GSStorm.RPG.Engine.GiftSet"/> class.
        /// </summary>
        /// <param name="p">The player for this gift set.</param>
        public GiftSet(){
            _gifts = new Dictionary<string, Gift>();
        }

		/// <summary>
		/// Check if has already learnt a gift 
		/// </summary>
		/// <returns><c>true</c>, if gift was learnt, <c>false</c> otherwise.</returns>
		/// <param name="typeId">Type identifier.</param>
		public bool Equipped(string typeId)
		{
            if (_gifts.ContainsKey(typeId))
				return true;

			return false;
		}

		/// <summary>
		/// Learns the gift.
		/// </summary>
		/// <param name="gift">The gift.</param>
		public bool Equip(Gift gift, CombatUnit combatUnit)
		{
			if (!Equipped(gift.TypeId))
			{
                _gifts[gift.TypeId] = gift;
				gift.AddAttributes(combatUnit);
				return true;
			}

			return false;
		}


		/// <summary>
		/// Forgets the gift.
		/// </summary>
		/// <param name="gift">The gift.</param>
		public bool Unequip(Gift gift, CombatUnit combatUnit)
		{
            if (Equipped(gift.TypeId))
			{
                _gifts.Remove(gift.TypeId);
				gift.SubstractAttributes(combatUnit);
				return true;
			}

			return false;
		}

        /// <summary>
        /// Gets the enumerable gift list.
        /// </summary>
        /// <value>The gift list.</value>
        public IEnumerable<Gift> Gifts { get { return _gifts.Values; } }

	}
}
