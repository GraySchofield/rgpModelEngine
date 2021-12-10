using UnityEngine;
using System.Collections.Generic;
using System;


//TOOD: re Engineer this thing, template classes should
//be as plain and simple as possilble

/// <summary>
/// Rune template.
/// </summary>
namespace GSStorm.RPG.Engine
{
    public class RuneTemplate: ItemTemplate
	{
        public List<EffectList> EffectsInLevel;

        public RuneTemplate(): base(){
            EffectsInLevel = new List<EffectList>();
            // Add a dump template.
            EffectsInLevel.Add(new EffectList());
        }

        public EffectList GetEffects(int level){
            return EffectsInLevel[level];
        }
	}


	/// <summary>
	/// Rune effect for a specific level
	/// </summary>
    [Serializable]    
    public class EffectList{
        public List<Effect> Effects;

        public EffectList(){
            Effects = new List<Effect>();
        }
    }
}

