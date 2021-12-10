using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

namespace GSStorm.RPG.Engine
{
    /// <summary>
    /// Attack skill.
    /// </summary>
    public class AttackSkill : Skill
    {
		/// <summary>
		/// The cool down of this skill
		/// </summary>
		/// <value>The cool down.</value>
		public Timer CoolDown {
			get;
			set;
		}

		/// <summary>
		/// General attribute set.
		/// 
		/// The attack skill will have only one active instances
		/// Though there could be multiple instances of this skill's gameobject
		/// exist in the scene, each refering to the same skill instance
		/// </summary>
		/// <value>The attributes.</value>
		public AttributeSet Attributes { 
			get; 
			private set;
        }

        /// <summary>
        /// The Damage of the skill
        /// </summary>
        public Damage Damage { get; private set; }

        /// <summary>
        /// The timer to indicate whether a skill is prepared.
        /// </summary>
        protected Timer _preparation;

        public AttackSkill(){
            // Defualt to no CD.
			CoolDown = new Timer (0f);

			Attributes = new AttributeSet ();
            Damage = new Damage();

            // Default to no pre attack time.
            _preparation = new Timer(0f);
        }


		//Register the following callbacks 
		//when attaching a rune to the main weapon
		//TODO: we might make the following callbacks in list or dict
		//which may depend on whether we need to pay attention to the execution order of the callbacks
		#region Skill Cast Modifier Delegates

		/// <summary>
		/// Gets or sets the prepare callback.
		/// </summary>
		/// <value>The prepare callback.</value>
		public Action<CombatUnit, AttackSkill> PrepareCallback{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the buff effect callback.
		/// </summary>
		/// <value>The buff effect callback.</value>
		public Action<AttackSkill> BuffEffectCallback {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the on cast callback.
		/// </summary>
		/// <value>The on cast callback.</value>
		public Action<CombatUnit, AttackSkill> OnCastCallback {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the before hit callback.
		/// </summary>
		/// <value>The before hit callback.</value>
		public Action<CombatUnit, AttackSkill, CombatUnit> BeforeHitCallback {
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the on hit callback.
		/// </summary>
		/// <value>The on hit callback.</value>
		public Action<CombatUnit, AttackSkill, CombatUnit> OnHitCallback {
			get;
			set;
		}

		#endregion

        /// <summary>
        /// Learn the skill.
        /// </summary>
        /// <returns>whether the player has learnt the skill</returns>
        /// <param name="learner">Player to learn the skill</param>
		public override bool Learn(CombatUnit learner)
        {
            if (!CanLearn(learner)) return false;

			learner.Skills.LearnSkill(this);

            return true;
        }

        /// <summary>
        /// Let player forget a learnt skill.
        /// </summary>
        /// <returns>if player has forgot the skill.</returns>
        /// <param name="learner">The player to forget the skill.</param>
		public override bool Forget(CombatUnit learner)
        {
            if (!CanForget(learner)) return false;
		
			if (learner.Skills.Forget (TypeId)) {
				return true;
			}

			return false;
        }

        /// <summary>
        /// Prepare the skill.
        /// </summary>
        /// <returns>Whether the prepare process is successful.</returns>
        /// <param name="caster">The caster.</param>
		public virtual bool Prepare(CombatUnit caster){
        	//Check modifiers for this skill at preparation stage
			if(PrepareCallback != null)
				PrepareCallback(caster, this);

			//Check buffs
			if(BuffEffectCallback != null)
				BuffEffectCallback (this);

			//Check CD 
			if (!CoolDown.IsReady) return false;

            // Check & Consume mana, start timers etc.
            float manaToCast = Attributes.GetAttribute(AttributeType.MP_COST).Value;
            if (caster.Mana < manaToCast) return false;

			//Take snapshot from character,etc..

			return true;
        }

        /// <summary>
        /// Cast the skill.
        /// </summary>
		public virtual bool OnCast(CombatUnit caster)
        {
			// Start cooldown count
			CoolDown.Restart();

			//Check rune for this stage
			if(OnCastCallback != null)
				OnCastCallback(caster, this);

            // Create skill behavior (should be done in the derived class)

            return true;
		}

        /// <summary>
        /// Befores the hit.
        /// </summary>
        /// <param name="caster">Caster.</param>
        /// <param name="target">Target.</param>
		public virtual void BeforeHit(CombatUnit caster, CombatUnit target)
        {
            // Skills on hit

            // debuff on hit

			//Check rune for this stage
			if(BeforeHitCallback != null)
				BeforeHitCallback(caster, this, target);
        }

        /// <summary>
        /// Ons the hit.
        /// </summary>
        /// <param name="caster">Caster.</param>
        /// <param name="target">Target.</param>
		public virtual void OnHit(CombatUnit caster, CombatUnit target)
        {
			//Check rune for this stage
			if(OnHitCallback != null)
				OnHitCallback(caster, this, target);

            target.OnTakenDamage(caster, Damage);
        }

        /// <summary>
        /// Get the damage stats.
        /// 
        /// This functional needs to be call when there is an action (equip gear, gain buff etc) affecting damage.
        /// </summary>
        public virtual void CalculateDamage(CombatUnit caster)
        {
            Debug.Log("Start calculating skill damage...");
            Damage[DamageType.COLD] = caster.Attributes[AttributeType.COLD_ATTACK_C].Value;
            Damage[DamageType.DARK] = caster.Attributes[AttributeType.DARK_ATTACK_C].Value;
            Damage[DamageType.LIGHTNING] = caster.Attributes[AttributeType.LIGHTNING_ATTACK_C].Value;
            Damage[DamageType.FIRE] = caster.Attributes[AttributeType.FIRE_ATTACK_C].Value;
            Damage[DamageType.HOLY] = caster.Attributes[AttributeType.HOLY_ATTACK_C].Value;
            Damage[DamageType.PHYSICAL] = caster.Attributes[AttributeType.PHYSICAL_ATTACK_C].Value;
        }
    }
}