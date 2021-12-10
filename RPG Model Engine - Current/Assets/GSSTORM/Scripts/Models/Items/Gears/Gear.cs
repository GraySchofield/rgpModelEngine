using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace GSStorm.RPG.Engine
{
    [Serializable]
	public class Gear : Item
    {
		#region Public Properties
		/// <summary>
		/// The attributes that the gear
		/// will provide once equipped
		/// </summary>
		/// <value>The attributes.</value>
        public AttributeSet Attributes { get; set; }

		/// <summary>
		/// The Type of the gear,
		/// For eg, Weapon, Armor, etc
		/// </summary>
		/// <value>The type of the gear.</value>
        public GearType GearType { get; set; }

		/// <summary>
		/// The body position where this gear can be 
		/// put onto
		/// </summary>
		/// <value>The body position.</value>
		public GearBodyPosition BodyPosition{ get; set; }

	
		/// <summary>
		/// Gets or sets the rune sockets.
		/// </summary>
		/// <value>The rune sockets.</value>
		public List<RuneSocket> RuneSockets{
			get;
			set;
		}
			
		public int MinRequiredLevel{
			get;
			set;
		}

        public CombatUnit WearingUnit { get; set; }

		#endregion

		public Gear(){
			Attributes = new AttributeSet();
			RuneSockets = new List<RuneSocket>();
		}

        public bool CanEquip (CombatUnit unit){
            if (unit.Level >= MinRequiredLevel)
				return true;
			return false;
		}

		#region Runes

		/// <summary>
		/// Player plugins in a rune
		/// </summary>
		/// <param name="rune">Rune.</param>
		/// <param name="index">Index.</param>
		public virtual bool PluginRune(Rune rune, int index, out Rune swappedRune){
            
			if (RuneSockets == null || RuneSockets.Count <= index) {
                swappedRune = null;
                return false;
			}

			swappedRune = UnpluginRune(index);

			RuneSockets[index].PluginRune(rune);
	
			//Regist callbacks for rune effect modifier for the main player 
            if (WearingUnit != null) {
                UpdateRuneOnEquip(rune);
			}

            return true;
		}

		/// <summary>
		/// Player removes a rune
		/// </summary>
		/// <param name="rune">Rune.</param>
        public virtual Rune UnpluginRune(int index){
			if (RuneSockets == null || RuneSockets.Count <= index) {
				return null;
			}

            Rune old = RuneSockets[index].UnPuginRune();

            if (WearingUnit != null && old != null) {
                UpdateRuneOnUnequip(old);
			}

            return old;
				
		} 

        /// <summary>
        /// Updates the rune on equip.
        /// </summary>
        /// <param name="rune">Rune.</param>
        public void UpdateRuneOnEquip(Rune rune){
            rune.OnEquipTo(WearingUnit);
            AddRuneCallbacks(rune);
        }

        /// <summary>
        /// Updates the rune on unequip.
        /// </summary>
        /// <param name="rune">Rune.</param>
        public void UpdateRuneOnUnequip(Rune rune){
            rune.OnUnequippedFrom(WearingUnit);
            RemoveRuneCallbacks(rune);
        }

        private void AddRuneCallbacks(Rune rune)
        {
            WearingUnit.Skills.AttackSkill.PrepareCallback += rune.OnPrepareForSkill;
            WearingUnit.Skills.AttackSkill.OnCastCallback += rune.OnCastForSkill;
            WearingUnit.Skills.AttackSkill.BeforeHitCallback += rune.OnBeforeSkillHit;
            WearingUnit.Skills.AttackSkill.OnHitCallback += rune.OnSkillHit;
        }

        private void RemoveRuneCallbacks(Rune rune)
        {
            WearingUnit.Skills.AttackSkill.PrepareCallback -= rune.OnPrepareForSkill;
            WearingUnit.Skills.AttackSkill.OnCastCallback -= rune.OnCastForSkill;
            WearingUnit.Skills.AttackSkill.BeforeHitCallback -= rune.OnBeforeSkillHit;
            WearingUnit.Skills.AttackSkill.OnHitCallback -= rune.OnSkillHit;
        }


		#endregion
	}
}
