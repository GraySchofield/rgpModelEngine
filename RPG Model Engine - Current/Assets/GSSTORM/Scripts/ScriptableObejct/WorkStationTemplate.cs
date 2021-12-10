using UnityEngine;
using System.Collections.Generic;

namespace GSStorm.RPG.Engine
{
	/// <summary>
	/// Work station stores the collection of 
	/// receipes, this could be for e.g.
	/// a shop, a blacksmith..etc
	/// 
	/// TODO: Later we will provide a super class (Facility template)
	/// </summary>
	[CreateAssetMenu(fileName = "WorkStation", menuName = "Template/WorkStation", order = 5)]
	public class WorkStationTemplate : BaseTemplate
	{
		public List<ReceipeTemplate> ContainedReceipes;
	}
}

