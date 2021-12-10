using System;
using UnityEngine;

namespace GSStorm.RPG.Engine
{
	/// <summary>
	/// Models the socket on a gear, where we can plug a rune into
	/// </summary>
	public class RuneSocket
	{

		/// <summary>
		/// Current socketted rune
		/// </summary>
		/// <value>The current rune.</value>
		public Rune Rune {
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the level.
		/// </summary>
		/// <value>The level.</value>
		public int Level {
			get;
			private set;
		}

		public RuneSocket(int level){
			Level = level;
		}

		/// <summary>
        /// Plugins the rune.
        /// 
        /// This method assumes the socket is empty.
        /// </summary>
        /// <returns><c>true</c>, if rune was plugined, <c>false</c> otherwise.</returns>
        /// <param name="newRune">New rune to be plugined.</param>
		public bool PluginRune(Rune newRune){
            if (Rune != null){
                return false;
            }

            if (newRune.Level < Level)
            {  //TODO: this comparison may be reverted
                return false;
            }

		    Rune = newRune;
            return true;
		}

		/// <summary>
		/// Remove the current socketted rune.
		/// </summary>
		/// <returns>The previous plugged rune.</returns>
		public Rune UnPuginRune(){
			Rune old = Rune;
			Rune = null;
			return old;
		}
	}
}

