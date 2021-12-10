using System;
using UnityEngine;
using System.Collections.Generic;

namespace GSStorm.RPG.Engine{

    /// <summary>
    /// Base class for runes,
    /// runes are support mechanics for the main attack skill
    /// each specific rune should subclass from this class
    /// and override the virtual functions
    /// 
    /// </summary>
    /// 
    [Serializable]
    public class Rune: Item
    {
        public List<Effect> Effects { get; private set; }

		public Rune(){
            Effects = new List<Effect>();
		}
		
		/// <summary>
		/// Rune Effect when equipped to the weapon
		/// Note:Runes should be equipped to the weapon of the target character 
		/// </summary>
		/// <param name="target">Target.</param>
        public virtual void OnEquipTo(CombatUnit target){
			Debug.Log ("Rune " + Name + " is equipped to : " + target.Name);

            foreach (Effect e in Effects)
            {
                EffectLibrary.OnEquip(e, target);
            }
		}

		/// <summary>
		/// Remove the rune effect when taken away from a weapon
		/// </summary>
		/// <param name="target">Target.</param>
        public virtual void OnUnequippedFrom(CombatUnit target){
			Debug.Log ("Rune " + Name + " is unequipped from : " + target.Name);

            foreach (Effect e in Effects)
            {
                EffectLibrary.OnUnequip(e, target);
            }
		}
     

		/// <summary>
		/// Rune effect when preparing for skill
		/// </summary>
		/// <param name="skill">Skill.</param>
		public virtual void OnPrepareForSkill(CombatUnit caster, AttackSkill skill){
			if (skill == null)
				return;
			Debug.Log ("Rune " + Name + " is active for preparing skill : " + skill.Name);
		}

		/// <summary>
		/// Rune effect when skill is already is casted out
		/// </summary>
		/// <param name="skill">Skill.</param>
		public virtual void OnCastForSkill(CombatUnit caster, AttackSkill skill){
			if (skill == null)
				return;
			Debug.Log ("Rune " + Name + " is active for on cast skill : " + skill.Name);

            foreach (Effect e in Effects)
            {
                EffectLibrary.OnSkillCast(e, caster, skill);
            }
        }


		/// <summary>
		/// Rune effect right before the skill will hit the target
		/// </summary>
		/// <param name="skill">Skill.</param>
		/// <param name="target">Target.</param>
		public virtual void OnBeforeSkillHit(CombatUnit caster, AttackSkill skill, CombatUnit target){
			if (skill == null || target == null)
				return;
			Debug.Log ("Rune " + Name + " is active for before skill hit : " + skill.Name);
		}

		/// <summary>
		/// Rune effect right on the skill hit the target
		/// </summary>
		/// <param name="skill">Skill.</param>
		/// <param name="target">Target.</param>
		public virtual void OnSkillHit(CombatUnit caster, AttackSkill skill, CombatUnit target){
			if (skill == null || target == null)
				return;
			Debug.Log ("Rune " + Name + " is active for right on skill hit : " + skill.Name);

            foreach (Effect e in Effects)
            {
                EffectLibrary.OnSkillHit(e, caster, skill, target);
            }
		
		}

        /// <summary>
        /// Adds the effect.
        /// </summary>
        /// <param name="e">The effect.</param>
        public void AddEffect(Effect e){
            Effects.Add(e);
        }

	}
}

