using System.Collections;
using System.Collections.Generic;
using System;

namespace GSStorm.RPG.Engine
{
	/// <summary>
	/// Combat unit skill set.
	/// models the collection of skills on a combat unit
	/// 
	/// We use the skill stone system
	/// </summary>
    public class CombatUnitSkillSet
    {
        private Dictionary<string, Skill> _learntSkills;
        private AttackSkill _attackSkill;
       
        /// <summary>
        /// Gets the current equipped attack skill.
        /// </summary>
        /// <value>The attack skill.</value>
        public AttackSkill AttackSkill
        {
			get{
				return _attackSkill;
			}
        }


        public CombatUnitSkillSet()
        {
            _learntSkills = new Dictionary<string, Skill>();
        }

        /// <summary>
        /// Add a skill to learnt skill set
        /// </summary>
        /// <param name="skill"></param>
        public void LearnSkill(Skill skill)
        {
            if (!_learntSkills.ContainsKey(skill.TypeId))
            {
                _learntSkills[skill.TypeId] = skill;
            }
        }

		/// <summary>
		/// Check if the attack skill is equipped. Equipped skill can be casted. 
		/// 
		/// </summary>
		/// <returns><c>true</c>, if skill was equipped, <c>false</c> otherwise.</returns>
		/// <param name="typeId">typeId.</param>
		public bool AttackSkillEquipped(string typeId)
		{
            if (_attackSkill == null)
                return false;

            if (_attackSkill.TypeId == typeId)
				return true;

			return false;
		}

        /// <summary>
        /// Equip the attack skill
        /// </summary>
        /// <param name="typeId">Type.</param>
		public bool EquipAttackSkill(string typeId){
            if (!_learntSkills.ContainsKey(typeId))
                return false;

            if (AttackSkillEquipped(typeId))
                return false;

            _attackSkill = _learntSkills[typeId] as AttackSkill;

            return true;
		}

		/// <summary>
		/// UnEquipthe current attack skill
		/// </summary>
		public void UnEquipAttackSkill(){
			_attackSkill = null;
		}

        /// <summary>
        /// Forgets the active skill.
        /// </summary>
        /// <param name="typeId">Type.</param>
        public bool Forget(string typeId)
        {
            if (_learntSkills.ContainsKey(typeId)) {
                _learntSkills.Remove (typeId);
                if (AttackSkillEquipped(typeId)) _attackSkill = null;
				return true;		
			}

			return false;
		}

        /// <summary>
        /// Gets the enumerable learnt skill list.
        /// </summary>
        /// <value>The learnt skill list.</value>
        public IEnumerable<Skill> LearntSkills { get { return _learntSkills.Values; } }

    }
}
