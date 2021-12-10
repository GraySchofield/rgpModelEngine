using System;
using UnityEngine;
using System.Collections.Generic;

namespace GSStorm.RPG.Engine
{
    [Serializable]
    public class Effect
    {
        public EffectType Type;
        public List<float> Parameters;

        public Effect(EffectType type, List<float> parameters){
            Type = type;
            Parameters = new List<float>(parameters);
        }

		public Effect(EffectType type)
		{
			Type = type;
			Parameters = new List<float>();
		}
    }
}