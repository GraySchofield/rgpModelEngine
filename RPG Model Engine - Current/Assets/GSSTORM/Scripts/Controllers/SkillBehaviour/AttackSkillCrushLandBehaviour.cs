using UnityEngine;
using System.Collections;

namespace GSStorm.RPG.Engine{
    
	/// <summary>
	/// Attack skill crush land behaviour.
	/// 
	/// controls the life cycle of the attack skill crush land
	/// </summary>
	public class AttackSkillCrushLandBehaviour : AttackSkillBaseBehaviour<AttackSkillCrushLand>
	{
		private Timer _effectTimer;

        private Animator _animator;

		void Awake(){           
			_animator = GetComponent<Animator>();  
		}

		void OnEnable(){
			if (Skill != null) {
				//Start Timer, the life time length of the skill will simply be the length of the animation
				_effectTimer = new Timer (_animator.runtimeAnimatorController.animationClips[0].length);
				_effectTimer.Start();
			}
		}

		void Update ()
		{
			if (Skill != null && Caster != null) {
				//Update Timer
				_effectTimer.Update (Time.deltaTime);

				//Check Timer finish
				if (_effectTimer.State == TimerState.FINISHED) {
	 				//Skill life cycle has ended, destroy it
					_effectTimer.Reset();
					Destroy(gameObject);
				}
			
			}

		}
	}
}

