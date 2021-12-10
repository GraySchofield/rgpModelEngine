using System;
using UnityEngine;
using System.Collections.Generic;

namespace GSStorm.RPG.Engine
{
    public enum EffectType
	{
        NONE = 1,

		//Stage Equip

		ATTACKSKILL_CD_REDUCTION = 2,
		//MANA_AUGMENTATION,
		//MANA_COST_AUGMENTATION,
		//PHYSICS_DAMGE_AUMENTATION

		//Stage Preparation

		//Stage OnCast
        ATTACKSKILL_MULTI_PROJECTILE = 3,
        ATTACKSKILL_ADD_BUFF_TO_SELF = 4,
        ATTACKSKILL_ADD_BUFF_TO_OTHER = 5,
		//Stage BeforeHit, OnHit

        
        //Stage Update
        PER_SECOND_LIFE_REDUCTION = 6
	}

    public static class EffectTypeDefinition
    {
        public static List<Tuple<string, string>> GetParameterDefinition(EffectType type){
            List<Tuple<string, string>> ret = new List<Tuple<string, string>>();
            switch(type)
            {
                case EffectType.ATTACKSKILL_CD_REDUCTION:
                    ret.Add(new Tuple<string, string>("CD Reduction(%)", "float"));
                    break;

				case EffectType.PER_SECOND_LIFE_REDUCTION:
					ret.Add(new Tuple<string, string>("Life Reduction  / per 1S ", "float"));
					break;
			}
            return ret;

        }
    }

}

