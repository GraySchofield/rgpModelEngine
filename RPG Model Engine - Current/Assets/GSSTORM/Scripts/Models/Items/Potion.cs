using System;

namespace GSStorm.RPG.Engine
{
    [Serializable]
	public class Potion : CountableItem
    {
        public Timer CoolDown { get; set; }

		//TODO: revamp the potion implementation

		// 1. AttributeSet, 永久效果，例如回血，回魔
		// 2. A List of Buffs, 有时间限制的效果

		/// <summary>
		/// Use this potion on the unit
		/// </summary>
		/// <param name="target">The target unit.</param>
        public void UsePotion(CombatUnit target, float cost = 1f){
            if (CanUse (target)) {
				//Consume this potion
				base.Consume(cost);
			
				//Start Cool Down if necessary
				if(CoolDown != null)
					CoolDown.Restart();

				//Produce the effect of this potion
				//TODO: potion should produce a buff while using 
			}
				
		}

		/// <summary>
		/// Check wether the target is valid to 
		/// use the potion
		/// </summary>
		/// <returns><c>true</c>, if use was caned, <c>false</c> otherwise.</returns>
        /// <param name="target">The target unit.</param>
        public bool CanUse(CombatUnit target){
			if (CoolDown != null && !CoolDown.IsReady)
				return false;
			
			return true;
		}
    }
}