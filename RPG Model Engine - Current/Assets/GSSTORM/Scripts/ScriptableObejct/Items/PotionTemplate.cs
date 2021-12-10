using UnityEngine;
using System.Collections;
using GSStorm.RPG.Engine;
using System.Collections.Generic;

public class PotionTemplate : ItemTemplate
{
	public float CoolDown;

	//Permanent Effect
	public List<AttributeTemplate> PermanentAttributes;

	//Timed Effect
	public float TimedEffectEffectiveTime;
	public List<AttributeTemplate> TimedAttributes;

}

