/// <summary>
/// 装备栏
/// The Gear set class models
/// a collection of gears
/// that the player is currently
/// wearing
/// </summary>
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

namespace GSStorm.RPG.Engine
{
    /// <summary>
    /// Set of equipped gears
    /// </summary>
    [Serializable]
    public class GearSet
    {
        private Dictionary<GearBodyPosition, Gear> _gears;

		public GearSet(){
            _gears = new Dictionary<GearBodyPosition, Gear> ();
		}

        /// <summary>
        /// Equip the specified gear for a CombatUnit.
        /// </summary>
        /// <returns>true if the gear is equipped, 0 otherwise.</returns>
        /// <param name="gear">Gear.</param>
        /// <param name="combatUnit">CombatUnit.</param>
        /// <param name="swappedGear">Swapped gear.</param>
        public bool Equip(Gear gear, CombatUnit combatUnit, out Gear swappedGear){
            swappedGear = null;
			if (_gears.ContainsKey (gear.BodyPosition)) {
				//Already wearing another gear in this body part
		
				//Unequip the old gear
				Debug.Log ("Unequiping the old gear...");
                swappedGear = combatUnit.UnequipGear(gear.BodyPosition);
			}

			//Add the gear now
			_gears[gear.BodyPosition] = gear;

			return true;
		}

        /// <summary>
        /// Get the gear in the specified bodyPositon.
        /// </summary>
        /// <returns>The gear.</returns>
        /// <param name="bodyPositon">Body positon.</param>
        public Gear Get(GearBodyPosition bodyPositon)
        {
            if (_gears.ContainsKey(bodyPositon))
            {
                return _gears[bodyPositon];
            }
            return null;
        }


		/// <summary>
		/// remove gear from this set
		/// </summary>
		/// <returns>The removed gear.</returns>
        public Gear Remove(GearBodyPosition bodyPositon){
            if (!_gears.ContainsKey (bodyPositon)) {
				Debug.LogError ("Tring to remove a gear that is not equipped!");
				return null;
			}

            Gear gear = _gears[bodyPositon];
            _gears.Remove(gear.BodyPosition);
			return gear;
		}
    }
}        
