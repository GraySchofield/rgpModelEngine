using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GSStorm.RPG.Engine
{
    // 天赋
    public class Gift: BaseModel
	{
		/// <summary>
		/// The required level that a character can learn this gift.
		/// </summary>
		public int RequiredLevel;

		/// <summary>
		/// Attribute set that defines this gift in term of general purpose.
		/// </summary>
		public AttributeSet Attributes;

		/// <summary>
		/// Determine if the player can learn this skill.
		/// </summary>
		/// <returns><c>true</c>, if the player can learn the skill <c>false</c> otherwise.</returns>
		/// <param name="learner">The player to learn the skill.</param>
        public bool CanLearn(CombatUnit learner)
		{
			if (learner.Level < RequiredLevel) return false;
			return true;
		}

		/// <summary>
		/// Learn the gift.
		/// </summary>
		/// <returns>whether the player has learnt the gift</returns>
		/// <param name="learner">Player to learn the gift</param>
		public bool Learn(CombatUnit learner)
		{
            // Sanity check
			if (!CanLearn(learner)) return false;

			return learner.Gifts.Equip(this, learner);
		}

        /// <summary>
        /// Adds the attributes of this gift to the combat unit.
        /// </summary>
        /// <param name="cu">The combat unit.</param>
        public void AddAttributes(CombatUnit cu){
			//Update attributes of the skill
			cu.Attributes.Add(Attributes);
        }

		/// <summary>
		/// Determine whether the character can forget this gift.
		/// </summary>
		/// <returns><c>true</c>, if this gift can be forgot by the player, <c>false</c> otherwise.</returns>
		/// <param name="leaner">The player to forget the skill.</param>
        public virtual bool CanForget(CombatUnit leaner) { return true; }

		/// <summary>
		/// Let player forget a learnt skill.
		/// </summary>
		/// <returns>if player has forgot the skill.</returns>
		/// <param name="learner">The player to forget the skill.</param>
		public bool Forget(CombatUnit learner)
		{
			if (!CanForget(learner)) return false;

			return learner.Gifts.Unequip(this, learner);
		}

        /// <summary>
        /// Remove the attributes of this gift to the combat unit.
        /// </summary>
        /// <param name="cu">The combat unit.</param>
        public void SubstractAttributes(CombatUnit cu){
			cu.Attributes.Substract(Attributes);
        }
	}
}
