using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace GSStorm.RPG.Engine
{
    /// <summary>
    /// Buff effect.
    /// </summary>
    [Serializable]
    public class Buff : BaseModel
    {
		/// <summary>
		/// The attribute set that defines a buff.
		/// 
		/// Note: a dedicated attribute set will be created for each buff.
		/// </summary>
		public AttributeSet Attributes;

		/// <summary>
		/// The order in which, this buff should be applied
		/// </summary>
		public int Priority;

		/// <summary>
		/// Whether this buff can stack
		/// </summary>
		public bool CanStack;


        /// <summary>
        /// How frequent the update effect of buff will take place.
        /// </summary>
        public Timer UpdateRate;


        /// <summary>
		/// The timer for the effect.
		/// 
		/// How long shoud the effect last, could be null
		/// </summary>
		public Timer Duration;


        /// <summary>
        /// Holds the effect types and paramters this buff has
        /// </summary>
        public List<Effect> Effects;


        /// <summary>
        /// The image name of the status icon of this buff
        /// </summary>
        public string ImageFileName;

        /// <summary>
        /// Basic Constructor
        /// </summary>
        public Buff(){
            Attributes = new AttributeSet();
			Effects = new List<Effect> ();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:GSStorm.RPG.Engine.Buff"/> class.
        /// </summary>
        /// <param name="set">Attribute Set</param>
        public Buff(AttributeSet set) : this()
		{
            Attributes.Add(set);
		}


		/// <summary>
		/// Effect of Buff, to be seen when updating the character
		/// </summary>
        public void OnBuffEffectUpdate(float dt, CombatUnit target)
        {
            UpdateRate.Update(dt);
            if(UpdateRate.IsReady)
            {
                // Execute the meat of the effect
                foreach (var effect in Effects)
                {
                    EffectLibrary.OnUpdate(effect, target, UpdateRate.TimeLimit);
                }

                UpdateRate.Restart();
            }
        }

	

		/// <summary>
		/// Effect of Buff, when the character casts a skill or attacks
		/// This happens in the prepare stage of the skill
		/// </summary>
		public void OnBuffEffectCastSkill(CombatUnit caster, AttackSkill skill) {

		}
			

		/// <summary>
		/// Effect of Buff, when the character is taking damage from another source character
		/// </summary>
		public void OnBuffEffectTakenDamage(Damage damage, CombatUnit targert, CombatUnit caster){
			foreach (var effect in Effects) {
				EffectLibrary.OnTakenDamageEffect (effect, damage, caster, targert);
			}
		}
			
    }
}
