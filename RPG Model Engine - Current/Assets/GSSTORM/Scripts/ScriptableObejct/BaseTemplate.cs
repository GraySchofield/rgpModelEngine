using UnityEngine;
using System.Collections;

namespace GSStorm.RPG.Engine{
	/// <summary>
	/// Used as base template for all scriptable templates
	/// Should not create an instance assest directly of
	/// this class
	/// </summary>
	public class BaseTemplate : ScriptableObject
	{
		public string TypeId;
		public string Name;
	}
}

