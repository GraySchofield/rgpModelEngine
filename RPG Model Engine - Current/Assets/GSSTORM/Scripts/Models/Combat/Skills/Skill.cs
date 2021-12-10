using System;

namespace GSStorm.RPG.Engine
{
    /// <summary>
    /// Super class for skills
    /// </summary>
    [Serializable]
    abstract public class Skill: BaseModel
    {
        /// <summary>
        /// The required level that a character can learn this skill.
        /// </summary>
        public int RequiredLevel;

		/// <summary>
		/// Determine if the character can learn this skill.
		/// </summary>
		/// <returns><c>true</c>, if the character can learn the skill <c>false</c> otherwise.</returns>
		/// <param name="learner">The character to learn the skill.</param>
		public virtual bool CanLearn(CombatUnit learner) {
            if (RequiredLevel > learner.Level) return false;
            return true; 
        }

		/// <summary>
		/// Learn the skill.
		/// </summary>
		/// <returns>whether the character has learnt the skill</returns>
		/// <param name="learner">character to learn the skill</param>
		abstract public bool Learn(CombatUnit learner);

        /// <summary>
        /// Determine whether the character can forget this skill.
        /// </summary>
        /// <returns><c>true</c>, if this skill can be forgot by the character, <c>false</c> otherwise.</returns>
        /// <param name="learner">The character to forget the skill.</param>
		public virtual bool CanForget(CombatUnit learner) { return true; }

		/// <summary>
		/// Let character forget a learnt skill.
		/// </summary>
		/// <returns>if character has forgot the skill.</returns>
		/// <param name="learner">The character to forget the skill.</param>
		abstract public bool Forget(CombatUnit learner);
    }
}
