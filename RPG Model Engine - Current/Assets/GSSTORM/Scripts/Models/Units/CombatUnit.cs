using System.Collections.Generic;
using System;
using UnityEngine;

namespace GSStorm.RPG.Engine
{
    /// <summary>
    /// Denote the state of a unit for an attack.
    /// </summary>
	public enum AttackState{
		IDLE = 0,
		PRE_ATTACK = 1,
		ATTACK = 2
	}

    /// <summary>
    /// The Unit that is able to do combat.
    /// Monster should be created from this class.
    /// </summary>
    [Serializable]
    public class CombatUnit: BaseModel
    {
		/// <summary>
		/// Character level.
		/// </summary>
		public int Level { get; set; }
		/// <summary>
		/// Character experience;
		/// </summary>
		public int Experience { get; set; }

        /// <summary>
        /// General attribute set.
        /// </summary>
        /// <value>The attributes.</value>
        public AttributeSet Attributes { get; set; }

        /// <summary>
        /// Set of active buffs.
        /// </summary>
        /// <value>The timed effects.</value>
        public CombatUnitBuffSet Buffs { get; set; }

        /// <summary>
        /// Set of learnt skills.
        /// </summary>
        /// <value>The skills.</value>
        public CombatUnitSkillSet Skills { get; set; }

		/// <summary>
		/// Gear set.
		/// </summary>
		/// <value>The gear set.</value>
		public GearSet Gears { get; set; }

		/// <summary>
		/// gift set.
		/// </summary>
		/// <value>The gear set.</value>
		public GiftSet Gifts { get; set; } 

        /// <summary>
        /// Position of the Combat unit
        /// </summary>
        public SerializableVector3 Position{ get; set; }

        /// <summary>
        /// Rotation of the Combat unit
        /// </summary>
        public SerializableQuaternion Rotation{ get; set; }

		/// <summary>
		/// Only enemy object within this layer will be  
		/// damaged by attack or skills from this player
		/// </summary>
		/// <value>The target enemy layers.</value>
		public List<int> TargetEnemyLayers {
			get;
			private set;
		}

        /// <summary>
        /// The state of the attack.
        /// </summary>
        /// <value>The state of the attack</value>
		public AttackState AttackState {
			get;
			set;
		}

		#region Private Variables

		private bool _isInStateTransition; // Indicate if attack state has changed 
		private float _transitionTime = 0f;

		#endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="T:GSStorm.RPG.Engine.CombatUnit"/> class.
        /// </summary>
        public CombatUnit()
        {
            Attributes = new AttributeSet();
            Buffs = new CombatUnitBuffSet();
            Skills = new CombatUnitSkillSet();
            Gears = new GearSet();
            Gifts = new GiftSet();
            TargetEnemyLayers = new List<int>();
            AttackState = AttackState.IDLE;

			_isInStateTransition = false;
		}

  		#region CombatUnitControls
		//TODO: We need to make a state machine for combat unit to perform its state update and additional functions


        /// <summary>
        /// Loop update for the Combat unit,
		/// 
		/// handles states changes and tasks for combat
		/// unit on an update base
        /// </summary>
        /// <param name="dt">time delta.</param>
		public void UpdateLoop(float dt, Transform transform, Quaternion characterRotation)
        {
            //Set this CombatUnit's transform
            Position = transform.position;

			Rotation = characterRotation;

			//Check if any buff has timed out
            List<Buff> toBeRemove;
            Buffs.UpdateTimer(dt, out toBeRemove);
            foreach (Buff buff in toBeRemove)
            {	
				RemoveBuff (buff);
			}


			//Take effect the buffs in their priority
			foreach (int priority in Buffs.GetPriorityList()) {
                foreach (var buff in Buffs.GetBuffs(priority)) {
					buff.OnBuffEffectUpdate(dt, this);
				}
			}

			//Update attack skill Timers
            if(Skills.AttackSkill != null)
            {
                Timer cd = Skills.AttackSkill.CoolDown;
                if (cd.State == TimerState.INIT)
                {
                    cd.Start();
                }
                cd.Update(dt);
            }

			//Attack States Update
			if (_isInStateTransition) {
				_transitionTime += dt;

				if (AttackState == AttackState.PRE_ATTACK) {
                    //Qian yao zhong
                    if (_transitionTime >= Attributes[AttributeType.PRE_ATTACK_TIME_C].Value) {
						//pre attack finished
						_transitionTime = 0f;  // reset timmer

						Attack ();
					}
				}

				if (AttackState == AttackState.ATTACK) {
                    // start post attack
                    if (_transitionTime >= Attributes[AttributeType.POST_ATTACK_TIME_C].Value) {
						CancelPostAttack ();
					}
				}
			}

            HandleRegeneration(dt);
        }

