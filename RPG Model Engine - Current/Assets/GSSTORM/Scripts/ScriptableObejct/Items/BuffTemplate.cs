using System;
using UnityEngine;
using System.Collections.Generic;

namespace GSStorm.RPG.Engine
{
	public class BuffTemplate : BaseTemplate
	{
		public List<AttributeTemplate> Attributes;
		public int Priority;
		public bool CanStack;
		public float UpdateFrequency;
        public float LastTime;
		public List<Effect> Effects; //Effect in buff no need to support level

        /// <summary>
        /// The satus image of the buff
        /// </summary>
        public Texture2D BuffIcon;
	}
}

