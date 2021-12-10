using System.Collections.Generic;

namespace GSStorm.RPG.Engine
{
    /// <summary>
    /// Combat unit buff set.
    /// 
    /// Manages the buffs that currently applied to the combat unit.
    /// </summary>
    public class CombatUnitBuffSet
    {
		/// <summary>
		/// the key is the priority of the buffs
		/// </summary>
        SortedList<int, List<Buff>> _buffIndex;
		List<Buff> _buffs;

        public CombatUnitBuffSet(){
            _buffIndex = new SortedList<int, List<Buff>>();
			_buffs = new List<Buff> ();
		}

        /// <summary>
        /// Add the buff. Start the timer immediately.
        /// </summary>
        /// <param name="buff">The buff.</param>
        public void Add(Buff buff)
        {
            List<Buff> buffs = null;
            if (_buffIndex.ContainsKey(buff.Priority))
            {
                buffs = _buffIndex[buff.Priority];
            }
            else
            {
                buffs = new List<Buff>();
                _buffIndex[buff.Priority] = buffs;
            }

            if (buff.CanStack){
                buffs.Add(buff);
                _buffs.Add(buff);
            } else {
                Buff oldBuff = buffs.Find(v => v.TypeId.Equals(buff.TypeId));
                if (oldBuff == null) { 
                    buffs.Add(buff); 
                    _buffs.Add(buff);
                }
                else {
                    // !!! reuse the old buff
                    buff = oldBuff;
                }
            }

            if (buff.Duration != null){
                buff.Duration.Restart();
            }
        }

        /// <summary>
        /// Remove the specified buff.
        /// </summary>
        /// <param name="buff">Buff.</param>
        public void Remove(Buff buff)
        {
			if (_buffIndex.ContainsKey(buff.Priority))
            {
				List<Buff> buffs = _buffIndex[buff.Priority];
                buffs.Remove(buff);
                if (buffs.Count <= 0)
                {
					_buffIndex.Remove(buff.Priority);
                }
                _buffs.Remove(buff);
            }
        }

        /// <summary>
        /// Gets list of buffs.
        /// </summary>
        /// <returns>The list of buffs.</returns>
        public IEnumerable<Buff> GetBuffs()
        {
            return _buffs;
        }

        /// <summary>
        /// Gets list of buffs for a specific priority.
        /// </summary>
        /// <returns>The list of buffs.</returns>
        /// <param name="priority">Priority.</param>
        public IEnumerable<Buff> GetBuffs(int priority){
			return _buffIndex[priority];
		}

		/// <summary>
		/// Gets the priority list.
		/// 
		/// Sorted from smallest to largest
		/// </summary>
		/// <returns>The priority list.</returns>
        public IEnumerable<int> GetPriorityList(){
            return _buffIndex.Keys;
		}
	
		/// <summary>
		/// Updates the timer for each timed buff.
		/// </summary>
		/// <param name="dt">time delta.</param>
		/// <param name="toBeRemoved" type="out">
		///     List of buffs to be removed, 
		///     this list can be used by the caller to subtract the attribute that gained by this buff.
		/// </param>
		public void UpdateTimer(float dt, out List<Buff> toBeRemoved)
        {
            toBeRemoved = new List<Buff>();

            foreach (Buff buff in _buffs)
            {
                if (buff.Duration != null)
                {
                    Timer timer = buff.Duration;
                    timer.Update(dt);
					if (timer.State == TimerState.FINISHED)
                    {
                        toBeRemoved.Add(buff);
                    }
                }
            }

        }
    }
}