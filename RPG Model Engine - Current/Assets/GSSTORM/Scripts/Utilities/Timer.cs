using UnityEngine;
using System;
namespace GSStorm.RPG.Engine
{
	/// <summary>
    /// The state of a timer.
    /// </summary>
	public enum TimerState{
		INIT = 1,
		RUNNING = 2,
		PAUSED = 3,
		FINISHED = 4
	}

    [Serializable]
	public class Timer
	{
		public float Process {
			get;
			private set;
		}

		public float TimeLimit{
			get;
			set;
		}

		public TimerState State {
			get;
			private set;
		}
			
		/// <summary>
		/// When return true, the skill or potion is ready
		/// to be used
        /// 
        /// The timer is ready when the time counting is finished or the <c>TimeLimit</c> is 0f.
		/// </summary>
		/// <value><c>true</c> if this timer is ready; otherwise, <c>false</c>.</value>
		public bool IsReady
        {
            get
            {
                return (TimeLimit < 0.0001f) || (State == TimerState.FINISHED);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:GSStorm.RPG.Engine.Timer"/> class.
        /// </summary>
        /// <param name="limit">Time Limit.</param>
		public Timer(float limit) {
            TimeLimit = limit;
            Reset();
		}

        /// <summary>
        /// Start a timer, if will continue the timer if it was paused.
        /// </summary>
		public void Start(){
            if (TimeLimit < 0.0001f) return;
			State = TimerState.RUNNING;
		}

        /// <summary>
        /// Pause the timer.
        /// </summary>
		public void Pause(){
            if (TimeLimit < 0.0001f) return;
			State = TimerState.PAUSED;
		}

        /// <summary>
        /// Reset the timer.
        /// </summary>
		public void Reset(){
			Process = 0f;
			State = TimerState.INIT;
		}

        /// <summary>
        /// Restart the timer.
        /// 
        /// It will reset it and then start over.
        /// </summary>
        public void Restart(){
            Reset();
            Start();
        }

        /// <summary>
        /// Update with specified delta.
        /// </summary>
        /// <param name="dt">Time detal.</param>
		public void Update(float dt){
			if (State == TimerState.RUNNING) {
				Process += dt;
				if (Process >= TimeLimit) {
                    Process = TimeLimit;
					State = TimerState.FINISHED;
				}
			}
		}
	}

}