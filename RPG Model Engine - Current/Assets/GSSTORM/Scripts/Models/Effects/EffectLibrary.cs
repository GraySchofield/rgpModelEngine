using System;
using UnityEngine;
using System.Collections.Generic;
using GSStorm.RPG.Game;

namespace GSStorm.RPG.Engine
{

	/// <summary>
	/// Effect library. 
	/// Holds the actual meat of whatever special combat effect
	/// in the game
	/// </summary>
	public static class EffectLibrary
	{

		#region Effect for Runes

        /// <summary>
        /// Equip Stage Effects
        /// </summary>
        /// <param name="effect">The effect</param>
        /// <param name="unit">The unit that this effect equips to</param>
        public static void OnEquip(Effect effect, CombatUnit unit){
            switch (effect.Type){
                case EffectType.ATTACKSKILL_CD_REDUCTION:
                    CdReduction(effect, unit, false);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Unequip Stage Effects
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="unit"></param>
        public static void OnUnequip(Effect effect, CombatUnit unit)
        {
            switch (effect.Type)
            {
                case EffectType.ATTACKSKILL_CD_REDUCTION:
                    CdReduction(effect, unit, true);
                    break;
                default:
                    break;
            }
        }


        public static void OnSkillCast(Effect effect, CombatUnit caster, AttackSkill skill)
        {
            switch (effect.Type)
            {
                case EffectType.ATTACKSKILL_ADD_BUFF_TO_SELF:
                    AddBuffToSelf(effect, caster);
                    break;

                default:
                    break;
            }
        }

        public static void OnSkillHit(Effect effect, CombatUnit caster, AttackSkill skill, CombatUnit target)
        {
            switch (effect.Type)
            {
                case EffectType.ATTACKSKILL_ADD_BUFF_TO_OTHER:
                    AddBuffToOther(effect, target);
                    break;


                default:
                    break;
            }
        }
        


		#endregion


		#region Effect for Buffs
	
        /// <summary>
        /// Update stage effect
		/// 
		/// Currently only buff effect will execute in update 
		/// So if we want an update effect, we need to cast a buff
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="target"></param>
        public static void OnUpdate(Effect effect,CombatUnit target, float dt)
        {
            switch (effect.Type)
            {
                case EffectType.PER_SECOND_LIFE_REDUCTION:
                    PerSecondLifeReduction(effect, target, dt);
                    break;
            }
        }

		/// <summary>
		/// Taken damage stage effects
		/// </summary>
		/// <param name="effect">Effect.</param>
		/// <param name="damage">Damage.</param>
		/// <param name="caster">Caster.</param>
		/// <param name="target">Target.</param>
		public static void OnTakenDamageEffect(Effect effect, Damage damage, CombatUnit caster, CombatUnit target){
			switch (effect.Type) {
			default:
				break;
			}
		}

		#endregion


		#region Equip stage effects
        public static void CdReduction(Effect effect, CombatUnit unit, bool reverse)
		{
            float cdReduction = effect.Parameters[0] / 100f;
            if (reverse)
            {
                unit.Skills.AttackSkill.CoolDown.TimeLimit /= 1 - cdReduction;
            }
            else
            {
                unit.Skills.AttackSkill.CoolDown.TimeLimit *= 1 - cdReduction;
            }
		}
        #endregion


        #region Skill on cast effects
        public static void AddBuffToSelf(Effect effect, CombatUnit character)
        {
            //TODO: buff type should be a parameter
            character.ApplyBuff(CoreGameController.Current.BuffFactory.Produce("buff_poison"));
        }
        #endregion


        #region Skill On Hit effects 
        public static void AddBuffToOther(Effect effect, CombatUnit target)
        {
            //TODO: buff type should be a parameter
            target.ApplyBuff(CoreGameController.Current.BuffFactory.Produce("buff_poison"));
        }
        
        #endregion



        #region Update stage effects
        public static void PerSecondLifeReduction(Effect effect, CombatUnit target, float dt)
        {
            target.Attributes.SubstractAttribute(AttributeType.HP_CURRENT, effect.Parameters[0]);
        }

		#endregion

		#region Damage taken stage effects
		#endregion

		//TODO: add other stage effects




	}
}