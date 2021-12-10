using System;
using System.Collections.Generic;
using UnityEngine;

namespace GSStorm.RPG.Engine
{
    /// <summary>
    /// Type of damage.
    /// </summary>
	public enum DamageType{
		FIRE = 1,
		COLD = 2,
		LIGHTNING = 3,
		HOLY = 4,
		DARK = 5,
		PHYSICAL = 6
	}

    /// <summary>
    /// Damage.
    /// </summary>
	public class Damage
	{
        private Dictionary<DamageType, float> _damages;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:GSStorm.RPG.Engine.Damage"/> class.
        /// The value of each damage type will be set to 0.
        /// </summary>
		public Damage(){
            Array damageTypeEnumValues = Enum.GetValues(typeof(DamageType));
            _damages = new Dictionary<DamageType,float> (damageTypeEnumValues.Length);

			//initialize empty value for each damage type
            foreach (DamageType key in damageTypeEnumValues) {
				_damages.Add (key, 0f);
			}
		}

        /// <summary>
        /// Gets or sets the <see cref="T:GSStorm.RPG.Engine.Damage"/> with the specified type.
        /// </summary>
        /// <param name="type">Type.</param>
        public float this[DamageType type]{
            get { return _damages[type]; }
            set { _damages[type] = value; }
        }

        /// <summary>
        /// Gets the damage value to a target.
        /// </summary>
        /// <param name="caster">Caster.</param>
        /// <param name="target">Target.</param>
        public float GetDamageValueToTarge(CombatUnit caster, CombatUnit target){
            //simple test implementation
            float damageValue = 0f;

            //Add up the raw damage
            foreach (float value in _damages.Values)
            {
                damageValue += value;
            }

            //resistency
            damageValue = ProcessResistance(damageValue, target);

            //Armor
            damageValue = ProcessArmour(damageValue, target);

            //evasion
            damageValue = ProcessEvasion(damageValue, target);

            return damageValue;
        }

        private float ProcessResistance(float damageValue, CombatUnit target){
            damageValue -= _damages[DamageType.FIRE] * target.Attributes[AttributeType.RESIS_FIRE].Value;
            damageValue -= _damages[DamageType.COLD] * target.Attributes[AttributeType.RESIS_COLD].Value;
            damageValue -= _damages[DamageType.LIGHTNING] * target.Attributes[AttributeType.RESIS_LIGHTNING].Value;
            damageValue -= _damages[DamageType.HOLY] * target.Attributes[AttributeType.RESIS_HOLY].Value;
            damageValue -= _damages[DamageType.DARK] * target.Attributes[AttributeType.RESIS_DARK].Value;
            damageValue -= _damages[DamageType.PHYSICAL] * target.Attributes[AttributeType.RESIS_PHYSICAL].Value;

            return damageValue;
        }

        private float ProcessArmour(float damageValue, CombatUnit target){
            return damageValue * Mathf.Clamp(100.0f / target.Attributes[AttributeType.ARMOUR_C].Value, 0.3f, 1f);
        }

        private float ProcessEvasion(float damageValue, CombatUnit target)
        {
            float probability = Mathf.Clamp(target.Attributes[AttributeType.EVASION_C].Value / 10000f, 0.1f, 0.5f);
            if (UnityEngine.Random.Range(0f, 1f) < probability)
            {
                //evade success;
                damageValue = 0;
            }
            return damageValue;
        }
	}
}