		/// <summary>
		/// Called when ever a character takes a damage
		/// </summary>
		/// <param name="damageCaster">Damage caster.</param>
		/// <param name="damage">Damage.</param>
		public void OnTakenDamage(CombatUnit damageCaster, Damage damage){
			//Take effect the buffs in their priority
			foreach (int priority in Buffs.GetPriorityList()) {
                foreach (var buff in Buffs.GetBuffs(priority)) {
					buff.OnBuffEffectTakenDamage (damage, this, damageCaster);
				}
			}
				
			//Do the damage calculation
            Attributes.SubstractAttribute(AttributeType.HP_CURRENT, damage.GetDamageValueToTarge(damageCaster, this));
			//TODO: if smaller than 0 , should trigger death

		}

        /// <summary>
        /// Gets the mana.
        /// </summary>
        /// <value>The mana.</value>
        public float Mana
        {
            get
            {
                Attribute manaAttribute = Attributes.GetAttribute(AttributeType.MP_CURRENT);
                return manaAttribute.Value;
            }
        }

        /// <summary>
        /// Consumes the mana.
        /// </summary>
        /// <param name="mana">Mana.</param>
        public void ConsumeMana(float mana)
        {
            Attributes.SubstractAttribute(AttributeType.MP_CURRENT, mana);
        }

        // 攻击三段， 前摇，攻击，后摇

        //We may need to split this process, to be pre, on, post (前摇，攻击，后摇)
        public bool PreAttack(){
			if (AttackState == AttackState.IDLE) {
				if (Skills.AttackSkill.Prepare (this)) {
					Debug.Log ("Ready to use skill : " + Skills.AttackSkill.Name); 
					_isInStateTransition = true;
					AttackState = AttackState.PRE_ATTACK;
                    return true;
                } else {
                    return false;
                }
			}

            return false;
		}

		protected void Attack(){
			if (AttackState == AttackState.PRE_ATTACK) {
				_isInStateTransition = true;
				AttackState = AttackState.ATTACK;
				Skills.AttackSkill.OnCast(this);
				Debug.Log ("Cast skill successful : " + Skills.AttackSkill.Name);
			}
		}

		//取消后摇硬直
		public void CancelPostAttack(){
			//post attack finished
			_transitionTime = 0f;
			AttackState = AttackState.IDLE;
			_isInStateTransition = false;
		}

        //回血回魔
        // need rewrite - better to have a update rate.
        private void HandleRegeneration(float dt){
            Attributes.AddAttribute(AttributeType.HP_CURRENT, Attributes[AttributeType.HP_REGENERATION_C].Value * dt);
            if(Attributes[AttributeType.HP_LIMIT_C].Value < Attributes[AttributeType.HP_CURRENT].Value){
                Attributes.SetAttribute(AttributeType.HP_CURRENT, Attributes[AttributeType.HP_LIMIT_C].Value);
            }
            Attributes.AddAttribute(AttributeType.MP_CURRENT, Attributes[AttributeType.MP_REGENERATION_C].Value * dt);
            if (Attributes[AttributeType.MP_LIMIT_C].Value < Attributes[AttributeType.MP_CURRENT].Value)
            {
                Attributes.SetAttribute(AttributeType.MP_CURRENT, Attributes[AttributeType.MP_LIMIT_C].Value);
            }
        }

        #endregion

        #region SkillManagement

