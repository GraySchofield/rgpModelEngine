using System;
using UnityEngine;
using System.Collections.Generic;

namespace GSStorm.RPG.Engine
{
	/// <summary>
	/// Character template.
	/// 
	/// Mostly used to create sample monsters
	/// </summary>
	public class CharacterTemplate : BaseTemplate
	{
		public int Level;

		//Attributes are mostly used to store informations in combat
		public List<AttributeTemplate> Attributes;
		public List<string> Skills; //The skills the character can use

		//TODO: we could add more later
	}
}