        /// <summary>
        /// Equips an attack skill.
        /// </summary>
        /// <returns><c>true</c>, if attack skill was equiped successfully, <c>false</c> otherwise.</returns>
        /// <param name="typeId">TypeId of the skill.</param>
        public bool EquipAttackSkill(string typeId)
        {
			if (Skills.EquipAttackSkill(typeId))
            {
                UpdateSkillDamage();
                           
                return true;
            }

            return false;
        }

        /// <summary>
        /// Unequip attack skill.
        /// </summary>
        public void UnEquipAttackSkill()
        {
            Skills.UnEquipAttackSkill();
        }

        /// <summary>
        /// Updates players skill damage.
        /// 
        /// This functional needs to be call when there is an action (equip gear, gain buff etc) affecting damage.
        /// </summary>
        public void UpdateSkillDamage(){
            Skills.AttackSkill.CalculateDamage(this);
        }

        #endregion


        #region Buff Logic
        //TODO: we are now assuming buffs can be stacked, this main need to be changed later 

        /// <summary>
        /// A new buff is applied to be combat unit
        /// </summary>
        /// <param name="buff">Buff.</param>
        public void ApplyBuff(Buff buff){
			Attributes.Add (buff.Attributes);
			Buffs.Add (buff);

			//refresh the buff callbacks
			Skills.AttackSkill.BuffEffectCallback += ApplyBuffEffectWhenCastSkill;
		}

		/// <summary>
		/// A buff is timed out or force removed from a combat unit
		/// </summary>
		/// <param name="buff">Buff.</param>
		public void RemoveBuff(Buff buff){
			Attributes.Substract(buff.Attributes);
			Buffs.Remove (buff);
	
			//refresh the buff callbacks
			Skills.AttackSkill.BuffEffectCallback -= ApplyBuffEffectWhenCastSkill;
		}


		/// <summary>
		/// Buff effect delegate when attack skill is used
		/// </summary>
		/// <param name="skill">Skill.</param>
		private void ApplyBuffEffectWhenCastSkill(AttackSkill skill){
			//Apply buff
			foreach (int priority in Buffs.GetPriorityList()) {
                foreach (var buff in Buffs.GetBuffs(priority)) {
					buff.OnBuffEffectCastSkill (this, skill);
				}
			}
		}

		#endregion

        #region Gear logic

        /// <summary>
        /// Equips the gear.
        /// </summary>
        /// <returns><c>true</c>, if gear was equiped, <c>false</c> otherwise.</returns>
        /// <param name="gear">Gear.</param>
        /// <param name="swappedGear">Swapped gear.</param>
        public virtual bool EquipGear(Gear gear, out Gear swappedGear)
        {
            if (!gear.CanEquip(this))
            {
                swappedGear = null;
                return false;
            }

            //Add to the gear set
            Gears.Equip(gear, this, out swappedGear);

            //Update wearing info
            gear.WearingUnit = this;

            //Update the Attributes
            Attributes.Add(gear.Attributes);

            //Update the Runes On Equip
            foreach (RuneSocket socket in gear.RuneSockets)
            {
                if (socket.Rune != null)
                {
                    gear.UpdateRuneOnEquip(socket.Rune);
                }
            }

            //Update skill damage
            UpdateSkillDamage();

            return true;
        }

        /// <summary>
        /// Unequips the gear.
        /// </summary>
        /// <returns>The gear being unequipped.</returns>
        /// <param name="position">Body position.</param>
        public virtual Gear UnequipGear(GearBodyPosition position)
        {
            Gear unequippedGear = Gears.Remove(position);
            if(unequippedGear != null) {
                unequippedGear.WearingUnit = null;

                Attributes.Substract(unequippedGear.Attributes);

                //Update the Runes On Equip
                foreach (RuneSocket socket in unequippedGear.RuneSockets)
                {
                    if (socket.Rune != null)
                    {
                        unequippedGear.UpdateRuneOnUnequip(socket.Rune);
                    }
                }

                //Update skill damage
                UpdateSkillDamage();
            }

            return unequippedGear;
        }

        #endregion


    }
}
